using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestProject.Models;
using TestProject.Services;
using TestProject.Dialogs;

namespace TestProject.ui
{
    public partial class inventoryForm : Form
    {
        private ProductService _productService;
        private InventoryService _inventoryService;
        private Timer _refreshTimer;

        public inventoryForm()
        {
            InitializeComponent();
            _productService = new ProductService();
            _inventoryService = new InventoryService();
            InitializeRuntime();
        }

        private void InitializeRuntime()
        {
            // Setup DataGridView button column
            if (dataGridView1.Columns.Contains("action"))
            {
                var idx = dataGridView1.Columns["action"].Index;
                dataGridView1.Columns.RemoveAt(idx);
                var btnCol = new DataGridViewButtonColumn();
                btnCol.Name = "action";
                btnCol.HeaderText = "Actions";
                btnCol.Text = "Manage";
                btnCol.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnCol);
            }

            dataGridView1.CellClick += DataGridView1_CellClick;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            button7.Click += QuickAdjustUpdate_Click;
            addProductBtn.Click += AddProductBtn_Click;

            LoadProductsToGrid();
            RefreshQuickAdjustList();
            LoadLowStockAlerts();

            // Setup search functionality
            textBox1.TextChanged += TextBox1_TextChanged;

            // Setup category filter buttons
            button1.Click += (s, e) => FilterByCategory(null);
            button2.Click += (s, e) => FilterByCategory("Flour");
            button3.Click += (s, e) => FilterByCategory("Dairy");
            button4.Click += (s, e) => FilterByCategory("Toppings");
            button6.Click += (s, e) => FilterByCategory("Syrups");
            button5.Click += (s, e) => FilterByCategory("Coffee");

            // Subscribe to panel8 resize event
            if (panel8 != null)
            {
                panel8.Resize += Panel8_Resize;
            }

            // Auto-refresh low stock alerts every 30 seconds
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000;
            _refreshTimer.Tick += (s, e) => LoadLowStockAlerts();
            _refreshTimer.Start();
        }

        private void LoadProductsToGrid()
        {
            dataGridView1.Rows.Clear();
            var products = _productService.GetAll();

            foreach (var product in products)
            {
                var inventory = _inventoryService.GetByProductId(product.Id);
                int stock = inventory?.QuantityOnHand ?? 0;
                int reorderLevel = inventory?.ReorderLevel ?? 10;
                string status = stock <= reorderLevel ? "⚠ LOW" : "✓ OK";

                int rowIndex = dataGridView1.Rows.Add(
                    product.Name,
                    product.CategoryName,
                    stock.ToString(),
                    $"₱{product.UnitPrice:F2}",
                    product.SKU,
                    status,
                    "Manage"
                );
                dataGridView1.Rows[rowIndex].Tag = product.Id;
            }
        }

