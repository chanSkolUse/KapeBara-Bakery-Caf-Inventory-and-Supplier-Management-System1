using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestProject.Dialogs;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.ui
{
    public partial class PurchaseOrderForm : Form
    {
        private OrderService _orderService;
        private SupplierService _supplierService;

        public PurchaseOrderForm()
        {
            InitializeComponent();
            _orderService = new OrderService();
            _supplierService = new SupplierService();
            LoadOrders();
            SetupComboBox();
            UpdateStatusCounts();
            WireEvents();
        }

        private void WireEvents()
        {
            button1.Click += CreateOrder_Click;
            button2.Click += FilterOrders_Click;
            dataGridView1.CellClick += DataGridView1_CellClick;
            textBox1.TextChanged += TextBox1_TextChanged;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void SetupComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("All Orders");
            comboBox1.Items.Add("Today");
            comboBox1.Items.Add("This Week");
            comboBox1.Items.Add("This Month");
            comboBox1.Items.Add("Last 30 Days");
            comboBox1.Items.Add("Pending");
            comboBox1.Items.Add("Processing");
            comboBox1.Items.Add("Shipped");
            comboBox1.Items.Add("Delivered");
            comboBox1.Items.Add("Cancelled");
            comboBox1.SelectedIndex = 0;
        }

        private void UpdateStatusCounts()
        {
            var allOrders = _orderService.GetAllOrders();

            lblPendingCount.Text = allOrders.Count(o => o.Status == "Pending").ToString();
            lblProcessingCount.Text = allOrders.Count(o => o.Status == "Processing").ToString();
            lblCompletedCount.Text = allOrders.Count(o => o.Status == "Delivered").ToString();
            lblCancelledCount.Text = allOrders.Count(o => o.Status == "Cancelled").ToString();
        }

        private void LoadOrders()
        {
            dataGridView1.Rows.Clear();
            var orders = _orderService.GetAllOrders().OrderByDescending(o => o.OrderDate);

            foreach (var order in orders)
            {
                AddOrderToGrid(order);
            }

            UpdateStatusCounts();
        }

        private void AddOrderToGrid(PurchaseOrder order)
        {
            int totalQuantity = 1;
            if (order.Items != null && order.Items.Count > 0)
            {
                totalQuantity = order.Items.Sum(i => i.Quantity);
            }

            int rowIndex = dataGridView1.Rows.Add(
                $"#{order.OrderId}",
                order.Supplier,
                order.ItemsSummary,
                totalQuantity.ToString(),
                $"₱{order.TotalCost:N2}",
                order.OrderDate.ToString("MMM dd, yyyy"),
                order.Status,
                "Manage"
            );
            dataGridView1.Rows[rowIndex].Tag = order.OrderId;

            // Color code status cells
            var statusCell = dataGridView1.Rows[rowIndex].Cells["status"];
            statusCell.Style.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            switch (order.Status)
            {
                case "Delivered":
                    statusCell.Style.ForeColor = Color.Green;
                    break;
                case "Pending":
                    statusCell.Style.ForeColor = Color.Orange;
                    break;
                case "Processing":
                    statusCell.Style.ForeColor = Color.Purple;
                    break;
                case "Shipped":
                    statusCell.Style.ForeColor = Color.Blue;
                    break;
                case "Cancelled":
                    statusCell.Style.ForeColor = Color.Red;
                    break;
            }
        }

        private void CreateOrder_Click(object sender, EventArgs e)
        {
            var dialog = new CreateOrderDialog(_supplierService.GetActiveSuppliers());
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newOrder = dialog.GetOrder();
                _orderService.AddOrder(newOrder);
                LoadOrders();
                MessageBox.Show("Order created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void FilterOrders_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = textBox1.Text.Trim().ToLower();
            string filterType = comboBox1.SelectedItem?.ToString() ?? "All Orders";

            var allOrders = _orderService.GetAllOrders();
            var filtered = allOrders.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(o =>
                    o.OrderId.ToString().Contains(searchText) ||
                    o.Supplier.ToLower().Contains(searchText) ||
                    o.ItemsSummary.ToLower().Contains(searchText)
                );
            }

            // Apply status/date filter
            if (filterType == "Pending" || filterType == "Processing" || filterType == "Shipped" || filterType == "Delivered" || filterType == "Cancelled")
            {
                filtered = filtered.Where(o => o.Status == filterType);
            }
            else if (filterType != "All Orders")
            {
                var today = DateTime.Today;

                if (filterType == "Today")
                {
                    filtered = filtered.Where(o => o.OrderDate.Date == today);
                }
                else if (filterType == "This Week")
                {
                    filtered = filtered.Where(o => o.OrderDate.Date >= today.AddDays(-7));
                }
                else if (filterType == "This Month")
                {
                    filtered = filtered.Where(o => o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year);
                }
                else if (filterType == "Last 30 Days")
                {
                    filtered = filtered.Where(o => o.OrderDate.Date >= today.AddDays(-30));
                }
            }

            dataGridView1.Rows.Clear();
            foreach (var order in filtered.OrderByDescending(o => o.OrderDate))
            {
                AddOrderToGrid(order);
            }

            if (filtered.Count() == 0 && !string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("No orders found matching the criteria.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked on Action column (Manage button)
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["action"].Index)
            {
                string orderIdStr = dataGridView1.Rows[e.RowIndex].Cells["orderID"].Value.ToString();
                int orderId = Convert.ToInt32(orderIdStr.Replace("#", ""));
                var order = _orderService.GetOrderById(orderId);

                if (order != null)
                {
                    // Create context menu
                    ContextMenuStrip contextMenu = new ContextMenuStrip();

                    // View Item
                    ToolStripMenuItem viewItem = new ToolStripMenuItem("View Order Details");
                    viewItem.Image = SystemIcons.Information.ToBitmap();
                    viewItem.Click += (s, ev) => {
                        var detailDialog = new OrderDetailDialog(order, _orderService);
                        if (detailDialog.ShowDialog() == DialogResult.OK)
                        {
                            LoadOrders();
                        }
                    };

                    // Separator
                    ToolStripSeparator separator = new ToolStripSeparator();

                    // Delete Item (only enabled for Pending or Cancelled orders)
                    ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete Order");
                    deleteItem.Image = SystemIcons.Warning.ToBitmap();
                    deleteItem.ForeColor = Color.Red;

                    if (order.Status != "Pending" && order.Status != "Cancelled")
                    {
                        deleteItem.Enabled = false;
                        deleteItem.ToolTipText = "Only Pending or Cancelled orders can be deleted";
                    }

                    deleteItem.Click += (s, ev) => {
                        var confirmResult = MessageBox.Show(
                            $"Are you sure you want to delete Order #{order.OrderId}?\n\n" +
                            $"Supplier: {order.Supplier}\n" +
                            $"Total Cost: ₱{order.TotalCost:N2}\n" +
                            $"Status: {order.Status}\n" +
                            $"Items: {order.Items?.Count ?? 1} item(s)\n\n" +
                            "This action cannot be undone.",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmResult == DialogResult.Yes)
                        {
                            bool deleted = _orderService.DeleteOrder(orderId);
                            if (deleted)
                            {
                                LoadOrders();
                                MessageBox.Show($"Order #{order.OrderId} has been deleted successfully.",
                                    "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Unable to delete this order. It may already be processed.",
                                    "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    };

                    contextMenu.Items.Add(viewItem);
                    contextMenu.Items.Add(separator);
                    contextMenu.Items.Add(deleteItem);

                    // Show context menu at mouse position
                    contextMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
                }
            }
        }
    }
}