using ServiceApp.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //foreach (var test in Helper.GetContext().Services)
            //{
            //    if (test.MainImagePath == null) continue;
            //    test.MainImagePath = test.MainImagePath.Replace("\\Images\\\\Image\\\\Image\\", "\\Images\\");
            //}
            //Helper.GetContext().SaveChanges();
        }

        private void Sigin_Click(object sender, RoutedEventArgs e)
        {
            CodeWindow codeWindow = new CodeWindow();
            if (Admin.IsChecked == true)
            {
                if (codeWindow.ShowDialog() == true)
                {
                    Helper._isAdmin = true;
                }
            }
            
            ServicesListWindow window = new ServicesListWindow();
            window.Show();
            Close();



        }
    }
}