        private void FilterByCategory(string category)
        {
            dataGridView1.Rows.Clear();
            var products = _productService.GetAll();

            var filteredProducts = string.IsNullOrEmpty(category)
                ? products
                : products.Where(p => p.CategoryName == category).ToList();

            foreach (var product in filteredProducts)
            {
                var inventory = _inventoryService.GetByProductId(product.Id);
                int stock = inventory?.QuantityOnHand ?? 0;
                int reorderLevel = inventory?.ReorderLevel ?? 10;
                string status = stock <= reorderLevel ? "⚠ LOW" : "✓ OK";

                int rowIndex = dataGridView1.Rows.Add(
                    product.Name,
                    product.CategoryName,
                    stock.ToString(),
                    $"₱{product.UnitPrice:F2}",
                    product.SKU,
                    status,
                    "Manage"
                );
                dataGridView1.Rows[rowIndex].Tag = product.Id;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.ToLower();
            dataGridView1.Rows.Clear();

            var products = _productService.GetAll();
            var filteredProducts = products.Where(p =>
                string.IsNullOrEmpty(searchText) ||
                p.Name.ToLower().Contains(searchText) ||
                p.SKU.ToLower().Contains(searchText)
            ).ToList();

            foreach (var product in filteredProducts)
            {
                var inventory = _inventoryService.GetByProductId(product.Id);
                int stock = inventory?.QuantityOnHand ?? 0;
                int reorderLevel = inventory?.ReorderLevel ?? 10;
                string status = stock <= reorderLevel ? "⚠ LOW" : "✓ OK";

                int rowIndex = dataGridView1.Rows.Add(
                    product.Name,
                    product.CategoryName,
                    stock.ToString(),
                    $"₱{product.UnitPrice:F2}",
                    product.SKU,
                    status,
                    "Manage"
                );
                dataGridView1.Rows[rowIndex].Tag = product.Id;
            }
        }

        private void RefreshQuickAdjustList()
        {
            comboBox1.Items.Clear();
            foreach (var product in _productService.GetAll())
            {
                comboBox1.Items.Add(new ComboboxItem { Text = product.Name, Value = product.Id });
            }
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
        }

        private void LoadLowStockAlerts()
        {
            // Clear existing controls in panel8
            if (panel8 != null)
            {
                panel8.Controls.Clear();
            }

            var lowStockItems = _inventoryService.GetLowStockItems();

            if (lowStockItems.Count == 0)
            {
                Label lblNoAlerts = new Label()
                {
                    Text = "  ✓ All items are at healthy stock levels",
                    Font = new Font("Microsoft Sans Serif", 10F),
                    ForeColor = Color.Green,
                    AutoSize = true,
                    Padding = new Padding(10, 20, 10, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panel8.Controls.Add(lblNoAlerts);
                return;
            }

            // Center the panels in panel8
            int panelWidth = 380;
            int startX = (panel8.Width - panelWidth) / 2;
            if (startX < 5) startX = 5;

            int yPosition = 5;
            foreach (var item in lowStockItems)
            {
                var product = _productService.GetById(item.ProductId);
                if (product != null)
                {
                    Panel alertPanel = CreateLowStockAlertPanel(product, item);
                    alertPanel.Location = new Point(startX, yPosition);
                    panel8.Controls.Add(alertPanel);
                    yPosition += alertPanel.Height + 8;
                }
            }
        }

        private Panel CreateLowStockAlertPanel(Product product, InventoryItem inventory)
        {
            Panel panel = new Panel()
            {
                Width = 380,  // Fixed width, not too wide
                Height = 60,
                BackColor = Color.FromArgb(255, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 5)
            };

            // Product Name
            Label lblName = new Label()
            {
                Text = product.Name,
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                Location = new Point(8, 6),
                Size = new Size(200, 18),
                ForeColor = Color.FromArgb(120, 71, 70)
            };

            // Stock remaining
            Label lblStock = new Label()
            {
                Text = $"Remaining: {inventory.QuantityOnHand} units",
                Font = new Font("Microsoft Sans Serif", 8F),
                Location = new Point(8, 26),
                Size = new Size(120, 14),
                ForeColor = Color.Gray
            };

            // Reorder level
            Label lblReorder = new Label()
            {
                Text = $"Reorder at: {inventory.ReorderLevel} units",
                Font = new Font("Microsoft Sans Serif", 8F),
                Location = new Point(8, 41),
                Size = new Size(120, 14),
                ForeColor = Color.Gray
            };

            // Status badge
            Panel statusBadge = new Panel()
            {
                Width = 60,
                Height = 24,
                BackColor = Color.OrangeRed,
                Location = new Point(panel.Width - 70, 18),
                BorderStyle = BorderStyle.None
            };

            Label lblStatus = new Label()
            {
                Text = "LOW",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(12, 4),
                AutoSize = true
            };
            statusBadge.Controls.Add(lblStatus);

            // Urgency indicator based on stock level
            int urgency = inventory.ReorderLevel - inventory.QuantityOnHand;
            if (urgency >= inventory.ReorderLevel / 2)
            {
                panel.BackColor = Color.FromArgb(255, 235, 235);
                statusBadge.BackColor = Color.Red;
            }

            // Add click event to filter products by this item
            panel.Click += (s, e) => FilterByProduct(product.Name);
            lblName.Click += (s, e) => FilterByProduct(product.Name);
            lblStock.Click += (s, e) => FilterByProduct(product.Name);
            lblReorder.Click += (s, e) => FilterByProduct(product.Name);
            statusBadge.Click += (s, e) => FilterByProduct(product.Name);

            // Change cursor to hand on clickable elements
            panel.Cursor = Cursors.Hand;
            lblName.Cursor = Cursors.Hand;
            lblStock.Cursor = Cursors.Hand;
            lblReorder.Cursor = Cursors.Hand;
            statusBadge.Cursor = Cursors.Hand;

            // Hover effect
            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(255, 225, 225);
            panel.MouseLeave += (s, e) =>
            {
                panel.BackColor = urgency >= inventory.ReorderLevel / 2 ? Color.FromArgb(255, 235, 235) : Color.FromArgb(255, 245, 245);
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblStock);
            panel.Controls.Add(lblReorder);
            panel.Controls.Add(statusBadge);

            return panel;
        }

        private void Panel8_Resize(object sender, EventArgs e)
        {
            // Refresh alerts when panel resizes to adjust widths
            LoadLowStockAlerts();
        }

        private void FilterByProduct(string productName)
        {
            // Search for the product in the grid
            textBox1.Text = productName;
            TextBox1_TextChanged(null, null);

            // Highlight the row if found
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["name"].Value != null && row.Cells["name"].Value.ToString() == productName)
                {
                    row.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = comboBox1.SelectedItem as ComboboxItem;
            if (item == null) return;

            var product = _productService.GetById((int)item.Value);
            var inventory = _inventoryService.GetByProductId(product.Id);

            textBox3.Text = inventory?.QuantityOnHand.ToString() ?? "0";
            textBox4.Text = product?.UnitPrice.ToString("0.00") ?? "0";
        }

        private void QuickAdjustUpdate_Click(object sender, EventArgs e)
        {
            var item = comboBox1.SelectedItem as ComboboxItem;
            if (item == null) return;

            if (!int.TryParse(textBox3.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid stock quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBox4.Text, out decimal unitPrice))
            {
                MessageBox.Show("Please enter a valid unit price.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (quantity < 0)
            {
                MessageBox.Show("Stock quantity cannot be negative.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = (int)item.Value;
            var existing = _inventoryService.GetByProductId(productId);
            int reorderLevel = existing?.ReorderLevel ?? 10;

            _inventoryService.AddOrUpdate(productId, quantity, reorderLevel);

            var product = _productService.GetById(productId);
            product.UnitPrice = unitPrice;
            _productService.Update(product);

            LoadProductsToGrid();
            LoadLowStockAlerts();
            RefreshQuickAdjustList();

            MessageBox.Show("Inventory updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Name == "action")
            {
                if (dataGridView1.Rows[e.RowIndex].Tag == null)
                {
                    MessageBox.Show("Error: Product ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int productId;
                try
                {
                    productId = (int)dataGridView1.Rows[e.RowIndex].Tag;
                }
                catch (InvalidCastException)
                {
                    MessageBox.Show("Error: Invalid product ID format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var product = _productService.GetById(productId);
                var inventory = _inventoryService.GetByProductId(productId);

                if (product == null)
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ShowProductManagementDialog(product, inventory);
            }
        }

        private void ShowProductManagementDialog(Product product, InventoryItem inventory)
        {
            Form dialog = new Form();
            dialog.Text = $"Manage Product: {product.Name}";
            dialog.Size = new Size(400, 420);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.BackColor = Color.White;

            int currentStock = inventory?.QuantityOnHand ?? 0;
            int reorderLevel = inventory?.ReorderLevel ?? 10;

            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 200;
            int leftMargin = 30;
            int controlLeft = leftMargin + labelWidth;

            // Product Name (readonly)
            Label lblName = new Label() { Text = "Product Name:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            TextBox txtName = new TextBox() { Text = product.Name, Location = new Point(controlLeft, yPos), Size = new Size(controlWidth, 25), ReadOnly = true };
            yPos += 35;

            // Category (readonly)
            Label lblCategory = new Label() { Text = "Category:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            TextBox txtCategory = new TextBox() { Text = product.CategoryName, Location = new Point(controlLeft, yPos), Size = new Size(controlWidth, 25), ReadOnly = true };
            yPos += 35;

            // SKU (readonly)
            Label lblSKU = new Label() { Text = "SKU:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            TextBox txtSKU = new TextBox() { Text = product.SKU, Location = new Point(controlLeft, yPos), Size = new Size(controlWidth, 25), ReadOnly = true };
            yPos += 35;

            // Current Stock
            Label lblStock = new Label() { Text = "Current Stock:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            NumericUpDown numStock = new NumericUpDown()
            {
                Location = new Point(controlLeft, yPos),
                Size = new Size(controlWidth, 25),
                Minimum = 0,
                Maximum = 99999,
                Value = currentStock
            };
            yPos += 35;

            // Reorder Level
            Label lblReorder = new Label() { Text = "Reorder Level:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            NumericUpDown numReorder = new NumericUpDown()
            {
                Location = new Point(controlLeft, yPos),
                Size = new Size(controlWidth, 25),
                Minimum = 0,
                Maximum = 9999,
                Value = reorderLevel
            };
            yPos += 35;

            // Unit Price
            Label lblPrice = new Label() { Text = "Unit Price:", Location = new Point(leftMargin, yPos), Size = new Size(labelWidth, 25) };
            NumericUpDown numPrice = new NumericUpDown()
            {
                Location = new Point(controlLeft, yPos),
                Size = new Size(controlWidth, 25),
                Minimum = 0,
                Maximum = 99999,
                DecimalPlaces = 2,
                Value = product.UnitPrice
            };
            yPos += 45;

            // Buttons
            Button btnUpdate = new Button()
            {
                Text = "Update",
                Location = new Point(controlLeft - 60, yPos),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(120, 71, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            Button btnDelete = new Button()
            {
                Text = "Delete Product",
                Location = new Point(controlLeft + 50, yPos),
                Size = new Size(100, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnUpdate.Click += (s, ev) =>
            {
                int newStock = (int)numStock.Value;
                int newReorder = (int)numReorder.Value;
                decimal newPrice = numPrice.Value;

                _inventoryService.AddOrUpdate(product.Id, newStock, newReorder);
                product.UnitPrice = newPrice;
                _productService.Update(product);

                LoadProductsToGrid();
                LoadLowStockAlerts();
                RefreshQuickAdjustList();

                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dialog.Close();
            };

            btnDelete.Click += (s, ev) =>
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{product.Name}'?\nThis action cannot be undone.",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _productService.Delete(product.Id);
                    LoadProductsToGrid();
                    RefreshQuickAdjustList();
                    LoadLowStockAlerts();
                    MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dialog.Close();
                }
            };

            dialog.Controls.Add(lblName);
            dialog.Controls.Add(txtName);
            dialog.Controls.Add(lblCategory);
            dialog.Controls.Add(txtCategory);
            dialog.Controls.Add(lblSKU);
            dialog.Controls.Add(txtSKU);
            dialog.Controls.Add(lblStock);
            dialog.Controls.Add(numStock);
            dialog.Controls.Add(lblReorder);
            dialog.Controls.Add(numReorder);
            dialog.Controls.Add(lblPrice);
            dialog.Controls.Add(numPrice);
            dialog.Controls.Add(btnUpdate);
            dialog.Controls.Add(btnDelete);

            dialog.ShowDialog();
        }

        private void AddProductBtn_Click(object sender, EventArgs e)
        {
            var dialog = new ProductDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newProduct = dialog.GetProduct();
                _productService.Add(newProduct);

                int stock = dialog.GetStockQuantity();
                int reorder = dialog.GetReorderLevel();
                _inventoryService.AddOrUpdate(newProduct.Id, stock, reorder);

                LoadProductsToGrid();
                RefreshQuickAdjustList();
                LoadLowStockAlerts();
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addProductBtn_Click_1(object sender, EventArgs e)
        {
            AddProductBtn_Click(sender, e);
        }
    }
}