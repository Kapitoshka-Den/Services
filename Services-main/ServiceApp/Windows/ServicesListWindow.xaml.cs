using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.EntityFrameworkCore.Infrastructure;
using ServiceApp.Models;

namespace ServiceApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для ServicesListWindow.xaml
    /// </summary>
    /// 

    public partial class ServicesListWindow : Window
    {
        private List<string> _sortBy = new()
        { 
        "по возрастанию", "по убыванию"
        };
        private List<string> _filterBy = new()
        { 
        "Все", "от 0 до 5%", "от 10 до 15%", "от 15 до 30%", "от 30 до 70%", "от 70 до 100%"
        };
        int maxSizeList;
        int allElemList;

        ObservableCollection<Service> services;
        public Visibility Visibility { get; set; } = Helper._isAdmin ? Visibility.Visible : Visibility.Collapsed;

        public Visibility isAdmin { get; set; } = Visibility.Collapsed;

        public ServicesListWindow()
        {
            DataContext = Visibility;

            InitializeComponent();
            FiltrCombo.ItemsSource = _filterBy;
            SortCombo.ItemsSource = _sortBy;
            LoadData();
            //var sources = Helper.GetContext().Services.ToList();
            //services = new ObservableCollection<Service>(sources);

            //maxSizeList = services.Count;
            //allElemList = services.Count;
           
        }

        //private void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    Button btn = sender as Button;
        //    int id = Convert.ToInt16(btn.Tag);
        //    var query = Helper.GetContext().Services.Where(s => s.Id == id).FirstOrDefault();

        //    EditService editService = new EditService(query);
        //    editService.ShowDialog();
        //}
        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            EditService editService = new EditService();
            editService.ShowDialog();
            LoadData(); 
        }

       private void LoadData()
        {
            var services = Helper.GetContext().Services.ToList();
            switch (SortCombo.SelectedIndex)
            {
                case 0:
                    services = services.OrderBy(t => t.Discount.HasValue ? t.CostWithDisount : t.Cost).ToList();
                    break;
                case 1:
                    services = services.OrderByDescending(t => t.Discount.HasValue ? t.CostWithDisount : t.Cost).ToList();
                    break;
            }
            switch (FiltrCombo.SelectedIndex)
            {
                case 1:
                    services = services.Where(t => t.Discount < 5 || t.Discount == null).ToList();
                    break;
                case 2:
                    services = services.Where(t => t.Discount >= 5 && t.Discount < 15).ToList();
                    break;
                case 3:
                    services = services.Where(t => t.Discount >= 15 && t.Discount < 30).ToList();
                    break;
                case 4:
                    services = services.Where(t => t.Discount >= 30 && t.Discount < 70).ToList();
                    break;
                case 5:
                    services = services.Where(t => t.Discount >= 70 && t.Discount < 100).ToList();
                    break;
            }

            services = services.Where(x => x.Title.ToLower().Contains(SerachingText.Text.ToLower()) || (!string.IsNullOrWhiteSpace (x.Description) && x.Description.ToLower().Contains(SerachingText.Text))).ToList();
            ServicesList.ItemsSource = services;
            Counter.Text = $"{services.Count} из {Helper.GetContext().Services.Count()}";
        }
        
        

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Service service = (sender as Button).DataContext as Service;

            EditService edit = new EditService(service);
            edit.ShowDialog();
            LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Service service = (sender as Button).DataContext as Service;
            if (service.ClientServices.Count != 0)
            {
                MessageBox.Show("На данную услугу есть записи","Удаление",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            var message = MessageBox.Show("Вы действительно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message == MessageBoxResult.Yes)
            {
                
                Helper.GetContext().Remove(service);
                Helper.GetContext().SaveChanges();
                LoadData();
            }
            
        }

        private void FiltrCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void SerachingText_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }
    }
}
