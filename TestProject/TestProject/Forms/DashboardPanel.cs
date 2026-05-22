using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TestProject.Services;
using TestProject.Forms;
using TestProject.ui;

namespace TestProject.ui
{
    public partial class dashboardPanel : Form
    {
        private ProductService _productService;
        private InventoryService _inventoryService;
        private OrderService _orderService;
        private Timer _refreshTimer;

        public dashboardPanel()
        {
            InitializeComponent();
            _productService = new ProductService();
            _inventoryService = new InventoryService();
            _orderService = new OrderService();
            LoadDashboardData();
            DrawInventoryStatusChart();
            DrawStockMovementChart();

            // Auto-refresh every 30 seconds
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 30000;
            _refreshTimer.Tick += (s, e) => RefreshCharts();
            _refreshTimer.Start();
        }

        private void RefreshCharts()
        {
            LoadDashboardData();
            DrawInventoryStatusChart();
            DrawStockMovementChart();
        }

        private void LoadDashboardData()
        {
            var totalItems = _productService.GetAll().Count;
            button1.Text = $"Total Items\n{totalItems}";

            var lowStockItems = _inventoryService.GetLowStockItems();
            button2.Text = $"Low Stock Alerts\n{lowStockItems.Count} items";

            var pendingOrders = _orderService.GetPendingOrders();
            button4.Text = $"To be Delivered\n{pendingOrders.Count} orders";

            var needReorder = lowStockItems.Count;
            button3.Text = $"To be Ordered\n{needReorder} items";

            var totalValue = _inventoryService.GetTotalInventoryValue(_productService);

            var monthlyOrders = _orderService.GetAllOrders().FindAll(o => o.OrderDate.Month == DateTime.Now.Month);
            decimal monthlySpend = 0;
            foreach (var order in monthlyOrders)
            {
                monthlySpend += order.TotalCost;
            }

            var products = _productService.GetAll();
            int inStock = products.Count - lowStockItems.Count;
        }

        private void DrawInventoryStatusChart()
        {
            panel4.Controls.Clear();

            var allProducts = _productService.GetAll();
            var lowStockItems = _inventoryService.GetLowStockItems();

            int totalItems = allProducts.Count;
            int lowStockCount = lowStockItems.Count;
            int healthyCount = totalItems - lowStockCount;

            double healthyPercent = totalItems > 0 ? (healthyCount * 100.0 / totalItems) : 0;
            double lowStockPercent = totalItems > 0 ? (lowStockCount * 100.0 / totalItems) : 0;

            // Create Pie Chart as a clickable PictureBox
            PictureBox pieChart = new PictureBox()
            {
                Size = new Size(180, 180),
                Location = new Point(60, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                Tag = "piechart"
            };

            Bitmap bmp = new Bitmap(180, 180);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.FromArgb(143, 126, 124));

                int centerX = 90;
                int centerY = 90;
                int radius = 75;
                Rectangle rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);

