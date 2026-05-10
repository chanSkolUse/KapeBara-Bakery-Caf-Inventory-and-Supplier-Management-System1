using System;

namespace KapeBara_Inventory_Management_System
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
        public int SupplierId { get; set; }
    }
}