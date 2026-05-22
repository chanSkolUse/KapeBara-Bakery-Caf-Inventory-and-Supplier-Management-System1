using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;

namespace TestProject.Services
{
    public class SupplierService
    {
        private static List<Supplier> _suppliers = new List<Supplier>();
        private static int _nextId = 6;

        static SupplierService()
        {
            _suppliers.Add(new Supplier
            {
                Id = 1,
                Name = "Coffee Importers Inc.",
                ContactPerson = "John Smith",
                Email = "orders@coffeeimporters.com",
                Phone = "02-8123-4567",
                Mobile = "09171234567",
                Address = "123 Coffee St, Manila",
                TaxID = "123-456-789-001",
                IsActive = true
            });
            _suppliers.Add(new Supplier
            {
                Id = 2,
                Name = "Sweet Syrups Co.",
                ContactPerson = "Jane Doe",
                Email = "sales@swtsyrups.com",
                Phone = "02-8234-5678",
                Mobile = "09172345678",
                Address = "456 Syrup Ave, Quezon City",
                TaxID = "123-456-789-002",
                IsActive = true
            });
            _suppliers.Add(new Supplier
            {
                Id = 3,
                Name = "Dairy Direct",
                ContactPerson = "Mike Johnson",
                Email = "info@dairydirect.com",
                Phone = "02-8345-6789",
                Mobile = "09173456789",
                Address = "789 Milk Road, Makati",
                TaxID = "123-456-789-003",
                IsActive = true
            });
            _suppliers.Add(new Supplier
            {
                Id = 4,
                Name = "Flour Mills Corp",
                ContactPerson = "Sarah Wilson",
                Email = "orders@flourmills.com",
                Phone = "02-8456-7890",
                Mobile = "09174567890",
                Address = "321 Flour St, Pasig",
                TaxID = "123-456-789-004",
                IsActive = true
            });
            _suppliers.Add(new Supplier
            {
                Id = 5,
                Name = "Toppings Plus",
                ContactPerson = "Robert Brown",
                Email = "contact@toppingsplus.com",
                Phone = "02-8567-8901",
                Mobile = "09175678901",
                Address = "654 Toppings Lane, Taguig",
                TaxID = "123-456-789-005",
                IsActive = true
            });
        }

        public List<Supplier> GetAllSuppliers() => _suppliers.ToList();
        public List<Supplier> GetActiveSuppliers() => _suppliers.Where(s => s.IsActive).ToList();
        public Supplier GetSupplierById(int id) => _suppliers.FirstOrDefault(s => s.Id == id);

        public void AddSupplier(Supplier supplier)
        {
            supplier.Id = _nextId++;
            supplier.IsActive = true;
            _suppliers.Add(supplier);
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var existing = GetSupplierById(supplier.Id);
            if (existing != null)
            {
                existing.Name = supplier.Name;
                existing.ContactPerson = supplier.ContactPerson;
                existing.Email = supplier.Email;
                existing.Phone = supplier.Phone;
                existing.Mobile = supplier.Mobile;
                existing.Address = supplier.Address;
                existing.TaxID = supplier.TaxID;
            }
        }

        public void DeleteSupplier(int id)
        {
            var supplier = GetSupplierById(id);
            if (supplier != null)
                supplier.IsActive = false;
        }

        public List<Supplier> SearchSuppliers(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return GetAllSuppliers();
            keyword = keyword.ToLower();
            return _suppliers.Where(s =>
                s.Name.ToLower().Contains(keyword) ||
                s.ContactPerson.ToLower().Contains(keyword) ||
                s.Email.ToLower().Contains(keyword)
            ).ToList();
        }
    }
}