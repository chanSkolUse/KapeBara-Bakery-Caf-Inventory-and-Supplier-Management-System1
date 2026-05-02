using System;
using System.Collections.Generic;
using System.Linq;

    using KapeBara_Inventory_Management_System;

    public class PurchaseOrderService
    {
        private readonly List<PurchaseOrder> _orders = new List<PurchaseOrder>();
        private int _nextId = 1;

        public IEnumerable<PurchaseOrder> GetAll() => _orders.ToList();

        public PurchaseOrder GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);

        public PurchaseOrder Create(PurchaseOrder order)
        {
            order.Id = _nextId++;
            order.OrderNumber = order.OrderNumber ?? GenerateOrderNumber(order.Id);
            order.OrderDate = order.OrderDate == default ? DateTime.UtcNow : order.OrderDate;
            order.TotalAmount = CalculateTotal(order);
            _orders.Add(order);
            return order;
        }

        private string GenerateOrderNumber(int id)
        {
            return $"PO-{DateTime.UtcNow:yyyyMMdd}-{id:D4}";
        }

        private decimal CalculateTotal(PurchaseOrder order)
        {
            if (order.Details == null) return 0m;
            decimal total = 0m;
            foreach (var d in order.Details)
            {
                total += d.LineTotal;
            }
            return total;
        }

        public bool AddDetail(int purchaseOrderId, PurchaseOrderDetail detail)
        {
            var order = GetById(purchaseOrderId);
            if (order == null) return false;
            detail.Id = (order.Details.Count == 0) ? 1 : order.Details.Max(x => x.Id) + 1;
            detail.PurchaseOrderId = purchaseOrderId;
            order.Details.Add(detail);
            order.TotalAmount = CalculateTotal(order);
            return true;
        }
    }
