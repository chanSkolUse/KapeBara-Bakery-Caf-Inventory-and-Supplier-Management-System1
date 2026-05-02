using System;

namespace KapeBara_Inventory_Management_System
{
    public class PurchaseOrderDetail
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }

        public decimal LineTotal => Quantity * UnitCost;
    }
}