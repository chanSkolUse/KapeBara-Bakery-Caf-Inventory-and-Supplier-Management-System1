using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;

namespace TestProject.Services
{
    public class AuthService
    {
        private static List<User> _users = new List<User>();
        private static int _nextId = 3;
        private static User _currentUser;

        static AuthService()
        {
            // Default admin user
            _users.Add(new User
            {
                Id = 1,
                FullName = "Admin User",
                EmployeeID = "ADMIN001",
                Age = 30,
                Gender = "Male",
                Email = "admin@kapebara.com",
                ContactNumber = "09123456789",
                Password = "admin123",
                Role = "Admin",
                CreatedAt = DateTime.Now
            });

            // Sample staff user
            _users.Add(new User
            {
                Id = 2,
                FullName = "John Doe",
                EmployeeID = "STAFF001",
                Age = 25,
                Gender = "Male",
                Email = "john.doe@kapebara.com",
                ContactNumber = "09234567890",
                Password = "staff123",
                Role = "Staff",
                CreatedAt = DateTime.Now
            });
        }

        public bool ValidateCredentials(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }

        public bool Register(User user)
        {
            if (_users.Any(u => u.Email == user.Email))
                return false;

            user.Id = _nextId++;
            user.CreatedAt = DateTime.Now;
            _users.Add(user);
            return true;
        }

        public User GetCurrentUser()
        {
            if (_currentUser != null)
            {
                _currentUser = GetUserById(_currentUser.Id);
            }
            return _currentUser;
        }

        public User GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public User GetUserByEmail(string email) => _users.FirstOrDefault(u => u.Email == email);

        public List<User> GetAllUsers() => _users.ToList();

        public void UpdateUser(User user)
        {
            var existing = GetUserById(user.Id);
            if (existing != null)
            {
                existing.FullName = user.FullName;
                existing.Age = user.Age;
                existing.Gender = user.Gender;
                existing.Email = user.Email;
                existing.ContactNumber = user.ContactNumber;
                existing.Role = user.Role;

                if (_currentUser != null && _currentUser.Id == user.Id)
                {
                    _currentUser = existing;
                }
            }
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            var user = GetUserByEmail(email);
            if (user != null && user.Password == oldPassword)
            {
                user.Password = newPassword;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public void RefreshCurrentUser()
        {
            if (_currentUser != null)
            {
                _currentUser = GetUserById(_currentUser.Id);
            }
        }
    }
}