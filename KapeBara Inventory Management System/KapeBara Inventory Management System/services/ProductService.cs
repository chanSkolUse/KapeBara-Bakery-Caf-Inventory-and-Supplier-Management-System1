using System.Collections.Generic;
using System.Linq;


    using KapeBara_Inventory_Management_System;

    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>();
        private int _nextId = 1;

        public IEnumerable<Product> GetAll() => _products.ToList();

        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

        public bool Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing == null) return false;
            existing.Name = product.Name;
            existing.SKU = product.SKU;
            existing.Description = product.Description;
            existing.CategoryId = product.CategoryId;
            existing.UnitPrice = product.UnitPrice;
            existing.SupplierId = product.SupplierId;
            return true;
        }

        public bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;
            return _products.Remove(existing);
        }
    }
