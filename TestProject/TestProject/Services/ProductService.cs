using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;

namespace TestProject.Services
{
    public class ProductService
    {
        private static List<Product> _products = new List<Product>();
        private static int _nextId = 1;

        static ProductService()
        {
            // Sample product categories: 1=Coffee, 2=Syrups, 3=Dairy, 4=Toppings, 5=Flour
            _products.Add(new Product { Id = _nextId++, Name = "Coffee Beans - Arabica", CategoryId = 1, CategoryName = "Coffee", UnitPrice = 12.99m, SKU = "COF-AR-001", Description = "Premium Arabica coffee beans" });
            _products.Add(new Product { Id = _nextId++, Name = "Coffee Beans - Robusta", CategoryId = 1, CategoryName = "Coffee", UnitPrice = 9.99m, SKU = "COF-RB-001", Description = "Strong Robusta coffee beans" });
            _products.Add(new Product { Id = _nextId++, Name = "French Vanilla Syrup", CategoryId = 2, CategoryName = "Syrups", UnitPrice = 8.50m, SKU = "SYR-VN-001", Description = "Rich French vanilla flavor" });
            _products.Add(new Product { Id = _nextId++, Name = "Caramel Syrup", CategoryId = 2, CategoryName = "Syrups", UnitPrice = 8.50m, SKU = "SYR-CR-001", Description = "Sweet caramel syrup" });
            _products.Add(new Product { Id = _nextId++, Name = "Hazelnut Syrup", CategoryId = 2, CategoryName = "Syrups", UnitPrice = 8.50m, SKU = "SYR-HZ-001", Description = "Nutty hazelnut flavor" });
            _products.Add(new Product { Id = _nextId++, Name = "Whole Milk", CategoryId = 3, CategoryName = "Dairy", UnitPrice = 5.99m, SKU = "DAY-MK-001", Description = "Fresh whole milk" });
            _products.Add(new Product { Id = _nextId++, Name = "Almond Milk", CategoryId = 3, CategoryName = "Dairy", UnitPrice = 6.99m, SKU = "DAY-AL-001", Description = "Unsweetened almond milk" });
            _products.Add(new Product { Id = _nextId++, Name = "Oat Milk", CategoryId = 3, CategoryName = "Dairy", UnitPrice = 7.49m, SKU = "DAY-OT-001", Description = "Creamy oat milk" });
            _products.Add(new Product { Id = _nextId++, Name = "Whipped Cream", CategoryId = 4, CategoryName = "Toppings", UnitPrice = 4.50m, SKU = "TOP-WC-001", Description = "Aerosol whipped cream" });
            _products.Add(new Product { Id = _nextId++, Name = "Chocolate Sprinkles", CategoryId = 4, CategoryName = "Toppings", UnitPrice = 3.99m, SKU = "TOP-CH-001", Description = "Chocolate sprinkles" });
            _products.Add(new Product { Id = _nextId++, Name = "Caramel Drizzle", CategoryId = 4, CategoryName = "Toppings", UnitPrice = 5.50m, SKU = "TOP-CD-001", Description = "Caramel sauce drizzle" });
            _products.Add(new Product { Id = _nextId++, Name = "All-Purpose Flour", CategoryId = 5, CategoryName = "Flour", UnitPrice = 4.99m, SKU = "FLR-AP-001", Description = "All-purpose flour for baking" });
        }

        public List<Product> GetAll() => _products.ToList();
        public Product GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public List<Product> GetByCategory(int categoryId) => _products.Where(p => p.CategoryId == categoryId).ToList();

        public void Add(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.CategoryId = product.CategoryId;
                existing.CategoryName = product.CategoryName;
                existing.UnitPrice = product.UnitPrice;
                existing.SKU = product.SKU;
                existing.Description = product.Description;
            }
        }

        public void Delete(int id) => _products.RemoveAll(p => p.Id == id);

        public List<Product> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return GetAll();
            keyword = keyword.ToLower();
            return _products.Where(p => p.Name.ToLower().Contains(keyword) || p.SKU.ToLower().Contains(keyword) || p.Description.ToLower().Contains(keyword)).ToList();
        }
    }
}