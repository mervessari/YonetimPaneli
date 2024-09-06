using System;
using System.Windows;
using System.Windows.Controls;
using YonetimPaneli.Models;
using YonetimPaneli.Services;

namespace YonetimPaneli.Panels
{
    public partial class AdminPanel : Window
    {
        private UserService userService;
        private User currentAdmin;

        public AdminPanel(UserService userService, User currentAdmin)
        {
            InitializeComponent();
            this.userService = userService;
            this.currentAdmin = currentAdmin;

            // Admin bilgilerini bağla
            DataContext = currentAdmin;

            ListUsers();
        }


        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow(userService);
            addUserWindow.Show();
            ListUsers();
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is User user)
            {
                EditUser editUserWindow = new EditUser(userService, user);
                bool? result = editUserWindow.ShowDialog();

                // Eğer değişiklik yapıldıysa listeyi yenile
                if (result == true)
                {
                    ListUsers();
                }
            }
            else
            {
                MessageBox.Show("Güncellemek için bir kullanıcı seçin", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is User user)
            {
                userService.RemoveUser(user.Username);
                ListUsers();
            }
            else
            {
                MessageBox.Show("Silmek için bir kullanıcı seçin", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditMyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            EditUser editUserWindow = new EditUser(userService, currentAdmin);
            bool? result = editUserWindow.ShowDialog();

            // Eğer admin bilgileri değiştirildiyse ekranı yenile
            if (result == true)
            {
                // Admin bilgilerini güncelle ve ekranı yenile
                DataContext = null;
                DataContext = currentAdmin;
                ListUsers();
            }
        }

        private void ListUsers()
        {
            userListBox.ItemsSource = userService.GetAllUsersSorted();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        
   
    }
}
