using TestProject.Forms;
using System;
using System.Windows.Forms;
using TestProject;
using TestProject.ui;

namespace TestProject.ui
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            LoadForm(new dashboardPanel());
        }

        public void LoadForm(object Form)
        {
            if (this.displayPanel.Controls.Count > 0)
            {
                this.displayPanel.Controls.RemoveAt(0);
            }
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.displayPanel.Controls.Add(f);
            this.displayPanel.Tag = f;
            f.Show();
        }

        private void dashboardBtn_Click(object sender, EventArgs e)
        {
            LoadForm(new dashboardPanel());
        }

        private void inventoryBtn_Click(object sender, EventArgs e)
        {
            LoadForm(new inventoryForm());
        }

        private void productBtn_Click(object sender, EventArgs e)
        {
            LoadForm(new ProductForm());
        }

        private void orderBtn_Click(object sender, EventArgs e)
        {
            LoadForm(new PurchaseOrderForm());
        }

        private void supplierBtn_Click(object sender, EventArgs e)
        {
            LoadForm(new supplierForm());
        }

        
        private void logoutBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void toggleSidebar_Click_1(object sender, EventArgs e)
        {
            if (sidebarPanel.Width == 200)
            {
                sidebarPanel.Width = 50;
            }
            else
            {
                sidebarPanel.Width = 200;
            }
        }

        private void profileBtn_Click_1(object sender, EventArgs e)
        {
            //LoadForm(new UserProfileForm());
            var profileForm = new UserProfileForm();
            LoadForm(profileForm);
        }
    }
}