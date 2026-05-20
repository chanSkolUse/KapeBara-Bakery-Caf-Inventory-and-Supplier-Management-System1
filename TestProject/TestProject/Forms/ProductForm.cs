using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TestProject.Dialogs;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Forms
{
    public partial class ProductForm : Form
    {
        private ProductService _productService;
        private InventoryService _inventoryService;
        private string _currentCategory = "All";
        private int _cardWidth = 200;
        private int _cardHeight = 280;
        private int _cardMargin = 23;
        private int _cardsPerRow = 4;

        public ProductForm()
        {
            InitializeComponent();
            _productService = new ProductService();
            _inventoryService = new InventoryService();
            SetupCategoryButtons();
            SetupPanelResize();
            LoadProducts();
            WireEvents();
        }

        private void WireEvents()
        {
            button1.Click += Button1_Click;
            textBox1.TextChanged += TextBox1_TextChanged;
            btnSearch.Click += BtnSearch_Click;
            this.Resize += ProductForm_Resize;
            panel2.Resize += Panel2_Resize;

            // Category filter buttons
            btnAll.Click += (s, e) => FilterByCategory("All");
            btnCoffee.Click += (s, e) => FilterByCategory("Coffee");
            btnSyrups.Click += (s, e) => FilterByCategory("Syrups");
            btnDairy.Click += (s, e) => FilterByCategory("Dairy");
            btnToppings.Click += (s, e) => FilterByCategory("Toppings");
            btnFlour.Click += (s, e) => FilterByCategory("Flour");
            btnBreads.Click += (s, e) => FilterByCategory("Breads");
            btnPastries.Click += (s, e) => FilterByCategory("Pastries");
            btnDrinks.Click += (s, e) => FilterByCategory("Drinks");
        }

        private void SetupPanelResize()
        {
            panel2.AutoScroll = true;
            panel2.AutoScrollMinSize = new Size(0, 0);
            //panel2.ResizeRedraw = true;
        }

        private void ProductForm_Resize(object sender, EventArgs e)
        {
            RecalculateCardsPerRow();
            FilterByCategory(_currentCategory);
        }

        private void Panel2_Resize(object sender, EventArgs e)
        {
            RecalculateCardsPerRow();
            FilterByCategory(_currentCategory);
        }

        private void RecalculateCardsPerRow()
        {
            int availableWidth = panel2.ClientSize.Width - 21; // Subtract padding
            _cardsPerRow = Math.Max(1, availableWidth / (_cardWidth + _cardMargin));

            // Calculate total width to center cards
            int totalCardsWidth = _cardsPerRow * (_cardWidth + _cardMargin) - _cardMargin;
            int startX = Math.Max(0, (availableWidth - totalCardsWidth) / 2);

            // Store start X for layout
            panel2.Padding = new Padding(startX, 10, 10, 0);
        }

        private void SetupCategoryButtons()
        {
            if (flowCategoryPanel == null)
            {
                flowCategoryPanel = new FlowLayoutPanel();
                flowCategoryPanel.Dock = DockStyle.Top;
                flowCategoryPanel.Height = 50;
                flowCategoryPanel.Padding = new Padding(10, 10, 0, 0);
                flowCategoryPanel.BackColor = Color.White;
                flowCategoryPanel.AutoScroll = true;
                panel1.Controls.Add(flowCategoryPanel);
                flowCategoryPanel.BringToFront();

                // Move existing controls down
                label2.Location = new Point(8, 60);
                label2.Visible = false;
                textBox1.Location = new Point(8, 60);
                btnSearch.Location = new Point(480, 58);
                panel2.Location = new Point(8, 100);
                panel2.Size = new Size(930, 416);
            }

            flowCategoryPanel.Controls.Clear();

            var categories = new[] { "All", "Coffee", "Syrups", "Dairy", "Toppings", "Flour", "Breads", "Pastries", "Drinks" };
            var categoryButtons = new[] { btnAll, btnCoffee, btnSyrups, btnDairy, btnToppings, btnFlour, btnBreads, btnPastries, btnDrinks };

            for (int i = 0; i < categories.Length; i++)
            {
                Button btn;
                if (i < categoryButtons.Length && categoryButtons[i] != null)
                {
                    btn = categoryButtons[i];
                }
                else
                {
                    btn = new Button();
                    btn.Name = $"btn{categories[i]}";
                    btn.Click += (s, e) => FilterByCategory(categories[i]);
                }

                btn.Text = categories[i];
                btn.Size = new Size(90, 35);
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(120, 71, 70);
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

                flowCategoryPanel.Controls.Add(btn);
            }
        }

        private void LoadProducts()
        {
            RecalculateCardsPerRow();
            var products = _productService.GetAll();
            DisplayProducts(products);
            UpdateCategoryCounts();
        }

        private void UpdateCategoryCounts()
        {
            var products = _productService.GetAll();
            var categoryCounts = products.GroupBy(p => p.CategoryName).ToDictionary(g => g.Key, g => g.Count());

            foreach (var btn in flowCategoryPanel.Controls.OfType<Button>())
            {
                string category = btn.Text.Split('(')[0].Trim();
                if (category != "All" && categoryCounts.ContainsKey(category))
                {
                    btn.Text = $"{category} ({categoryCounts[category]})";
                }
                else if (category != "All")
                {
                    btn.Text = $"{category} (0)";
                }
                else
                {
                    btn.Text = $"All ({products.Count})";
                }
            }
        }

        private void FilterByCategory(string category)
        {
            _currentCategory = category;

            // Update button styles
            foreach (var btn in flowCategoryPanel.Controls.OfType<Button>())
            {
                string btnCategory = btn.Text.Split('(')[0].Trim();
                if (btnCategory == category)
                {
                    btn.BackColor = Color.FromArgb(120, 71, 70);
                    btn.ForeColor = Color.White;
                }
                else
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = Color.FromArgb(120, 71, 70);
                }
            }

            var products = _productService.GetAll();
            if (category != "All")
            {
                products = products.Where(p => p.CategoryName == category).ToList();
            }

            // Apply search filter
            string searchText = textBox1.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText) && searchText != "search products...")
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchText) ||
                                                p.SKU.ToLower().Contains(searchText)).ToList();
            }

            DisplayProducts(products);
        }

        private void DisplayProducts(System.Collections.Generic.List<Product> products)
        {
            panel2.Controls.Clear();

            if (products.Count == 0)
            {
                Label lblNoResults = new Label
                {
                    Text = "No products found",
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(panel2.Width - 20, 100),
                    Location = new Point(10, (panel2.Height - 100) / 2)
                };
                panel2.Controls.Add(lblNoResults);
                return;
            }

            int row = 0;
            int col = 0;

            foreach (var product in products)
            {
                int x = col * (_cardWidth + _cardMargin);
                int y = row * (_cardHeight + _cardMargin);

                var productCard = CreateEnhancedProductCard(product);
                productCard.Location = new Point(x, y);
                panel2.Controls.Add(productCard);

                col++;
                if (col >= _cardsPerRow)
                {
                    col = 0;
                    row++;
                }
            }

            // Set panel scroll area
            int totalHeight = (row + 1) * (_cardHeight + _cardMargin) + 20;
            panel2.AutoScrollMinSize = new Size(0, totalHeight);
        }

        private Panel CreateEnhancedProductCard(Product product)
        {
            var inventory = _inventoryService.GetByProductId(product.Id);
            int stock = inventory?.QuantityOnHand ?? 0;
            int reorderLevel = inventory?.ReorderLevel ?? 10;
            bool isLowStock = stock <= reorderLevel;
            string statusText = isLowStock ? "LOW STOCK" : "IN STOCK";
            Color statusColor = isLowStock ? Color.OrangeRed : Color.Green;
            Color statusBgColor = isLowStock ? Color.FromArgb(255, 245, 245) : Color.FromArgb(240, 255, 240);

            var panel = new Panel
            {
                Width = _cardWidth,
                Height = _cardHeight,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = product.Id
            };

            // Image Panel
            Panel imagePanel = new Panel
            {
                Width = _cardWidth - 2,
                Height = 140,
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            PictureBox pbImage = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Load product image
            if (product.ProductImage != null && product.ProductImage.Length > 0)
            {
                using (var ms = new MemoryStream(product.ProductImage))
                {
                    pbImage.Image = Image.FromStream(ms);
                }
            }
            else if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
            {
                pbImage.Image = Image.FromFile(product.ImagePath);
            }
            else
            {
                // Default image based on category with colored background
                pbImage.BackColor = GetCategoryColor(product.CategoryName);
                pbImage.Image = GetDefaultCategoryImage(product.CategoryName);
                pbImage.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            imagePanel.Controls.Add(pbImage);

            // Status Badge
            Panel statusBadge = new Panel
            {
                Width = 80,
                Height = 22,
                BackColor = statusBgColor,
                Location = new Point(_cardWidth - 90, 8),
                BorderStyle = BorderStyle.None
            };

            Label lblStatus = new Label
            {
                Text = statusText,
                ForeColor = statusColor,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(8, 4),
                AutoSize = true
            };
            statusBadge.Controls.Add(lblStatus);
            imagePanel.Controls.Add(statusBadge);
            statusBadge.BringToFront();

            // Product Name
            Label lblName = new Label
            {
                Text = product.Name.Length > 25 ? product.Name.Substring(0, 22) + "..." : product.Name,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 148),
                Size = new Size(_cardWidth - 20, 45),
                ForeColor = Color.FromArgb(120, 71, 70)
            };

            // Price
            Label lblPrice = new Label
            {
                Text = $"₱ {product.UnitPrice:F2}",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Location = new Point(10, 195),
                Size = new Size(120, 25),
                ForeColor = Color.FromArgb(120, 71, 70)
            };

            // Stock
            Label lblStock = new Label
            {
                Text = $"Stock: {stock} units",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 222),
                Size = new Size(120, 20),
                ForeColor = Color.Gray
            };

            // Action Buttons Panel
            Panel actionPanel = new Panel
            {
                Width = _cardWidth - 20,
                Height = 32,
                Location = new Point(10, 245)
            };

            Button btnEdit = new Button
            {
                Text = "Edit",
                Location = new Point(0, 0),
                Size = new Size((_cardWidth - 30) / 2, 28),
                BackColor = Color.FromArgb(120, 71, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = product,
                Cursor = Cursors.Hand
            };
            btnEdit.Click += (s, e) => EditProduct(product);

            Button btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point((_cardWidth - 30) / 2 + 5, 0),
                Size = new Size((_cardWidth - 30) / 2, 28),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = product,
                Cursor = Cursors.Hand
            };
            btnDelete.Click += (s, e) => DeleteProduct(product);

            actionPanel.Controls.Add(btnEdit);
            actionPanel.Controls.Add(btnDelete);

            panel.Controls.Add(imagePanel);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(lblStock);
            panel.Controls.Add(actionPanel);

            // Add hover effect
            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(255, 252, 250);
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        private Color GetCategoryColor(string category)
        {
            switch (category)
            {
                case "Coffee": return Color.FromArgb(210, 180, 140);
                case "Syrups": return Color.FromArgb(255, 228, 196);
                case "Dairy": return Color.FromArgb(240, 248, 255);
                case "Toppings": return Color.FromArgb(255, 240, 245);
                case "Flour": return Color.FromArgb(245, 245, 220);
                case "Breads": return Color.FromArgb(222, 184, 135);
                case "Pastries": return Color.FromArgb(255, 218, 185);
                case "Drinks": return Color.FromArgb(224, 255, 255);
                default: return Color.FromArgb(250, 250, 250);
            }
        }

        private Image GetDefaultCategoryImage(string category)
        {
            // Create a simple icon based on category
            Bitmap bmp = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(GetCategoryColor(category));
                using (Font font = new Font("Segoe UI Emoji", 24))
                {
                    string icon;

                    // Traditional switch statement (C# 7.3 compatible)
                    switch (category)
                    {
                        case "Coffee":
                            icon = "☕";
                            break;
                        case "Syrups":
                            icon = "🍯";
                            break;
                        case "Dairy":
                            icon = "🥛";
                            break;
                        case "Toppings":
                            icon = "✨";
                            break;
                        case "Flour":
                            icon = "🌾";
                            break;
                        case "Breads":
                            icon = "🍞";
                            break;
                        case "Pastries":
                            icon = "🥐";
                            break;
                        case "Drinks":
                            icon = "🥤";
                            break;
                        default:
                            icon = "📦";
                            break;
                    }

                    var size = g.MeasureString(icon, font);
                    g.DrawString(icon, font, Brushes.Gray, (64 - size.Width) / 2, (64 - size.Height) / 2);
                }
            }
            return bmp;
        }

        private void EditProduct(Product product)
        {
            var dialog = new ProductDialog(product);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var updatedProduct = dialog.GetProduct();
                updatedProduct.Id = product.Id;
                _productService.Update(updatedProduct);
                LoadProducts();
                MessageBox.Show("Product updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteProduct(Product product)
        {
            var result = MessageBox.Show($"Are you sure you want to delete '{product.Name}'?\nThis action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _productService.Delete(product.Id);
                LoadProducts();
                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var dialog = new ProductDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newProduct = dialog.GetProduct();
                _productService.Add(newProduct);
                _inventoryService.AddOrUpdate(newProduct.Id, dialog.GetStockQuantity(), dialog.GetReorderLevel());
                LoadProducts();
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            FilterByCategory(_currentCategory);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterByCategory(_currentCategory);
        }
    }
}