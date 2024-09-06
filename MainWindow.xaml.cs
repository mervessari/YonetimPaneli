using System;
using System.Windows;
using YonetimPaneli.Models;
using YonetimPaneli.Panels;
using YonetimPaneli.Services;

namespace YonetimPaneli
{
    public partial class MainWindow : Window
    {
        private UserService userService;

        public MainWindow()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            User user = userService.ValidateUser(username, password);
            if (user != null)
            {
                if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    AdminPanel adminPanel = new AdminPanel(userService, user);
                    adminPanel.Show();
                }
                else
                {
                    StandardPanel standardPanel = new StandardPanel(userService, user);
                    standardPanel.Show();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre", "Giriş Başarısız", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
