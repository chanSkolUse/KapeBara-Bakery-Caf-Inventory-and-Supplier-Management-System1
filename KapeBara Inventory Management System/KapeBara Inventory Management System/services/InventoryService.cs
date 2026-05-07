using System;
using System.Collections.Generic;
using System.Linq;


    using KapeBara_Inventory_Management_System;

    public class InventoryService
    {
        private readonly List<InventoryItem> _items = new List<InventoryItem>();
        private int _nextId = 1;

        public IEnumerable<InventoryItem> GetAll() => _items.ToList();

        public InventoryItem GetByProductId(int productId) => _items.FirstOrDefault(i => i.ProductId == productId);

        public InventoryItem AddOrUpdate(int productId, int quantity, int reorderLevel)
        {
            var item = GetByProductId(productId);
            if (item == null)
            {
                item = new InventoryItem
                {
                    Id = _nextId++,
                    ProductId = productId,
                    QuantityOnHand = quantity,
                    ReorderLevel = reorderLevel,
                    LastUpdated = DateTime.UtcNow
                };
                _items.Add(item);
            }
            else
            {
                item.QuantityOnHand = quantity;
                item.ReorderLevel = reorderLevel;
                item.LastUpdated = DateTime.UtcNow;
            }
            return item;
        }

        public bool AdjustQuantity(int productId, int delta)
        {
            var item = GetByProductId(productId);
            if (item == null) return false;
            item.QuantityOnHand += delta;
            item.LastUpdated = DateTime.UtcNow;
            return true;
        }
    }
