// Forms/InventoryForm.cs (Fixed)
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
                // IMPORTANT: Set the Tag to store the Product ID
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
            var lowStockItems = _inventoryService.GetLowStockItems();
            string alertText = "";
            foreach (var item in lowStockItems)
            {
                var product = _productService.GetById(item.ProductId);
                if (product != null)
                {
                    alertText += $"• {product.Name}: {item.QuantityOnHand} units (Reorder at {item.ReorderLevel})\n";
                }
            }

            if (string.IsNullOrEmpty(alertText))
                alertText = "All items are at healthy stock levels.";

            label9.Text = alertText;
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

            int productId = (int)item.Value;
            var existing = _inventoryService.GetByProductId(productId);
            int reorderLevel = existing?.ReorderLevel ?? 10;

            _inventoryService.AddOrUpdate(productId, quantity, reorderLevel);

            var product = _productService.GetById(productId);
            product.UnitPrice = unitPrice;
            _productService.Update(product);

            LoadProductsToGrid();
            LoadLowStockAlerts();
            MessageBox.Show("Inventory updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Name == "action")
            {
                // Add null check and validation
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

        }
    }
}