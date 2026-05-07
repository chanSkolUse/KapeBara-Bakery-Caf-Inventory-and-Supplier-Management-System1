using System.Collections.Generic;
using System.Linq;

using KapeBara_Inventory_Management_System;
public class SupplierService
    {
        private readonly List<Supplier> _suppliers = new List<Supplier>();
        private int _nextId = 1;

        public IEnumerable<Supplier> GetAll() => _suppliers.ToList();

        public Supplier GetById(int id) => _suppliers.FirstOrDefault(s => s.Id == id);

        public Supplier Add(Supplier supplier)
        {
            supplier.Id = _nextId++;
            _suppliers.Add(supplier);
            return supplier;
        }

        public bool Update(Supplier supplier)
        {
            var existing = GetById(supplier.Id);
            if (existing == null) return false;
            existing.Name = supplier.Name;
            existing.ContactName = supplier.ContactName;
            existing.Phone = supplier.Phone;
            existing.Email = supplier.Email;
            existing.Address = supplier.Address;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;
            return _suppliers.Remove(existing);
        }
    }
