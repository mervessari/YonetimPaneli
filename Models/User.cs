using System.ComponentModel;

namespace YonetimPaneli.Models
{
    public class User : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public int Age { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string School { get; set; }
        public int Experience { get; set; }

        public User(string username, string password, string role, string firstName, string lastName, int age, string email, string birthDate, string school, int experience)
        {
            Username = username;
            Password = password;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Email = email;
            BirthDate = birthDate;
            School = school;
            Experience = experience;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
