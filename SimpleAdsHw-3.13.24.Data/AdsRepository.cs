using BCrypt.Net;
using System.Data.SqlClient;

namespace Homework__3._13._24.Data
{
    public class AdsRepository
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=HomeWork; Integrated Security=true;";

        public List<Ad> GetAds(int userId = 0)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            var byUserId = userId != 0 ? "Where a.UserId = @id" : "";
            command.CommandText = $"SELECT *, (SELECT Name FROM Users u WHERE u.Id = a.userId) AS 'Name' FROM Ads a {byUserId}";
            command.Parameters.AddWithValue("@id", userId);
            connection.Open();
            var reader = command.ExecuteReader();
            var ads = new List<Ad>();

            while (reader.Read())
            {
                ads.Add(new()
                {
                    Description = (string)reader["Description"],
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Date = (DateTime)reader["Date"],
                    Name = (string)reader["Name"]
                });
            }
            return ads;
        }

        public void NewAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO ADS (Description, Date, UserId, PhoneNumber) VALUES (@description, @date, @userId, @phoneNumber)";
            command.Parameters.AddWithValue("@description", ad.Description);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Parameters.AddWithValue("@userId", ad.UserId);
            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void AddUser(User user, string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Users (Name, Email, PasswordHash) VALUES (@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", hash);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User GetUserByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }

        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);

            return user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? null : user;
        }
    }
}