namespace TestProject.Forms
{
    partial class ProductForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;

        // Category buttons
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnCoffee;
        private System.Windows.Forms.Button btnSyrups;
        private System.Windows.Forms.Button btnDairy;
        private System.Windows.Forms.Button btnToppings;
        private System.Windows.Forms.Button btnFlour;
        private System.Windows.Forms.Button btnBreads;
        private System.Windows.Forms.Button btnPastries;
        private System.Windows.Forms.Button btnDrinks;
        private System.Windows.Forms.FlowLayoutPanel flowCategoryPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.flowCategoryPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnCoffee = new System.Windows.Forms.Button();
            this.btnSyrups = new System.Windows.Forms.Button();
            this.btnDairy = new System.Windows.Forms.Button();
            this.btnToppings = new System.Windows.Forms.Button();
            this.btnFlour = new System.Windows.Forms.Button();
            this.btnBreads = new System.Windows.Forms.Button();
            this.btnPastries = new System.Windows.Forms.Button();
            this.btnDrinks = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "Products";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(850, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 35);
            this.button1.TabIndex = 3;
            this.button1.Text = "+ Create Product";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(480, 88);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 25);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBox1.Location = new System.Drawing.Point(8, 88);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(466, 25);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Search products...";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.flowCategoryPanel);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Location = new System.Drawing.Point(12, 57);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(946, 524);
            this.panel1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 15);
            this.label3.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.panel2.Location = new System.Drawing.Point(8, 125);
            this.panel2.Margin = new System.Windows.Forms.Padding(10);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(930, 391);
            this.panel2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.label2.Location = new System.Drawing.Point(8, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "All Products";
            this.label2.Visible = false;
            // 
            // flowCategoryPanel
            // 
            this.flowCategoryPanel.AutoScroll = true;
            this.flowCategoryPanel.BackColor = System.Drawing.Color.White;
            this.flowCategoryPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowCategoryPanel.Location = new System.Drawing.Point(5, 5);
            this.flowCategoryPanel.Name = "flowCategoryPanel";
            this.flowCategoryPanel.Padding = new System.Windows.Forms.Padding(10, 8, 0, 0);
            this.flowCategoryPanel.Size = new System.Drawing.Size(936, 50);
            this.flowCategoryPanel.TabIndex = 11;
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(0, 0);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 0;
            // 
            // btnCoffee
            // 
            this.btnCoffee.Location = new System.Drawing.Point(0, 0);
            this.btnCoffee.Name = "btnCoffee";
            this.btnCoffee.Size = new System.Drawing.Size(75, 23);
            this.btnCoffee.TabIndex = 0;
            // 
            // btnSyrups
            // 
            this.btnSyrups.Location = new System.Drawing.Point(0, 0);
            this.btnSyrups.Name = "btnSyrups";
            this.btnSyrups.Size = new System.Drawing.Size(75, 23);
            this.btnSyrups.TabIndex = 0;
            // 
            // btnDairy
            // 
            this.btnDairy.Location = new System.Drawing.Point(0, 0);
            this.btnDairy.Name = "btnDairy";
            this.btnDairy.Size = new System.Drawing.Size(75, 23);
            this.btnDairy.TabIndex = 0;
            // 
            // btnToppings
            // 
            this.btnToppings.Location = new System.Drawing.Point(0, 0);
            this.btnToppings.Name = "btnToppings";
            this.btnToppings.Size = new System.Drawing.Size(75, 23);
            this.btnToppings.TabIndex = 0;
            // 
            // btnFlour
            // 
            this.btnFlour.Location = new System.Drawing.Point(0, 0);
            this.btnFlour.Name = "btnFlour";
            this.btnFlour.Size = new System.Drawing.Size(75, 23);
            this.btnFlour.TabIndex = 0;
            // 
            // btnBreads
            // 
            this.btnBreads.Location = new System.Drawing.Point(0, 0);
            this.btnBreads.Name = "btnBreads";
            this.btnBreads.Size = new System.Drawing.Size(75, 23);
            this.btnBreads.TabIndex = 0;
            // 
            // btnPastries
            // 
            this.btnPastries.Location = new System.Drawing.Point(0, 0);
            this.btnPastries.Name = "btnPastries";
            this.btnPastries.Size = new System.Drawing.Size(75, 23);
            this.btnPastries.TabIndex = 0;
            // 
            // btnDrinks
            // 
            this.btnDrinks.Location = new System.Drawing.Point(0, 0);
            this.btnDrinks.Name = "btnDrinks";
            this.btnDrinks.Size = new System.Drawing.Size(75, 23);
            this.btnDrinks.TabIndex = 0;
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(970, 593);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProductForm";
            this.Text = "Products";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}