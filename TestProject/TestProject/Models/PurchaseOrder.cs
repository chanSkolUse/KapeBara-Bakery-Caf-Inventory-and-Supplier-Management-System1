using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class PurchaseOrder
    {
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public List<OrderItem> Items { get; set; }
        public string ItemsSummary { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDelivery { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
