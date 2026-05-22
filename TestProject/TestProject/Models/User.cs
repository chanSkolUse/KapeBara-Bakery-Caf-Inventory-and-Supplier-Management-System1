using System;

namespace TestProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmployeeID { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}