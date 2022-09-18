using Microsoft.Win32;
using ServiceApp.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.Xml.Linq;

namespace ServiceApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditService.xaml
    /// </summary>
    public partial class EditService : Window
    {
        Service _service;
        public EditService()
        {
            InitializeComponent();
            _service = new Service();
            DataContext = _service;
        }
        public EditService(Service service)
        {
            InitializeComponent();
            _service = service;
            DataContext = _service;
            _mainImagePath = _service.AbsolutePath;
            MainImage.Source = new BitmapImage(new Uri(_mainImagePath));
        }
        private void SaveEditChange_Click(object sender, RoutedEventArgs e)
        {
            Helper.GetContext().SaveChanges();
            MessageBox.Show("Данные успешно сохранены");
            this.Close();
        }

        string _mainImagePath;
        private void ImageChange_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg file (*.jpg)|*.jpg|png files (*.png)|*.png| jpeg file (*.jpeg)|*.jpeg";
            if(openFileDialog.ShowDialog() == true)
            {
                _mainImagePath = openFileDialog.FileName;
                MainImage.Source = new BitmapImage(new Uri(_mainImagePath));
                
            }
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {
            if(_service.Id == 0)
            {
                Helper.GetContext().Add(_service);
            }
            _service.MainImagePath = "\\Images\\" + _mainImagePath.Split("\\").LastOrDefault();
            Helper.GetContext().SaveChanges();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg file (*.jpg)|*.jpg|png files (*.png)|*.png| jpeg file (*.jpeg)|*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                var path = openFileDialog.FileName;
                var name = openFileDialog.SafeFileName;
                CopyImage(path, name);
                _service.ServicePhotos.Add(new ServicePhoto()
                {
                    PhotoPath = @$"\Images\{name}",
                });

                ListPhotos.ItemsSource = _service.ServicePhotos.ToList();
            }
        }

        private void CopyImage(string path, string name)
        {
            string projectDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../../"));
            if (!File.Exists($"{projectDir}/Images/{name}"))
            {
                File.Copy(path, $"{projectDir}/Images/{name}");
            }
        }

        private void DeleteImageMethod(string path)
        {
            try
            {
                string projectDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../../"));
                if (File.Exists($"{projectDir}{path}"))
                {
                    File.Delete($"{projectDir}{path}");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if(ListPhotos.SelectedItem is ServicePhoto servicePhoto)
            {
                _service.ServicePhotos.Remove(servicePhoto);
                DeleteImageMethod(servicePhoto.PhotoPath);
                ListPhotos.ItemsSource = _service.ServicePhotos.ToList();
            }
        }
    }
}
