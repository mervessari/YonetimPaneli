using System.Windows;
using YonetimPaneli.Models;
using YonetimPaneli.Panels;
using YonetimPaneli.Services;

namespace YonetimPaneli.Services
{
    public partial class EditUser : Window
    {
        private UserService userService;
        private User currentUser;
        private string oldUsername; 
        private string oldPassword;

        public bool IsAdmin { get; private set; }

        public EditUser(UserService userService, User currentUser)
        {
            InitializeComponent();
            this.userService = userService;
            this.currentUser = currentUser;

            IsAdmin = currentUser.Role == "Admin";
            RoleComboBox.IsEnabled = IsAdmin;

            DataContext = currentUser;

            oldUsername = currentUser.Username; // Eski kullanıcı adını saklayın
            oldPassword = currentUser.Password;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool isUpdated = userService.UpdateUser(currentUser, oldUsername);
            bool isUpdatedPassword = userService.UpdateUser(currentUser, oldPassword);

            if (isUpdated || isUpdatedPassword)
            {
                MessageBox.Show("Kullanıcı bilgileri güncellendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

                // DialogResult değerini true yaparak pencerenin başarıyla kapandığını belirt
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Güncelleme sırasında bir hata oluştu.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }







}
