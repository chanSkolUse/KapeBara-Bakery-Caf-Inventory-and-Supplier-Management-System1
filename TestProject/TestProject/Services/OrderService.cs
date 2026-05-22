using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Models;

namespace TestProject.Services
{
    public class OrderService
    {
        private static List<PurchaseOrder> _orders = new List<PurchaseOrder>();
        private static int _nextOrderId = 1000;

        static OrderService()
        {
            // Sample orders with items
            var sampleItems1 = new List<OrderItem>
            {
                new OrderItem { ProductName = "Coffee Beans Arabica", Quantity = 50, UnitPrice = 12.99m },
                new OrderItem { ProductName = "Coffee Beans Robusta", Quantity = 30, UnitPrice = 9.99m }
            };

            _orders.Add(new PurchaseOrder
            {
                OrderId = _nextOrderId++,
                SupplierId = 1,
                Supplier = "Coffee Importers Inc.",
                Items = sampleItems1,
                ItemsSummary = "Coffee Beans Arabica (50kg), Coffee Beans Robusta (30kg)",
                TotalCost = 949.20m,
                OrderDate = DateTime.Now.AddDays(-5),
                ExpectedDelivery = DateTime.Now.AddDays(-2),
                Status = "Delivered",
                Notes = "Delivered on time"
            });

            _orders.Add(new PurchaseOrder
            {
                OrderId = _nextOrderId++,
                SupplierId = 2,
                Supplier = "Sweet Syrups Co.",
                ItemsSummary = "Vanilla, Caramel Syrup (24 bottles each)",
                TotalCost = 408.00m,
                OrderDate = DateTime.Now.AddDays(-3),
                ExpectedDelivery = DateTime.Now.AddDays(2),
                Status = "Shipped",
                Notes = "Track #: SYR-2024-001"
            });

            _orders.Add(new PurchaseOrder
            {
                OrderId = _nextOrderId++,
                SupplierId = 3,
                Supplier = "Dairy Direct",
                ItemsSummary = "Whole Milk (100 cartons)",
                TotalCost = 599.00m,
                OrderDate = DateTime.Now.AddDays(-1),
                ExpectedDelivery = DateTime.Now.AddDays(3),
                Status = "Pending",
                Notes = ""
            });
        }

        public List<PurchaseOrder> GetAllOrders() => _orders.ToList();
        public PurchaseOrder GetOrderById(int id) => _orders.FirstOrDefault(o => o.OrderId == id);

        public void AddOrder(PurchaseOrder order)
        {
            order.OrderId = _nextOrderId++;
            order.OrderDate = DateTime.Now;
            order.Status = "Pending";
            _orders.Add(order);
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = GetOrderById(orderId);
            if (order != null)
                order.Status = status;
        }

        public bool DeleteOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            // Only allow deletion of Pending or Cancelled orders
            if (order != null && (order.Status == "Pending" || order.Status == "Cancelled"))
            {
                return _orders.Remove(order);
            }
            return false;
        }

        public List<PurchaseOrder> GetOrdersByStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return GetAllOrders();
            return _orders.Where(o => o.Status == status).ToList();
        }

        public List<PurchaseOrder> SearchOrders(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return GetAllOrders();
            keyword = keyword.ToLower();
            return _orders.Where(o =>
                o.OrderId.ToString().Contains(keyword) ||
                o.Supplier.ToLower().Contains(keyword) ||
                o.ItemsSummary.ToLower().Contains(keyword)
            ).ToList();
        }

        public List<PurchaseOrder> GetPendingOrders()
        {
            return _orders.Where(o => o.Status != "Delivered" && o.Status != "Cancelled").ToList();
        }
    }
}