using KapebaraOrganizedWinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KapeBara_Inventory_Management_System.ui
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            loadForm(new dashboardPanel());
        }

        public void loadForm(object Form) {
            if (this.displayPanel.Controls.Count > 0) { 
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
            loadForm(new dashboardPanel());
        }

        private void inventoryBtn_Click(object sender, EventArgs e)
        {
            loadForm(new inventoryForm());
        }

        private void productBtn_Click(object sender, EventArgs e)
        {
            loadForm(new ProductForm());
        }

        private void orderBtn_Click(object sender, EventArgs e)
        {
            loadForm(new purchaseOrderForm());
        }

        private void supplierBtn_Click(object sender, EventArgs e)
        {
            loadForm(new supplierForm());
        }

       

        private void profileBtn_Click(object sender, EventArgs e)
        {
            loadForm(new UserProfileForm());
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            this.Hide();
            loginForm.ShowDialog();
        }
    }
}
