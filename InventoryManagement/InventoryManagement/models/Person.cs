using System;

namespace KapeBara_Inventory_Management_System
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string FullName => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)
            ? string.Empty
            : string.Format("{0} {1}", FirstName?.Trim(), LastName?.Trim()).Trim();
    }
}