                if (totalItems > 0)
                {
                    float healthyAngle = 360f * healthyCount / totalItems;
                    float lowStockAngle = 360f * lowStockCount / totalItems;

                    // Draw Healthy Stock slice (Green)
                    if (healthyCount > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                        {
                            g.FillPie(brush, rect, 0, healthyAngle);
                        }
                    }

                    // Draw Low Stock slice (Red)
                    if (lowStockCount > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 69, 58)))
                        {
                            g.FillPie(brush, rect, healthyAngle, lowStockAngle);
                        }
                    }

                    // Draw border
                    using (Pen pen = new Pen(Color.White, 2))
                    {
                        g.DrawEllipse(pen, rect);
                    }

                    // Draw center text
                    using (Font centerFont = new Font("Segoe UI", 12, FontStyle.Bold))
                    using (Brush centerBrush = new SolidBrush(Color.White))
                    {
                        string centerText = $"{totalItems}";
                        SizeF textSize = g.MeasureString(centerText, centerFont);
                        g.DrawString(centerText, centerFont, centerBrush,
                            centerX - textSize.Width / 2, centerY - textSize.Height / 2);
                    }
                }
                else
                {
                    using (Brush brush = new SolidBrush(Color.Gray))
                    using (Font font = new Font("Segoe UI", 9))
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        g.FillEllipse(brush, rect);
                        g.DrawString("No Data", font, textBrush, centerX - 25, centerY - 8);
                    }
                }
            }

            pieChart.Image = bmp;
            pieChart.Click += (s, e) => ShowInventoryDetails();
            panel4.Controls.Add(pieChart);

            // Clickable Healthy Stock Legend
            Panel healthyLegend = CreateClickableLegend("Healthy Stock", healthyCount, healthyPercent, Color.FromArgb(76, 175, 80));
            healthyLegend.Location = new Point(30, 230);
            healthyLegend.Click += (s, e) => ShowHealthyStockDetails();
            panel4.Controls.Add(healthyLegend);

            // Clickable Low Stock Legend
            Panel lowLegend = CreateClickableLegend("Low Stock", lowStockCount, lowStockPercent, Color.FromArgb(255, 69, 58));
            lowLegend.Location = new Point(160, 230);
            lowLegend.Click += (s, e) => ShowLowStockDetails();
            panel4.Controls.Add(lowLegend);

            // Title
            Label title = new Label()
            {
                Text = "INVENTORY STATUS",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(85, 10),
                AutoSize = true
            };
            panel4.Controls.Add(title);
        }

        private Panel CreateClickableLegend(string label, int count, double percent, Color color)
        {
            Panel panel = new Panel()
            {
                Size = new Size(120, 45),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            Panel colorBox = new Panel()
            {
                Size = new Size(12, 12),
                Location = new Point(5, 5),
                BackColor = color,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label textLabel = new Label()
            {
                Text = $"{label}: {count}\n({percent:F1}%)",
                Font = new Font("Segoe UI", 7),
                ForeColor = Color.White,
                Location = new Point(22, 2),
                Size = new Size(95, 40)
            };

            panel.Controls.Add(colorBox);
            panel.Controls.Add(textLabel);

            return panel;
        }

        private void DrawStockMovementChart()
        {
            panel3.Controls.Clear();

            var allOrders = _orderService.GetAllOrders();

            // Get last 6 months data
            DateTime today = DateTime.Today;
            string[] monthNames = new string[6];
            decimal[] monthlyPurchases = new decimal[6];
            int[] monthlyOrdersCount = new int[6];
            DateTime[] monthDates = new DateTime[6];

            for (int i = 0; i < 6; i++)
            {
                DateTime targetDate = today.AddMonths(-(5 - i));
                monthDates[i] = targetDate;
                monthNames[i] = targetDate.ToString("MMM yyyy");

                monthlyPurchases[i] = allOrders
                    .Where(o => o.OrderDate.Year == targetDate.Year && o.OrderDate.Month == targetDate.Month)
                    .Sum(o => o.TotalCost);

                monthlyOrdersCount[i] = allOrders
                    .Count(o => o.OrderDate.Year == targetDate.Year && o.OrderDate.Month == targetDate.Month);
            }

            // Create clickable Bar Chart
            PictureBox barChart = new PictureBox()
            {
                Size = new Size(panel3.Width - 20, panel3.Height - 40),
                Location = new Point(10, 30),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand
            };

            Bitmap bmp = new Bitmap(panel3.Width - 20, panel3.Height - 40);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Clear(Color.FromArgb(143, 126, 124));

                int chartWidth = bmp.Width - 80;
                int chartHeight = bmp.Height - 60;
                int startX = 45;
                int startY = 20;
                int barWidth = (chartWidth - 30) / 6 - 5;
                int barSpacing = 8;
                int maxBarHeight = chartHeight - 30;

                decimal maxPurchase = monthlyPurchases.Max();
                if (maxPurchase == 0) maxPurchase = 1;

                // Draw axes
                using (Pen axisPen = new Pen(Color.White, 2))
                {
                    g.DrawLine(axisPen, startX - 5, startY, startX - 5, startY + chartHeight);
                    g.DrawLine(axisPen, startX - 5, startY + chartHeight, startX + chartWidth + 10, startY + chartHeight);
                }

                // Draw grid lines
                using (Pen gridPen = new Pen(Color.FromArgb(80, 255, 255, 255), 1))
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        int y = startY + (chartHeight * i / 4);
                        g.DrawLine(gridPen, startX - 5, y, startX + chartWidth + 10, y);
                    }
                }

                // Draw Y-axis labels
                using (Font axisFont = new Font("Segoe UI", 7))
                using (Brush axisBrush = new SolidBrush(Color.White))
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        int y = startY + chartHeight - (chartHeight * i / 4);
                        decimal value = maxPurchase * i / 4;
                        string label = value >= 1000 ? $"₱{value / 1000:N0}K" : $"₱{value:N0}";
                        g.DrawString(label, axisFont, axisBrush, 5, y - 6);
                    }
                }

                // Store bar coordinates for click detection
                var barAreas = new System.Collections.Generic.List<Rectangle>();

                // Draw bars
                for (int i = 0; i < 6; i++)
                {
                    int x = startX + i * (barWidth + barSpacing);
                    int barHeight = (int)((double)monthlyPurchases[i] / (double)maxPurchase * maxBarHeight);
                    if (barHeight < 3 && monthlyPurchases[i] > 0) barHeight = 3;

                    Color barColor;
                    if (monthlyPurchases[i] > maxPurchase * 0.7m)
                        barColor = Color.FromArgb(255, 69, 58);
                    else if (monthlyPurchases[i] > maxPurchase * 0.3m)
                        barColor = Color.FromArgb(255, 193, 7);
                    else
                        barColor = Color.FromArgb(76, 175, 80);

                    Rectangle barRect = new Rectangle(x, startY + chartHeight - barHeight, barWidth, barHeight);
                    barAreas.Add(barRect);

                    using (Brush barBrush = new SolidBrush(barColor))
                    {
                        g.FillRectangle(barBrush, barRect);
                    }

                    using (Pen borderPen = new Pen(Color.White, 1))
                    {
                        g.DrawRectangle(borderPen, barRect);
                    }

                    // Draw month label
                    using (Font labelFont = new Font("Segoe UI", 7, FontStyle.Bold))
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        string shortMonth = monthDates[i].ToString("MMM");
                        g.DrawString(shortMonth, labelFont, textBrush, x + barWidth / 4, startY + chartHeight + 5);
                    }

                    // Draw value on top of bar
                    if (monthlyPurchases[i] > 0 && barHeight > 15)
                    {
                        using (Font valueFont = new Font("Segoe UI", 6, FontStyle.Bold))
                        using (Brush valueBrush = new SolidBrush(Color.White))
                        {
                            string valueText = monthlyPurchases[i] >= 1000 ? $"₱{monthlyPurchases[i] / 1000:N0}K" : $"₱{monthlyPurchases[i]:N0}";
                            SizeF textSize = g.MeasureString(valueText, valueFont);
                            if (textSize.Width < barWidth)
                            {
                                g.DrawString(valueText, valueFont, valueBrush,
                                    x + (barWidth - textSize.Width) / 2,
                                    startY + chartHeight - barHeight - 12);
                            }
                        }
                    }
                }

                // Store bar areas in chart tag for click detection
                barChart.Tag = barAreas;
                barChart.Tag = new { Areas = barAreas, Months = monthNames, Values = monthlyPurchases, OrderCounts = monthlyOrdersCount, Dates = monthDates };

                // Add title
                using (Font titleFont = new Font("Segoe UI", 11, FontStyle.Bold))
                using (Brush titleBrush = new SolidBrush(Color.White))
                {
                    g.DrawString("MONTHLY STOCK MOVEMENT", titleFont, titleBrush, new PointF(startX + chartWidth / 2 - 90, 5));
                }
            }

            barChart.Image = bmp;
            barChart.Click += BarChart_Click;
            panel3.Controls.Add(barChart);

            // Title label
            Label title = new Label()
            {
                Text = "STOCK MOVEMENT TREND (Click on bars for details)",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                AutoSize = true
            };
            panel3.Controls.Add(title);
        }

        private void BarChart_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            PictureBox chart = sender as PictureBox;

            if (chart?.Tag != null)
            {
                try
                {
                    dynamic data = chart.Tag;
                    var areas = data.Areas as System.Collections.Generic.List<Rectangle>;

                    if (areas != null)
                    {
                        for (int i = 0; i < areas.Count; i++)
                        {
                            if (areas[i].Contains(me.Location))
                            {
                                string month = data.Months[i];
                                decimal value = data.Values[i];
                                int orderCount = data.OrderCounts[i];
                                DateTime date = data.Dates[i];

                                ShowMonthlyDetails(month, value, orderCount, date);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }
        }

        private void ShowMonthlyDetails(string month, decimal amount, int orderCount, DateTime date)
        {
            string message = $"📊 MONTHLY REPORT - {month}\n\n" +
                            $"├─ Total Spend: ₱{amount:N2}\n" +
                            $"├─ Number of Orders: {orderCount}\n" +
                            $"├─ Average Order Value: ₱{(orderCount > 0 ? amount / orderCount : 0):N2}\n" +
                            $"└─ Month: {date:MMMM yyyy}\n\n" +
                            "Click OK to view orders for this month.";

            var result = MessageBox.Show(message, "Monthly Details", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                // Navigate to orders and filter by this month
                var dashboard = this.ParentForm as DashboardForm;
                if (dashboard != null)
                {
                    dashboard.LoadForm(new PurchaseOrderForm());
                }
            }
        }

        private void ShowInventoryDetails()
        {
            var allProducts = _productService.GetAll();
            var lowStockItems = _inventoryService.GetLowStockItems();
            int healthyCount = allProducts.Count - lowStockItems.Count;
            decimal totalValue = _inventoryService.GetTotalInventoryValue(_productService);

            string message = $"📦 INVENTORY SUMMARY\n\n" +
                            $"├─ Total Products: {allProducts.Count}\n" +
                            $"├─ Healthy Stock: {healthyCount}\n" +
                            $"├─ Low Stock: {lowStockItems.Count}\n" +
                            $"├─ Total Inventory Value: ₱{totalValue:N2}\n" +
                            $"└─ Reorder Needed: {lowStockItems.Count}\n\n" +
                            "Click OK to view inventory details.";

            var result = MessageBox.Show(message, "Inventory Status", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                var dashboard = this.ParentForm as DashboardForm;
                if (dashboard != null)
                {
                    dashboard.LoadForm(new inventoryForm());
                }
            }
        }

        private void ShowHealthyStockDetails()
        {
            var allProducts = _productService.GetAll();
            var lowStockItems = _inventoryService.GetLowStockItems();
            var healthyProducts = allProducts.Where(p => !lowStockItems.Any(l => l.ProductId == p.Id)).ToList();

            string message = $"✅ HEALTHY STOCK ITEMS ({healthyProducts.Count})\n\n";

            foreach (var product in healthyProducts.Take(10))
            {
                var inventory = _inventoryService.GetByProductId(product.Id);
                message += $"• {product.Name}: {inventory?.QuantityOnHand ?? 0} units\n";
            }

            if (healthyProducts.Count > 10)
            {
                message += $"\n... and {healthyProducts.Count - 10} more items";
            }

            message += "\n\nClick OK to view all healthy stock items.";

            var result = MessageBox.Show(message, "Healthy Stock", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                var dashboard = this.ParentForm as DashboardForm;
                if (dashboard != null)
                {
                    dashboard.LoadForm(new inventoryForm());
                }
            }
        }

        private void ShowLowStockDetails()
        {
            var lowStockItems = _inventoryService.GetLowStockItems();

            if (lowStockItems.Count == 0)
            {
                MessageBox.Show("No low stock items! All products are at healthy levels.", "Low Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = $"⚠ LOW STOCK ALERTS ({lowStockItems.Count})\n\n";

            foreach (var item in lowStockItems.OrderBy(i => i.QuantityOnHand))
            {
                var product = _productService.GetById(item.ProductId);
                int needed = item.ReorderLevel - item.QuantityOnHand;
                message += $"• {product?.Name}\n";
                message += $"  Current: {item.QuantityOnHand} | Reorder at: {item.ReorderLevel} | Need: {needed}\n\n";
            }

            message += "Click OK to view and manage low stock items.";

            var result = MessageBox.Show(message, "Low Stock Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                var dashboard = this.ParentForm as DashboardForm;
                if (dashboard != null)
                {
                    dashboard.LoadForm(new inventoryForm());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var products = _productService.GetAll();
            string message = $"📊 INVENTORY SUMMARY\n\n";
            message += $"Total Products: {products.Count}\n\n";
            message += "By Category:\n";
            var categories = products.GroupBy(p => p.CategoryName);
            foreach (var category in categories)
            {
                message += $"• {category.Key}: {category.Count()}\n";
            }
            MessageBox.Show(message, "Inventory Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lowStock = _inventoryService.GetLowStockItems();
            if (lowStock.Count > 0)
            {
                string message = "⚠ LOW STOCK ITEMS ⚠\n\n";
                foreach (var item in lowStock.OrderBy(i => i.QuantityOnHand))
                {
                    var product = _productService.GetById(item.ProductId);
                    int needed = item.ReorderLevel - item.QuantityOnHand;
                    message += $"• {product?.Name}\n";
                    message += $"  Stock: {item.QuantityOnHand} / {item.ReorderLevel} (Need {needed})\n\n";
                }
                MessageBox.Show(message, "Low Stock Alerts", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("✓ All items are at healthy stock levels.", "Inventory Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var lowStock = _inventoryService.GetLowStockItems();
            if (lowStock.Count > 0)
            {
                string message = "ITEMS TO ORDER\n\n";
                foreach (var item in lowStock.OrderByDescending(i => i.ReorderLevel - i.QuantityOnHand))
                {
                    var product = _productService.GetById(item.ProductId);
                    int needed = item.ReorderLevel - item.QuantityOnHand;
                    message += $"• {product?.Name}: Order {needed} units\n";
                    message += $"  (Current: {item.QuantityOnHand}, Reorder at: {item.ReorderLevel})\n\n";
                }
                MessageBox.Show(message, "Reorder List", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No items need to be ordered.", "Reorder Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var pendingOrders = _orderService.GetPendingOrders();
            if (pendingOrders.Count > 0)
            {
                string message = "PENDING DELIVERIES\n\n";
                foreach (var order in pendingOrders)
                {
                    message += $"• Order #{order.OrderId}: {order.Supplier}\n";
                    message += $"  Status: {order.Status} | Total: ₱{order.TotalCost:N2}\n";
                    message += $"  Date: {order.OrderDate:MMM dd, yyyy}\n\n";
                }
                MessageBox.Show(message, "Pending Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No pending deliveries.", "Order Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}