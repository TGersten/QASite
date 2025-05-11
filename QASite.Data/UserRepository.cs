using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace QASite.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            var ctx = new QuestionsDataContext(_connectionString);
            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            user.PasswordHash = hash;
            
            ctx.Users.Add(user);
            ctx.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);

            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (isValidPassword)
            {
                return user;

            }
            return null;
        }

        public User GetUserByEmail(string email)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
