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
using WpfApp1.Pages;



namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Pages.Autho());
        }

        private void FrmMain_ContentRendered(object sender, EventArgs e)
        {
            if (FrmMain.Content is Pages.Autho)
            {
                btnBack.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnBack.Visibility = Visibility.Visible;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrmMain.CanGoBack)
            {
                FrmMain.GoBack();
            }
        }
    }
}