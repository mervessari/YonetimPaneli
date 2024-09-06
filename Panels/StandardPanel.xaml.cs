using System;
using System.Windows;
using YonetimPaneli.Models;
using YonetimPaneli.Services;

namespace YonetimPaneli.Panels
{
    public partial class StandardPanel : Window
    {
        private UserService userService;
        private User currentUser;

        public StandardPanel(UserService userService, User currentUser)
        {
            InitializeComponent();
            this.userService = userService;
            this.currentUser = currentUser;

            DataContext = currentUser;

            // Kullanıcıları listele
            ListUsers();
        }
   
    private void ListUsers()
        {

            userListBox.ItemsSource = null;
            userListBox.ItemsSource = userService.GetAllUsersSorted();
        }

        private void EditMyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            EditUser editUserWindow = new EditUser(userService, currentUser);
            bool? result = editUserWindow.ShowDialog();

            // Eğer değişiklik yapıldıysa veriyi güncelle
            if (result == true)
            {
                DataContext = null; // Önce veriyi temizle
                DataContext = currentUser; // Sonra güncellenmiş veriyi tekrar bağla
                ListUsers();
            }
        }

        private void UpdateUserButton_Click(object sender, RoutedEventArgs e)
        {
            EditUser editUserWindow = new EditUser(userService, currentUser);
            editUserWindow.ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


    
    }
}
