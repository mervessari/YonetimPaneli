using System.Windows;
using System.Windows.Controls;
using YonetimPaneli.Models;

namespace YonetimPaneli.Services
{
    public partial class AddUserWindow : Window
    {
        private UserService userService;

        public AddUserWindow(UserService userService)
        {
            InitializeComponent();
            this.userService = userService;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = ((ComboBoxItem)UserTypeComboBox.SelectedItem)?.Content.ToString();
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            int age = int.TryParse(AgeTextBox.Text, out int a) ? a : 0;
            string email = EmailTextBox.Text;
            string birthDate = BirthDateTextBox.Text;
            string school = SchoolTextBox.Text;
            int experience = int.TryParse(ExperienceTextBox.Text, out int exp) ? exp : 0;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)  || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email)
                        || string.IsNullOrEmpty(birthDate) || string.IsNullOrEmpty(school) 
           ) {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            User newUser = new User(username, password, role, firstName, lastName, age, email, birthDate, school, experience);
            userService.AddUser(newUser);
            MessageBox.Show("Kullanıcı başarıyla eklendi.");
            this.Close();
        }
    }
}
