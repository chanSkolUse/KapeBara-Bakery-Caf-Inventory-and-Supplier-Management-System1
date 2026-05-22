using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Services
{
    public class InventoryService
    {
        private static List<InventoryItem> _inventory = new List<InventoryItem>();
        private static int _nextId = 1;

        static InventoryService()
        {
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 1, QuantityOnHand = 45, ReorderLevel = 20, ReorderQuantity = 50, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 2, QuantityOnHand = 38, ReorderLevel = 20, ReorderQuantity = 50, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 3, QuantityOnHand = 12, ReorderLevel = 15, ReorderQuantity = 30, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 4, QuantityOnHand = 8, ReorderLevel = 15, ReorderQuantity = 30, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 5, QuantityOnHand = 25, ReorderLevel = 15, ReorderQuantity = 30, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 6, QuantityOnHand = 18, ReorderLevel = 10, ReorderQuantity = 50, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 7, QuantityOnHand = 30, ReorderLevel = 15, ReorderQuantity = 30, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 8, QuantityOnHand = 22, ReorderLevel = 15, ReorderQuantity = 30, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 9, QuantityOnHand = 15, ReorderLevel = 10, ReorderQuantity = 20, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 10, QuantityOnHand = 25, ReorderLevel = 10, ReorderQuantity = 20, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 11, QuantityOnHand = 20, ReorderLevel = 10, ReorderQuantity = 20, LastUpdated = DateTime.Now });
            _inventory.Add(new InventoryItem { Id = _nextId++, ProductId = 12, QuantityOnHand = 50, ReorderLevel = 25, ReorderQuantity = 50, LastUpdated = DateTime.Now });
        }

        public List<InventoryItem> GetAll() => _inventory.ToList();
        public InventoryItem GetById(int id) => _inventory.FirstOrDefault(i => i.Id == id);
        public InventoryItem GetByProductId(int productId) => _inventory.FirstOrDefault(i => i.ProductId == productId);

        public void AddOrUpdate(int productId, int quantityOnHand, int reorderLevel, int reorderQuantity = 0)
        {
            var existing = GetByProductId(productId);
            if (existing != null)
            {
                existing.QuantityOnHand = quantityOnHand;
                existing.ReorderLevel = reorderLevel;
                if (reorderQuantity > 0) existing.ReorderQuantity = reorderQuantity;
                existing.LastUpdated = DateTime.Now;
            }
            else
            {
                _inventory.Add(new InventoryItem
                {
                    Id = _nextId++,
                    ProductId = productId,
                    QuantityOnHand = quantityOnHand,
                    ReorderLevel = reorderLevel,
                    ReorderQuantity = reorderQuantity > 0 ? reorderQuantity : 20,
                    LastUpdated = DateTime.Now
                });
            }
        }

        public void UpdateStock(int productId, int quantityChange)
        {
            var item = GetByProductId(productId);
            if (item != null)
            {
                item.QuantityOnHand += quantityChange;
                if (item.QuantityOnHand < 0) item.QuantityOnHand = 0;
                item.LastUpdated = DateTime.Now;
            }
        }

        public List<InventoryItem> GetLowStockItems()
        {
            return _inventory.Where(i => i.QuantityOnHand <= i.ReorderLevel).ToList();
        }

        public decimal GetTotalInventoryValue(ProductService productService)
        {
            decimal total = 0;
            foreach (var item in _inventory)
            {
                var product = productService.GetById(item.ProductId);
                if (product != null)
                {
                    total += product.UnitPrice * item.QuantityOnHand;
                }
            }
            return total;
        }
    }
}