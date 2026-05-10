using System.Collections.Generic;
using System.Linq;


    using KapeBara_Inventory_Management_System;

    public class AuthService
    {
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>();

        public AuthService()
        {
            // seed a default admin user (username: admin, password: admin) - in real apps don't store plain text
            _users["admin"] = "admin";
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;
            return _users.TryGetValue(username, out var stored) && stored == password;
        }

        public bool Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;
            if (_users.ContainsKey(username)) return false;
            _users[username] = password;
            return true;
        }
    }
