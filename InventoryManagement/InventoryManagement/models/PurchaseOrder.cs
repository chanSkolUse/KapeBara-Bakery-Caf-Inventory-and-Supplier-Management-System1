using System;
using System.Collections.Generic;

namespace KapeBara_Inventory_Management_System
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public decimal TotalAmount { get; set; }

        // navigation
        public List<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
    }
}