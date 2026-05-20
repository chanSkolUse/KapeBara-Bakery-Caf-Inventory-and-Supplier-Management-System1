using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Dialogs
{
    public partial class OrderDetailDialog : Form
    {
        private PurchaseOrder _order;
        private OrderService _orderService;

        public OrderDetailDialog(PurchaseOrder order, OrderService orderService)
        {
            InitializeComponent();
            _order = order;
            _orderService = orderService;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            lblOrderId.Text = $"Order #{_order.OrderId}";
            txtSupplier.Text = _order.Supplier;
            txtTotalCost.Text = $"₱{_order.TotalCost:N2}";
            txtOrderDate.Text = _order.OrderDate.ToString("MMMM dd, yyyy hh:mm tt");
            txtExpectedDelivery.Text = _order.ExpectedDelivery.ToString("MMMM dd, yyyy");
            txtNotes.Text = _order.Notes;

            // Load items with quantities into DataGridView
            LoadItemsGrid();

            // Load status combo box
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            cmbStatus.Text = _order.Status;

            // Update summary
            UpdateOrderSummary();

            // Apply status color
            ApplyStatusColor();

            // Enable/disable delete button based on order status
            btnDeleteOrder.Enabled = (_order.Status == "Pending" || _order.Status == "Cancelled");

            // Set tooltip for delete button
            ToolTip tt = new ToolTip();
            if (!btnDeleteOrder.Enabled)
            {
                tt.SetToolTip(btnDeleteOrder, "Only Pending or Cancelled orders can be deleted");
            }
            else
            {
                tt.SetToolTip(btnDeleteOrder, "Delete this order permanently");
            }
        }

        private void ApplyStatusColor()
        {
            switch (_order.Status)
            {
                case "Delivered":
                    cmbStatus.BackColor = Color.FromArgb(220, 255, 220);
                    break;
                case "Pending":
                    cmbStatus.BackColor = Color.FromArgb(255, 245, 220);
                    break;
                case "Shipped":
                    cmbStatus.BackColor = Color.FromArgb(220, 235, 255);
                    break;
                case "Processing":
                    cmbStatus.BackColor = Color.FromArgb(245, 220, 255);
                    break;
                case "Cancelled":
                    cmbStatus.BackColor = Color.FromArgb(255, 220, 220);
                    break;
                default:
                    cmbStatus.BackColor = Color.White;
                    break;
            }
        }

        private void LoadItemsGrid()
        {
            dgvOrderItems.Rows.Clear();

            if (_order.Items != null && _order.Items.Count > 0)
            {
                foreach (var item in _order.Items)
                {
                    dgvOrderItems.Rows.Add(
                        item.ProductName,
                        item.Quantity.ToString(),
                        $"₱{item.UnitPrice:N2}",
                        $"₱{(item.Quantity * item.UnitPrice):N2}"
                    );
                }
            }
            else
            {
                // If no detailed items, try to extract quantity from ItemsSummary
                int estimatedQty = ExtractQuantityFromSummary(_order.ItemsSummary);
                string itemName = _order.ItemsSummary;

                // Try to extract item name without quantity
                var words = _order.ItemsSummary.Split(' ');
                string cleanName = "";
                foreach (var word in words)
                {
                    if (!int.TryParse(word, out _))
                    {
                        cleanName += word + " ";
                    }
                }
                if (string.IsNullOrWhiteSpace(cleanName))
                    cleanName = _order.ItemsSummary;

                dgvOrderItems.Rows.Add(
                    cleanName.Trim(),
                    estimatedQty.ToString(),
                    $"₱{(_order.TotalCost / estimatedQty):N2}",
                    $"₱{_order.TotalCost:N2}"
                );
            }

            // Set column headers
            dgvOrderItems.Columns["colProductName"].HeaderText = "Product / Item";
            dgvOrderItems.Columns["colQuantity"].HeaderText = "Quantity";
            dgvOrderItems.Columns["colUnitPrice"].HeaderText = "Unit Price";
            dgvOrderItems.Columns["colTotal"].HeaderText = "Total";

            // Adjust column widths
            dgvOrderItems.Columns["colProductName"].Width = 200;
            dgvOrderItems.Columns["colQuantity"].Width = 80;
            dgvOrderItems.Columns["colUnitPrice"].Width = 100;
            dgvOrderItems.Columns["colTotal"].Width = 100;
        }

        private int ExtractQuantityFromSummary(string summary)
        {
            if (string.IsNullOrEmpty(summary)) return 1;

            var words = summary.Split(' ');
            foreach (var word in words)
            {
                if (int.TryParse(word, out int qty))
                    return qty;
            }
            return 1;
        }

        private void UpdateOrderSummary()
        {
            int totalItems = _order.Items?.Count ?? 1;
            int totalQuantity = _order.Items?.Sum(i => i.Quantity) ?? ExtractQuantityFromSummary(_order.ItemsSummary);
            decimal totalValue = _order.Items?.Sum(i => i.Quantity * i.UnitPrice) ?? _order.TotalCost;

            lblOrderSummary.Text = $"{totalItems} item(s), {totalQuantity} unit(s) total";
            lblTotalValue.Text = $"₱{totalValue:N2}";
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (cmbStatus.Text != _order.Status)
            {
                // Validate status transition
                if (!IsValidStatusTransition(_order.Status, cmbStatus.Text))
                {
                    MessageBox.Show($"Cannot change status from '{_order.Status}' to '{cmbStatus.Text}'.\n\n" +
                        "Valid transitions:\n" +
                        "• Pending → Processing or Cancelled\n" +
                        "• Processing → Shipped or Cancelled\n" +
                        "• Shipped → Delivered or Cancelled\n" +
                        "• Delivered/Cancelled → No changes allowed",
                        "Invalid Status Change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbStatus.Text = _order.Status;
                    return;
                }

                var confirmResult = MessageBox.Show(
                    $"Change order status from '{_order.Status}' to '{cmbStatus.Text}'?",
                    "Confirm Status Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    _orderService.UpdateOrderStatus(_order.OrderId, cmbStatus.Text);
                    _order.Status = cmbStatus.Text;
                    ApplyStatusColor();

                    // Update delete button state based on new status
                    btnDeleteOrder.Enabled = (_order.Status == "Pending" || _order.Status == "Cancelled");

                    MessageBox.Show("Order status updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    // User cancelled, revert combo box
                    cmbStatus.Text = _order.Status;
                }
            }
            else
            {
                MessageBox.Show("No changes made to status.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            // Check if order can be deleted
            if (_order.Status != "Pending" && _order.Status != "Cancelled")
            {
                MessageBox.Show($"Cannot delete order with status '{_order.Status}'.\n\n" +
                    "Only Pending or Cancelled orders can be deleted.",
                    "Delete Not Allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Show detailed confirmation dialog
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete this order?\n\n" +
                $"Order ID: #{_order.OrderId}\n" +
                $"Supplier: {_order.Supplier}\n" +
                $"Total Cost: ₱{_order.TotalCost:N2}\n" +
                $"Status: {_order.Status}\n" +
                $"Items: {_order.Items?.Count ?? 1} item(s)\n\n" +
                "This action cannot be undone.",
                "Confirm Delete Order",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                bool deleted = _orderService.DeleteOrder(_order.OrderId);
                if (deleted)
                {
                    MessageBox.Show($"Order #{_order.OrderId} has been deleted successfully.",
                        "Delete Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Close the dialog and signal parent to refresh
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Unable to delete this order. It may have already been processed.",
                        "Delete Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            if (currentStatus == newStatus)
                return true;

            if (currentStatus == "Pending" && (newStatus == "Processing" || newStatus == "Cancelled"))
                return true;

            if (currentStatus == "Processing" && (newStatus == "Shipped" || newStatus == "Cancelled"))
                return true;

            if (currentStatus == "Shipped" && (newStatus == "Delivered" || newStatus == "Cancelled"))
                return true;

            if (currentStatus == "Delivered" || currentStatus == "Cancelled")
                return false;

            return false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InitializeComponent()
        {
            this.lblOrderId = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.txtSupplier = new System.Windows.Forms.TextBox();
            this.lblOrderItems = new System.Windows.Forms.Label();
            this.dgvOrderItems = new System.Windows.Forms.DataGridView();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.txtTotalCost = new System.Windows.Forms.TextBox();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.txtOrderDate = new System.Windows.Forms.TextBox();
            this.lblExpectedDelivery = new System.Windows.Forms.Label();
            this.txtExpectedDelivery = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            this.btnDeleteOrder = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblOrderSummary = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.panelLine1 = new System.Windows.Forms.Panel();
            this.panelLine2 = new System.Windows.Forms.Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOrderId
            // 
            this.lblOrderId.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblOrderId.ForeColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.lblOrderId.Location = new System.Drawing.Point(30, 20);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(340, 30);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "Order Details";
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(30, 70);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(48, 13);
            this.lblSupplier.TabIndex = 1;
            this.lblSupplier.Text = "Supplier:";
            // 
            // txtSupplier
            // 
            this.txtSupplier.Location = new System.Drawing.Point(120, 67);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.ReadOnly = true;
            this.txtSupplier.Size = new System.Drawing.Size(350, 20);
            this.txtSupplier.TabIndex = 2;
            // 
            // lblOrderItems
            // 
            this.lblOrderItems.AutoSize = true;
            this.lblOrderItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblOrderItems.Location = new System.Drawing.Point(30, 105);
            this.lblOrderItems.Name = "lblOrderItems";
            this.lblOrderItems.Size = new System.Drawing.Size(88, 17);
            this.lblOrderItems.TabIndex = 3;
            this.lblOrderItems.Text = "Order Items";
            // 
            // dgvOrderItems
            // 
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.AllowUserToDeleteRows = false;
            this.dgvOrderItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderItems.ColumnHeadersHeight = 35;
            this.dgvOrderItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProductName,
            this.colQuantity,
            this.colUnitPrice,
            this.colTotal});
            this.dgvOrderItems.Location = new System.Drawing.Point(30, 125);
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.ReadOnly = true;
            this.dgvOrderItems.RowHeadersVisible = false;
            this.dgvOrderItems.Size = new System.Drawing.Size(500, 150);
            this.dgvOrderItems.TabIndex = 4;
            // 
            // colProductName
            // 
            this.colProductName.HeaderText = "Product";
            this.colProductName.Name = "colProductName";
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.HeaderText = "Unit Price";
            this.colUnitPrice.Name = "colUnitPrice";
            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "colTotal";
            // 
            // panelLine1
            // 
            this.panelLine1.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.panelLine1.Location = new System.Drawing.Point(30, 280);
            this.panelLine1.Name = "panelLine1";
            this.panelLine1.Size = new System.Drawing.Size(500, 1);
            this.panelLine1.TabIndex = 5;
            // 
            // lblOrderSummary
            // 
            this.lblOrderSummary.AutoSize = true;
            this.lblOrderSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblOrderSummary.ForeColor = System.Drawing.Color.Gray;
            this.lblOrderSummary.Location = new System.Drawing.Point(30, 295);
            this.lblOrderSummary.Name = "lblOrderSummary";
            this.lblOrderSummary.Size = new System.Drawing.Size(96, 15);
            this.lblOrderSummary.TabIndex = 6;
            this.lblOrderSummary.Text = "0 item(s), 0 units";
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalCost.Location = new System.Drawing.Point(30, 325);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(80, 17);
            this.lblTotalCost.TabIndex = 7;
            this.lblTotalCost.Text = "Total Cost:";
            // 
            // txtTotalCost
            // 
            this.txtTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.txtTotalCost.ForeColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.txtTotalCost.Location = new System.Drawing.Point(120, 322);
            this.txtTotalCost.Name = "txtTotalCost";
            this.txtTotalCost.ReadOnly = true;
            this.txtTotalCost.Size = new System.Drawing.Size(150, 23);
            this.txtTotalCost.TabIndex = 8;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.lblTotalValue.Location = new System.Drawing.Point(280, 322);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(46, 17);
            this.lblTotalValue.TabIndex = 9;
            this.lblTotalValue.Text = "₱0.00";
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.panelLine2.Location = new System.Drawing.Point(30, 350);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(500, 1);
            this.panelLine2.TabIndex = 10;
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(30, 370);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(62, 13);
            this.lblOrderDate.TabIndex = 11;
            this.lblOrderDate.Text = "Order Date:";
            // 
            // txtOrderDate
            // 
            this.txtOrderDate.Location = new System.Drawing.Point(120, 367);
            this.txtOrderDate.Name = "txtOrderDate";
            this.txtOrderDate.ReadOnly = true;
            this.txtOrderDate.Size = new System.Drawing.Size(280, 20);
            this.txtOrderDate.TabIndex = 12;
            // 
            // lblExpectedDelivery
            // 
            this.lblExpectedDelivery.AutoSize = true;
            this.lblExpectedDelivery.Location = new System.Drawing.Point(30, 400);
            this.lblExpectedDelivery.Name = "lblExpectedDelivery";
            this.lblExpectedDelivery.Size = new System.Drawing.Size(90, 13);
            this.lblExpectedDelivery.TabIndex = 13;
            this.lblExpectedDelivery.Text = "Expected Delivery:";
            // 
            // txtExpectedDelivery
            // 
            this.txtExpectedDelivery.Location = new System.Drawing.Point(120, 397);
            this.txtExpectedDelivery.Name = "txtExpectedDelivery";
            this.txtExpectedDelivery.ReadOnly = true;
            this.txtExpectedDelivery.Size = new System.Drawing.Size(280, 20);
            this.txtExpectedDelivery.TabIndex = 14;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(30, 435);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 15;
            this.lblStatus.Text = "Status:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(120, 432);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 21);
            this.cmbStatus.TabIndex = 16;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(30, 470);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.TabIndex = 17;
            this.lblNotes.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(120, 467);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(350, 60);
            this.txtNotes.TabIndex = 18;
            // 
            // btnUpdateStatus
            // 
            this.btnUpdateStatus.BackColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.btnUpdateStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateStatus.ForeColor = System.Drawing.Color.White;
            this.btnUpdateStatus.Location = new System.Drawing.Point(230, 545);
            this.btnUpdateStatus.Name = "btnUpdateStatus";
            this.btnUpdateStatus.Size = new System.Drawing.Size(100, 35);
            this.btnUpdateStatus.TabIndex = 19;
            this.btnUpdateStatus.Text = "Update Status";
            this.btnUpdateStatus.UseVisualStyleBackColor = false;
            this.btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);
            // 
            // btnDeleteOrder
            // 
            this.btnDeleteOrder.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDeleteOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteOrder.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOrder.Location = new System.Drawing.Point(120, 545);
            this.btnDeleteOrder.Name = "btnDeleteOrder";
            this.btnDeleteOrder.Size = new System.Drawing.Size(100, 35);
            this.btnDeleteOrder.TabIndex = 20;
            this.btnDeleteOrder.Text = "Delete Order";
            this.btnDeleteOrder.UseVisualStyleBackColor = false;
            this.btnDeleteOrder.Click += new System.EventHandler(this.btnDeleteOrder_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(340, 545);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // OrderDetailDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(560, 610);
            this.Controls.Add(this.btnDeleteOrder);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdateStatus);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtExpectedDelivery);
            this.Controls.Add(this.lblExpectedDelivery);
            this.Controls.Add(this.txtOrderDate);
            this.Controls.Add(this.lblOrderDate);
            this.Controls.Add(this.panelLine2);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.txtTotalCost);
            this.Controls.Add(this.lblTotalCost);
            this.Controls.Add(this.lblOrderSummary);
            this.Controls.Add(this.panelLine1);
            this.Controls.Add(this.dgvOrderItems);
            this.Controls.Add(this.lblOrderItems);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.lblOrderId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderDetailDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Order Details";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.TextBox txtSupplier;
        private System.Windows.Forms.Label lblOrderItems;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.TextBox txtTotalCost;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.TextBox txtOrderDate;
        private System.Windows.Forms.Label lblExpectedDelivery;
        private System.Windows.Forms.TextBox txtExpectedDelivery;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnUpdateStatus;
        private System.Windows.Forms.Button btnDeleteOrder;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblOrderSummary;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.Panel panelLine2;
    }
}