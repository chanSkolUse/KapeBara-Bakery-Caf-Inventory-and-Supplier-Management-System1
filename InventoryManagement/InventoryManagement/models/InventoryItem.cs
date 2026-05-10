using System;

namespace KapeBara_Inventory_Management_System
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}