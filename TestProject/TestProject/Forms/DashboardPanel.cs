using TestProject.Services;
using System;
using System.Reflection.Emit;
using System.Windows.Forms;
using TestProject.Services;

namespace TestProject.ui
{
    public partial class dashboardPanel : Form
    {
        private ProductService _productService;
        private InventoryService _inventoryService;
        private OrderService _orderService;

        public dashboardPanel()
        {
            InitializeComponent();
            _productService = new ProductService();
            _inventoryService = new InventoryService();
            _orderService = new OrderService();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Update total items
            var totalItems = _productService.GetAll().Count;
            button1.Text = $"Total Items\n{totalItems}";

            // Update low stock alerts
            var lowStockItems = _inventoryService.GetLowStockItems();
            button2.Text = $"Low Stock Alerts\n{lowStockItems.Count} items";

            // Update to be delivered
            var pendingOrders = _orderService.GetPendingOrders();
            button4.Text = $"To be Delivered\n{pendingOrders.Count} orders";

            // Update to be ordered (low stock items that need reordering)
            var needReorder = lowStockItems.Count;
            button3.Text = $"To be Ordered\n{needReorder} items";

            // Update total inventory value
            var totalValue = _inventoryService.GetTotalInventoryValue(_productService);
            label7.Text = $"₱ {totalValue:N2}";

            // Update monthly spend (sample calculation)
            var monthlyOrders = _orderService.GetAllOrders().FindAll(o => o.OrderDate.Month == DateTime.Now.Month);
            decimal monthlySpend = 0;
            foreach (var order in monthlyOrders)
            {
                monthlySpend += order.TotalCost;
            }
            label6.Text = $"Monthly Spend: ₱ {monthlySpend:N2}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Total Products: {_productService.GetAll().Count}", "Inventory Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lowStock = _inventoryService.GetLowStockItems();
            if (lowStock.Count > 0)
            {
                string message = "Low Stock Items:\n";
                foreach (var item in lowStock)
                {
                    var product = _productService.GetById(item.ProductId);
                    message += $"- {product?.Name}: {item.QuantityOnHand} units (Reorder at {item.ReorderLevel})\n";
                }
                MessageBox.Show(message, "Low Stock Alerts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("No low stock items.", "Inventory Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var pendingOrders = _orderService.GetPendingOrders();
            if (pendingOrders.Count > 0)
            {
                string message = "Pending Deliveries:\n";
                foreach (var order in pendingOrders)
                {
                    message += $"- Order #{order.OrderId}: {order.Supplier} - {order.Status}\n";
                }
                MessageBox.Show(message, "Pending Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No pending orders.", "Order Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}