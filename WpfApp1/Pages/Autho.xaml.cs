using System;
using System.Collections.Generic;
using System.Data;
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
using WpfApp1;      
using WpfApp1.Services;




namespace WpfApp1.Pages
{
    public partial class Autho : Page
    {
        int click;

        public Autho()
        {
            InitializeComponent();
            click = 0;

           
            tbCaptcha.Visibility = Visibility.Collapsed;
            tblCaptcha.Visibility = Visibility.Collapsed;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            tbLogin.Clear();
            tbPassword.Clear();
            tbCaptcha.Clear();
            tbCaptcha.Visibility = Visibility.Collapsed;
            tblCaptcha.Visibility = Visibility.Collapsed;

            NavigationService.Navigate(new Client(null, "Гость"));
        }

        private void GenerateCapctcha()
        {
            tbCaptcha.Visibility = Visibility.Visible;
            tblCaptcha.Visibility = Visibility.Visible;

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.Text = capctchaText;
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;

            tbCaptcha.Clear();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;

            string login = tbLogin.Text.Trim();
            string password = tbPassword.Text.Trim();

            
            string hashedPassword = Hash.HashPassword(password);

            AgencyEntities db = new AgencyEntities();

            var user = db.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == hashedPassword);

            if (click == 1)
            {
                if (user != null)
                {
                    string role = GetRoleByUserId(db, user.UserID);

                   
                    MessageBox.Show("Вы вошли под ролью: " + role, "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    
                    Dispatcher.InvokeAsync(() =>
                    {
                        LoadPage(role, user);
                    });
                }
                else
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbPassword.Clear();
                    GenerateCapctcha();
                }
            }
            else if (click > 1)
            {
                if (user != null && tbCaptcha.Text.Trim() == tblCaptcha.Text)
                {
                    string role = GetRoleByUserId(db, user.UserID);

                    MessageBox.Show("Вы вошли под ролью: " + role, "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    Dispatcher.InvokeAsync(() =>
                    {
                        LoadPage(role, user);
                    });
                }
                else
                {
                    MessageBox.Show("Введите данные заново!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbPassword.Clear();
                    tbCaptcha.Clear();
                    GenerateCapctcha();
                }
            }
        }

        private string GetRoleByUserId(AgencyEntities db, int userId)
        {
            if (db.Agents.Any(a => a.UserID == userId))
                return "Агент";
            if (db.Buyers.Any(b => b.UserID == userId))
                return "Покупатель";
            if (db.Sellers.Any(s => s.UserID == userId))
                return "Продавец";

            return "Пользователь";
        }

        private void LoadPage(string role, Users user)
        {
            click = 0;

            tbLogin.Clear();
            tbPassword.Clear();
            tbCaptcha.Clear();
            tbCaptcha.Visibility = Visibility.Collapsed;
            tblCaptcha.Visibility = Visibility.Collapsed;

            switch (role)
            {
                case "Агент":
                    NavigationService.Navigate(new Pages.Agent(user, role));
                    break;
                case "Покупатель":
                    NavigationService.Navigate(new Pages.Buyer(user, role));
                    break;
                case "Продавец":
                    NavigationService.Navigate(new Pages.Seller(user, role));
                    break;
                default:
                    NavigationService.Navigate(new Pages.Client(user, role));
                    break;
            }
        }
    }
}