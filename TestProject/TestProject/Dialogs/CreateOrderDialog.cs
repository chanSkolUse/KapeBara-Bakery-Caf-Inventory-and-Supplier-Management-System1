using TestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestProject.Dialogs
{
    public partial class CreateOrderDialog : Form
    {
        private List<Supplier> _suppliers;
        private PurchaseOrder _newOrder = new PurchaseOrder();
        private List<OrderItem> _orderItems = new List<OrderItem>();
        private decimal _currentTotalCost = 0;

        public CreateOrderDialog(List<Supplier> suppliers)
        {
            InitializeComponent();
            _suppliers = suppliers;
            LoadSuppliers();
            SetupDataGridView();
            UpdateTotalDisplay();
            WireEvents();
        }

        private void WireEvents()
        {
            btnAddItem.Click += BtnAddItem_Click;
            btnRemoveItem.Click += BtnRemoveItem_Click;
            btnCreate.Click += BtnCreate_Click;
            btnCancel.Click += BtnCancel_Click;
            numQuantity.ValueChanged += (s, e) => UpdateItemTotal();
            txtUnitPrice.TextChanged += (s, e) => UpdateItemTotal();
            cmbSupplier.SelectedIndexChanged += CmbSupplier_SelectedIndexChanged;
            dtpDeliveryDate.ValueChanged += (s, e) => UpdateDateWarning();
        }

        private void SetupDataGridView()
        {
            dgvItems.Columns.Clear();
            dgvItems.Columns.Add("colProduct", "Product/Item");
            dgvItems.Columns.Add("colQuantity", "Quantity");
            dgvItems.Columns.Add("colUnitPrice", "Unit Price");
            dgvItems.Columns.Add("colTotal", "Total");

            dgvItems.Columns["colProduct"].Width = 200;
            dgvItems.Columns["colQuantity"].Width = 80;
            dgvItems.Columns["colUnitPrice"].Width = 100;
            dgvItems.Columns["colTotal"].Width = 100;

            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.ReadOnly = true;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadSuppliers()
        {
            cmbSupplier.Items.Clear();
            foreach (var supplier in _suppliers)
            {
                cmbSupplier.Items.Add(supplier.Name);
            }
            if (cmbSupplier.Items.Count > 0)
                cmbSupplier.SelectedIndex = 0;
        }

        private void CmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedIndex >= 0 && cmbSupplier.SelectedIndex < _suppliers.Count)
            {
                var supplier = _suppliers[cmbSupplier.SelectedIndex];
                lblSupplierContact.Text = $"Contact: {supplier.ContactPerson} | {supplier.Phone}";
            }
        }

        private void UpdateDateWarning()
        {
            var daysDiff = (dtpDeliveryDate.Value - DateTime.Now).Days;
            if (daysDiff < 1)
            {
                lblDeliveryWarning.Text = "⚠ Delivery date should be at least 1 day from now";
                lblDeliveryWarning.ForeColor = System.Drawing.Color.Orange;
            }
            else if (daysDiff > 30)
            {
                lblDeliveryWarning.Text = "⚠ Long delivery time (over 30 days)";
                lblDeliveryWarning.ForeColor = System.Drawing.Color.Orange;
            }
            else
            {
                lblDeliveryWarning.Text = "✓ Delivery date looks good";
                lblDeliveryWarning.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void UpdateItemTotal()
        {
            if (decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) && numQuantity.Value > 0)
            {
                decimal itemTotal = unitPrice * numQuantity.Value;
                lblItemTotal.Text = $"Item Total: ₱{itemTotal:N2}";
            }
            else
            {
                lblItemTotal.Text = "Item Total: ₱0.00";
            }
        }

        private void UpdateTotalDisplay()
        {
            _currentTotalCost = _orderItems.Sum(i => i.Quantity * i.UnitPrice);
            lblGrandTotal.Text = $"₱{_currentTotalCost:N2}";
        }

        private void UpdateItemsGrid()
        {
            dgvItems.Rows.Clear();
            foreach (var item in _orderItems)
            {
                dgvItems.Rows.Add(
                    item.ProductName,
                    item.Quantity.ToString(),
                    $"₱{item.UnitPrice:N2}",
                    $"₱{(item.Quantity * item.UnitPrice):N2}"
                );
            }
            UpdateTotalDisplay();
            UpdateOrderSummary();
        }

        private void UpdateOrderSummary()
        {
            int totalItems = _orderItems.Count;
            int totalQuantity = _orderItems.Sum(i => i.Quantity);
            lblOrderSummary.Text = $"{totalItems} item(s), {totalQuantity} unit(s) total";
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Please enter a product/item name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Please enter a valid quantity (minimum 1).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numQuantity.Focus();
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Please enter a valid unit price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return;
            }

            var existingItem = _orderItems.FirstOrDefault(i => i.ProductName.Equals(txtProductName.Text, StringComparison.OrdinalIgnoreCase));
            if (existingItem != null)
            {
                var result = MessageBox.Show($"'{txtProductName.Text}' already exists. Add to existing quantity?",
                    "Duplicate Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    existingItem.Quantity += (int)numQuantity.Value;
                    decimal totalValue = (existingItem.Quantity - (int)numQuantity.Value) * existingItem.UnitPrice + (numQuantity.Value * unitPrice);
                    existingItem.UnitPrice = totalValue / existingItem.Quantity;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _orderItems.Add(new OrderItem
                {
                    ProductName = txtProductName.Text,
                    Quantity = (int)numQuantity.Value,
                    UnitPrice = unitPrice
                });
            }

            txtProductName.Clear();
            numQuantity.Value = 1;
            txtUnitPrice.Clear();
            txtProductName.Focus();

            UpdateItemsGrid();
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvItems.SelectedRows[0].Index;
                var itemToRemove = _orderItems[selectedIndex];

                var result = MessageBox.Show($"Remove '{itemToRemove.ProductName}' from order?",
                    "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _orderItems.RemoveAt(selectedIndex);
                    UpdateItemsGrid();
                }
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a supplier.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSupplier.Focus();
                return;
            }

            if (_orderItems.Count == 0)
            {
                MessageBox.Show("Please add at least one item to the order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Create order for {_orderItems.Count} item(s) with total ₱{_currentTotalCost:N2}?\n\nSupplier: {cmbSupplier.Text}",
                "Confirm Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_orderItems.Count > 0)
            {
                var result = MessageBox.Show("You have unsaved items. Cancel anyway?",
                    "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                    return;
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public PurchaseOrder GetOrder()
        {
            _newOrder.Supplier = cmbSupplier.Text;
            _newOrder.SupplierId = _suppliers[cmbSupplier.SelectedIndex]?.Id ?? 0;
            _newOrder.Items = _orderItems.ToList();
            _newOrder.ItemsSummary = GenerateItemsSummary();
            _newOrder.TotalCost = _currentTotalCost;
            _newOrder.Status = "Pending";
            _newOrder.OrderDate = DateTime.Now;
            _newOrder.ExpectedDelivery = dtpDeliveryDate.Value;
            _newOrder.Notes = txtNotes.Text;
            return _newOrder;
        }

        private string GenerateItemsSummary()
        {
            if (_orderItems.Count == 0) return "";
            if (_orderItems.Count == 1)
            {
                var item = _orderItems[0];
                return $"{item.ProductName} ({item.Quantity} x ₱{item.UnitPrice:N2})";
            }
            return $"{_orderItems.Count} items - {_orderItems.Sum(i => i.Quantity)} units total";
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.lblSupplierContact = new System.Windows.Forms.Label();
            this.lblDeliveryDate = new System.Windows.Forms.Label();
            this.dtpDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.lblDeliveryWarning = new System.Windows.Forms.Label();
            this.lblOrderItems = new System.Windows.Forms.Label();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.lblAddItem = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.lblItemTotal = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.lblOrderSummary = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblGrandTotal = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelLine = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(226, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Create New Order";
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplier.Location = new System.Drawing.Point(20, 70);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(73, 17);
            this.lblSupplier.TabIndex = 1;
            this.lblSupplier.Text = "Supplier:";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(120, 67);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(300, 24);
            this.cmbSupplier.TabIndex = 2;
            // 
            // lblSupplierContact
            // 
            this.lblSupplierContact.AutoSize = true;
            this.lblSupplierContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblSupplierContact.ForeColor = System.Drawing.Color.Gray;
            this.lblSupplierContact.Location = new System.Drawing.Point(120, 95);
            this.lblSupplierContact.Name = "lblSupplierContact";
            this.lblSupplierContact.Size = new System.Drawing.Size(145, 15);
            this.lblSupplierContact.TabIndex = 3;
            this.lblSupplierContact.Text = "Contact: Select a supplier";
            // 
            // lblDeliveryDate
            // 
            this.lblDeliveryDate.AutoSize = true;
            this.lblDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeliveryDate.Location = new System.Drawing.Point(20, 130);
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            this.lblDeliveryDate.Size = new System.Drawing.Size(111, 17);
            this.lblDeliveryDate.TabIndex = 4;
            this.lblDeliveryDate.Text = "Delivery Date:";
            // 
            // dtpDeliveryDate
            // 
            this.dtpDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDeliveryDate.Location = new System.Drawing.Point(120, 127);
            this.dtpDeliveryDate.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dtpDeliveryDate.Name = "dtpDeliveryDate";
            this.dtpDeliveryDate.Size = new System.Drawing.Size(200, 23);
            this.dtpDeliveryDate.TabIndex = 5;
            this.dtpDeliveryDate.Value = new System.DateTime(2026, 5, 29, 1, 53, 35, 294);
            // 
            // lblDeliveryWarning
            // 
            this.lblDeliveryWarning.AutoSize = true;
            this.lblDeliveryWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblDeliveryWarning.Location = new System.Drawing.Point(120, 155);
            this.lblDeliveryWarning.Name = "lblDeliveryWarning";
            this.lblDeliveryWarning.Size = new System.Drawing.Size(135, 13);
            this.lblDeliveryWarning.TabIndex = 6;
            this.lblDeliveryWarning.Text = "✓ Delivery date looks good";
            // 
            // lblOrderItems
            // 
            this.lblOrderItems.AutoSize = true;
            this.lblOrderItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblOrderItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.lblOrderItems.Location = new System.Drawing.Point(20, 190);
            this.lblOrderItems.Name = "lblOrderItems";
            this.lblOrderItems.Size = new System.Drawing.Size(104, 20);
            this.lblOrderItems.TabIndex = 7;
            this.lblOrderItems.Text = "Order Items";
            // 
            // dgvItems
            // 
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItems.Location = new System.Drawing.Point(20, 215);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(550, 150);
            this.dgvItems.TabIndex = 8;
            // 
            // lblAddItem
            // 
            this.lblAddItem.AutoSize = true;
            this.lblAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblAddItem.Location = new System.Drawing.Point(590, 215);
            this.lblAddItem.Name = "lblAddItem";
            this.lblAddItem.Size = new System.Drawing.Size(71, 17);
            this.lblAddItem.TabIndex = 9;
            this.lblAddItem.Text = "Add Item";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(590, 245);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(78, 13);
            this.lblProductName.TabIndex = 10;
            this.lblProductName.Text = "Product Name:";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(590, 262);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 11;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(590, 295);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(49, 13);
            this.lblQuantity.TabIndex = 12;
            this.lblQuantity.Text = "Quantity:";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(590, 312);
            this.numQuantity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(80, 20);
            this.numQuantity.TabIndex = 13;
            this.numQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Location = new System.Drawing.Point(680, 295);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(56, 13);
            this.lblUnitPrice.TabIndex = 14;
            this.lblUnitPrice.Text = "Unit Price:";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(680, 312);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(100, 20);
            this.txtUnitPrice.TabIndex = 15;
            // 
            // lblItemTotal
            // 
            this.lblItemTotal.AutoSize = true;
            this.lblItemTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblItemTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.lblItemTotal.Location = new System.Drawing.Point(590, 345);
            this.lblItemTotal.Name = "lblItemTotal";
            this.lblItemTotal.Size = new System.Drawing.Size(116, 15);
            this.lblItemTotal.TabIndex = 16;
            this.lblItemTotal.Text = "Item Total: ₱0.00";
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.ForeColor = System.Drawing.Color.White;
            this.btnAddItem.Location = new System.Drawing.Point(680, 365);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(100, 30);
            this.btnAddItem.TabIndex = 17;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoveItem.Location = new System.Drawing.Point(470, 370);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(100, 30);
            this.btnRemoveItem.TabIndex = 18;
            this.btnRemoveItem.Text = "Remove Selected";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            // 
            // lblOrderSummary
            // 
            this.lblOrderSummary.AutoSize = true;
            this.lblOrderSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblOrderSummary.ForeColor = System.Drawing.Color.Gray;
            this.lblOrderSummary.Location = new System.Drawing.Point(20, 380);
            this.lblOrderSummary.Name = "lblOrderSummary";
            this.lblOrderSummary.Size = new System.Drawing.Size(97, 15);
            this.lblOrderSummary.TabIndex = 19;
            this.lblOrderSummary.Text = "0 item(s), 0 units";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(20, 420);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(96, 20);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total Cost:";
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.AutoSize = true;
            this.lblGrandTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.lblGrandTotal.Location = new System.Drawing.Point(120, 415);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(74, 26);
            this.lblGrandTotal.TabIndex = 21;
            this.lblGrandTotal.Text = "₱0.00";
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblNotes.Location = new System.Drawing.Point(20, 460);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(55, 17);
            this.lblNotes.TabIndex = 22;
            this.lblNotes.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(20, 480);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(550, 60);
            this.txtNotes.TabIndex = 23;
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(590, 500);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(120, 40);
            this.btnCreate.TabIndex = 24;
            this.btnCreate.Text = "Create Order";
            this.btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(720, 500);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // panelLine
            // 
            this.panelLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panelLine.Location = new System.Drawing.Point(20, 175);
            this.panelLine.Name = "panelLine";
            this.panelLine.Size = new System.Drawing.Size(800, 1);
            this.panelLine.TabIndex = 26;
            // 
            // CreateOrderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(850, 570);
            this.Controls.Add(this.panelLine);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.lblGrandTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblOrderSummary);
            this.Controls.Add(this.btnRemoveItem);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.lblItemTotal);
            this.Controls.Add(this.txtUnitPrice);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.lblAddItem);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.lblOrderItems);
            this.Controls.Add(this.lblDeliveryWarning);
            this.Controls.Add(this.dtpDeliveryDate);
            this.Controls.Add(this.lblDeliveryDate);
            this.Controls.Add(this.lblSupplierContact);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateOrderDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Order";
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblSupplierContact;
        private System.Windows.Forms.Label lblDeliveryDate;
        private System.Windows.Forms.DateTimePicker dtpDeliveryDate;
        private System.Windows.Forms.Label lblDeliveryWarning;
        private System.Windows.Forms.Label lblOrderItems;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Label lblAddItem;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label lblItemTotal;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Label lblOrderSummary;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelLine;
    }
}