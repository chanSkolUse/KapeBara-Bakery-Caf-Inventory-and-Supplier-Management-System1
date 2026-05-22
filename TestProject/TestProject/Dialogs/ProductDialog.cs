using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Dialogs
{
    public partial class ProductDialog : Form
    {
        private Product _product;
        private int _stockQuantity = 0;
        private int _reorderLevel = 10;
        private string _selectedImagePath = null;
        private byte[] _selectedImageBytes = null;

        public ProductDialog(Product product = null)
        {
            InitializeComponent();
            _product = product ?? new Product();
            LoadCategories();
            
            if (product != null)
            {
                LoadProductData();
                txtStock.Enabled = false;
                this.Text = "Edit Product";
            }
            else
            {
                this.Text = "Add New Product";
            }
        }

        // In ProductDialog.cs, replace any switch expressions with traditional switch statements
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(new ComboboxItem { Text = "Coffee", Value = 1 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Syrups", Value = 2 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Dairy", Value = 3 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Toppings", Value = 4 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Flour", Value = 5 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Breads", Value = 6 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Pastries", Value = 7 });
            cmbCategory.Items.Add(new ComboboxItem { Text = "Drinks", Value = 8 });
            cmbCategory.DisplayMember = "Text";
            cmbCategory.ValueMember = "Value";
            cmbCategory.SelectedIndex = 0;
        }

        private void LoadProductData()
        {
            txtName.Text = _product.Name;
            txtSKU.Text = _product.SKU;
            txtUnitPrice.Text = _product.UnitPrice.ToString();
            txtDescription.Text = _product.Description;

            // Set category
            for (int i = 0; i < cmbCategory.Items.Count; i++)
            {
                var item = cmbCategory.Items[i] as ComboboxItem;
                if (item != null && item.Text == _product.CategoryName)
                {
                    cmbCategory.SelectedIndex = i;
                    break;
                }
            }

            // Load image if exists with null check
            try
            {
                if (_product.ProductImage != null && _product.ProductImage.Length > 0)
                {
                    using (var ms = new MemoryStream(_product.ProductImage))
                    {
                        pbProductImage.Image = Image.FromStream(ms);
                    }
                }
                else if (!string.IsNullOrEmpty(_product.ImagePath) && File.Exists(_product.ImagePath))
                {
                    pbProductImage.Image = Image.FromFile(_product.ImagePath);
                    _selectedImagePath = _product.ImagePath;
                }
                else
                {
                    // Create a simple default image instead of using Properties.Resources
                    Bitmap defaultImg = new Bitmap(120, 120);
                    using (Graphics g = Graphics.FromImage(defaultImg))
                    {
                        g.Clear(Color.FromArgb(240, 240, 240));
                        using (Font font = new Font("Segoe UI", 12))
                        {
                            g.DrawString("No Image", font, Brushes.Gray, 30, 50);
                        }
                    }
                    pbProductImage.Image = defaultImg;
                    pbProductImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception)
            {
                // Fallback default image
                Bitmap defaultImg = new Bitmap(120, 120);
                using (Graphics g = Graphics.FromImage(defaultImg))
                {
                    g.Clear(Color.FromArgb(240, 240, 240));
                }
                pbProductImage.Image = defaultImg;
                pbProductImage.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        public Product GetProduct()
        {
            _product.Name = txtName.Text;
            _product.SKU = txtSKU.Text;
            _product.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            _product.Description = txtDescription.Text;
            
            var selectedCategory = cmbCategory.SelectedItem as ComboboxItem;
            if (selectedCategory != null)
            {
                _product.CategoryName = selectedCategory.Text;
                _product.CategoryId = (int)selectedCategory.Value;
            }
            
            // Save image
            if (_selectedImageBytes != null)
            {
                _product.ProductImage = _selectedImageBytes;
            }
            else if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                _product.ImagePath = _selectedImagePath;
            }
            
            return _product;
        }

        public int GetStockQuantity() => _stockQuantity;
        public int GetReorderLevel() => _reorderLevel;

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select Product Image";
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _selectedImagePath = openFileDialog.FileName;
                        _selectedImageBytes = File.ReadAllBytes(_selectedImagePath);
                        pbProductImage.Image = Image.FromFile(_selectedImagePath);
                        pbProductImage.SizeMode = PictureBoxSizeMode.Zoom;
                        lblImageStatus.Text = "Image loaded successfully";
                        lblImageStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            // Create a simple default image instead of using Properties.Resources
            Bitmap defaultImg = new Bitmap(120, 120);
            using (Graphics g = Graphics.FromImage(defaultImg))
            {
                g.Clear(Color.FromArgb(240, 240, 240));
                using (Font font = new Font("Segoe UI", 10))
                {
                    g.DrawString("No Image", font, Brushes.Gray, 35, 50);
                }
            }
            pbProductImage.Image = defaultImg;
            pbProductImage.SizeMode = PictureBoxSizeMode.Zoom;
            _selectedImageBytes = null;
            _selectedImagePath = null;
            lblImageStatus.Text = "No image selected";
            lblImageStatus.ForeColor = System.Drawing.Color.Gray;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter product name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid unit price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return;
            }
            if (txtStock.Enabled && !int.TryParse(txtStock.Text, out _stockQuantity))
            {
                _stockQuantity = 0;
            }
            if (!int.TryParse(txtReorderLevel.Text, out _reorderLevel) || _reorderLevel < 0)
            {
                _reorderLevel = 10;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblSKU = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.lblReorderLevel = new System.Windows.Forms.Label();
            this.txtReorderLevel = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblProductImage = new System.Windows.Forms.Label();
            this.pbProductImage = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.btnRemoveImage = new System.Windows.Forms.Button();
            this.lblImageStatus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(25, 25);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(102, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Product Name:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtName.Location = new System.Drawing.Point(140, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(280, 25);
            this.txtName.TabIndex = 1;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCategory.Location = new System.Drawing.Point(25, 60);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(69, 19);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(140, 57);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(280, 25);
            this.cmbCategory.TabIndex = 3;
            // 
            // lblSKU
            // 
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSKU.Location = new System.Drawing.Point(25, 95);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(38, 19);
            this.lblSKU.TabIndex = 4;
            this.lblSKU.Text = "SKU:";
            // 
            // txtSKU
            // 
            this.txtSKU.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSKU.Location = new System.Drawing.Point(140, 92);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(280, 25);
            this.txtSKU.TabIndex = 5;
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUnitPrice.Location = new System.Drawing.Point(25, 130);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(73, 19);
            this.lblUnitPrice.TabIndex = 6;
            this.lblUnitPrice.Text = "Unit Price:";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUnitPrice.Location = new System.Drawing.Point(140, 127);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(150, 25);
            this.txtUnitPrice.TabIndex = 7;
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStock.Location = new System.Drawing.Point(25, 165);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(97, 19);
            this.lblStock.TabIndex = 8;
            this.lblStock.Text = "Initial Stock:";
            // 
            // txtStock
            // 
            this.txtStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtStock.Location = new System.Drawing.Point(140, 162);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(100, 25);
            this.txtStock.TabIndex = 9;
            this.txtStock.Text = "0";
            // 
            // lblReorderLevel
            // 
            this.lblReorderLevel.AutoSize = true;
            this.lblReorderLevel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblReorderLevel.Location = new System.Drawing.Point(25, 200);
            this.lblReorderLevel.Name = "lblReorderLevel";
            this.lblReorderLevel.Size = new System.Drawing.Size(101, 19);
            this.lblReorderLevel.TabIndex = 10;
            this.lblReorderLevel.Text = "Reorder Level:";
            // 
            // txtReorderLevel
            // 
            this.txtReorderLevel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtReorderLevel.Location = new System.Drawing.Point(140, 197);
            this.txtReorderLevel.Name = "txtReorderLevel";
            this.txtReorderLevel.Size = new System.Drawing.Size(100, 25);
            this.txtReorderLevel.TabIndex = 11;
            this.txtReorderLevel.Text = "10";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(25, 235);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(83, 19);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescription.Location = new System.Drawing.Point(140, 232);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(280, 60);
            this.txtDescription.TabIndex = 13;
            // 
            // lblProductImage
            // 
            this.lblProductImage.AutoSize = true;
            this.lblProductImage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductImage.Location = new System.Drawing.Point(25, 310);
            this.lblProductImage.Name = "lblProductImage";
            this.lblProductImage.Size = new System.Drawing.Size(98, 19);
            this.lblProductImage.TabIndex = 14;
            this.lblProductImage.Text = "Product Image:";
            // 
            // pbProductImage
            // 
            this.pbProductImage.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.pbProductImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbProductImage.Location = new System.Drawing.Point(140, 305);
            this.pbProductImage.Name = "pbProductImage";
            this.pbProductImage.Size = new System.Drawing.Size(120, 120);
            this.pbProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbProductImage.TabIndex = 15;
            this.pbProductImage.TabStop = false;
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.ForeColor = System.Drawing.Color.White;
            this.btnSelectImage.Location = new System.Drawing.Point(270, 305);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(75, 30);
            this.btnSelectImage.TabIndex = 16;
            this.btnSelectImage.Text = "Browse...";
            this.btnSelectImage.UseVisualStyleBackColor = false;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveImage.ForeColor = System.Drawing.Color.White;
            this.btnRemoveImage.Location = new System.Drawing.Point(270, 345);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(75, 30);
            this.btnRemoveImage.TabIndex = 17;
            this.btnRemoveImage.Text = "Remove";
            this.btnRemoveImage.UseVisualStyleBackColor = false;
            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);
            // 
            // lblImageStatus
            // 
            this.lblImageStatus.AutoSize = true;
            this.lblImageStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblImageStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblImageStatus.Location = new System.Drawing.Point(270, 390);
            this.lblImageStatus.Name = "lblImageStatus";
            this.lblImageStatus.Size = new System.Drawing.Size(87, 13);
            this.lblImageStatus.TabIndex = 18;
            this.lblImageStatus.Text = "No image selected";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(120, 71, 70);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(140, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save Product";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(280, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProductDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(450, 510);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblImageStatus);
            this.Controls.Add(this.btnRemoveImage);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.pbProductImage);
            this.Controls.Add(this.lblProductImage);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtReorderLevel);
            this.Controls.Add(this.lblReorderLevel);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.txtUnitPrice);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.txtSKU);
            this.Controls.Add(this.lblSKU);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Details";
            ((System.ComponentModel.ISupportInitialize)(this.pbProductImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Label lblReorderLevel;
        private System.Windows.Forms.TextBox txtReorderLevel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblProductImage;
        private System.Windows.Forms.PictureBox pbProductImage;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Button btnRemoveImage;
        private System.Windows.Forms.Label lblImageStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}