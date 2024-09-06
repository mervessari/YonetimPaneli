using System;
using System.Collections.Generic;
using System.Data.SQLite;
using YonetimPaneli.Models;

namespace YonetimPaneli.Services
{
    public class UserService
    {
        private string connectionString = "Data Source=your_database_file_path.db;Version=3;";

        public UserService()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Username TEXT PRIMARY KEY,
                    Password TEXT,
                    FirstName TEXT,
                    LastName TEXT,
                    Age INTEGER,
                    Email TEXT,
                    BirthDate TEXT,
                    School TEXT,
                    Experience INTEGER,
                    Role TEXT
                );";
                command.ExecuteNonQuery();

                // Varsayılan admin kullanıcısını ekle
                AddDefaultAdminIfNotExists(connection);
            }
        }

        public List<User> GetAllUsersSorted()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users ORDER BY Username";

                var users = new List<User>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Boş alanlar olup olmadığını kontrol ediyoruz
                        if (!string.IsNullOrWhiteSpace(reader["Username"].ToString()) &&
                            !string.IsNullOrWhiteSpace(reader["Password"].ToString()) &&
                            !string.IsNullOrWhiteSpace(reader["Role"].ToString()))
                        {
                            var user = new User(
                                reader["Username"].ToString(),
                                reader["Password"].ToString(),
                                reader["Role"].ToString(),
                                reader["FirstName"].ToString(),
                                reader["LastName"].ToString(),
                                int.Parse(reader["Age"].ToString()),
                                reader["Email"].ToString(),
                                reader["BirthDate"].ToString(),
                                reader["School"].ToString(),
                                int.Parse(reader["Experience"].ToString())
                            );

                            users.Add(user);
                        }
                    }
                }
                return users;
            }
        }


        public void RemoveUser(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Users WHERE Username = @username";
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
        }

        private void AddDefaultAdminIfNotExists(SQLiteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            command.Parameters.AddWithValue("@username", "admin");

            long count = (long)command.ExecuteScalar();
            if (count == 0)
            {
                command.CommandText = @"
                INSERT INTO Users (Username, Password, FirstName, LastName, Age, Email, BirthDate, School, Experience, Role)
                VALUES (@username, @password, @firstName, @lastName, @age, @email, @birthDate, @school, @experience, @role)";
                command.Parameters.AddWithValue("@username", "admin");
                command.Parameters.AddWithValue("@password", "admin");
                command.Parameters.AddWithValue("@firstName", "Admin");
                command.Parameters.AddWithValue("@lastName", "Admin");
                command.Parameters.AddWithValue("@age", 30);
                command.Parameters.AddWithValue("@email", "admin@example.com");
                command.Parameters.AddWithValue("@birthDate", "01/01/1990");
                command.Parameters.AddWithValue("@school", "Admin School");
                command.Parameters.AddWithValue("@experience", 10);
                command.Parameters.AddWithValue("@role", "Admin");
                command.ExecuteNonQuery();
            }
        }

        public User ValidateUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT * FROM Users 
                WHERE Username = @username AND Password = @password";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User(
                            reader["Username"].ToString(),
                            reader["Password"].ToString(),
                            reader["Role"].ToString(),
                            reader["FirstName"].ToString(),
                            reader["LastName"].ToString(),
                            reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0,
                            reader["Email"].ToString(),
                            reader["BirthDate"].ToString(),
                            reader["School"].ToString(),
                            reader["Experience"] != DBNull.Value ? Convert.ToInt32(reader["Experience"]) : 0
                        );
                    }
                }
            }
            return null;
        }

        public bool AddUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Kullanıcı adının var olup olmadığını kontrol et
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                command.Parameters.AddWithValue("@username", user.Username);
                long count = (long)command.ExecuteScalar();
                if (count > 0)
                {
                    return false; // Kullanıcı zaten var
                }

                command.CommandText = @"
                INSERT INTO Users (Username, Password, FirstName, LastName, Age, Email, BirthDate, School, Experience, Role)
                VALUES (@username, @password, @firstName, @lastName, @age, @email, @birthDate, @school, @experience, @role)";
                command.Parameters.AddWithValue("@password", user.Password);
                command.Parameters.AddWithValue("@firstName", user.FirstName);
                command.Parameters.AddWithValue("@lastName", user.LastName);
                command.Parameters.AddWithValue("@age", user.Age);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@birthDate", user.BirthDate);
                command.Parameters.AddWithValue("@school", user.School);
                command.Parameters.AddWithValue("@experience", user.Experience);
                command.Parameters.AddWithValue("@role", user.Role);
                command.ExecuteNonQuery();

                return true;
            }
        }

        public bool UpdateUser(User user, string oldUsername)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                UPDATE Users
                SET Username = @NewUsername,
                    Password = @Password,
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Age = @Age,
                    Email = @Email,
                    BirthDate = @BirthDate,
                    School = @School,
                    Experience = @Experience,
                    Role = @Role
                WHERE Username = @OldUsername";

                command.Parameters.AddWithValue("@NewUsername", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Age", user.Age);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                command.Parameters.AddWithValue("@School", user.School);
                command.Parameters.AddWithValue("@Experience", user.Experience);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@OldUsername", oldUsername);

                int rowsUpdated = command.ExecuteNonQuery();
                return rowsUpdated > 0;
            }
        }
    }
}
