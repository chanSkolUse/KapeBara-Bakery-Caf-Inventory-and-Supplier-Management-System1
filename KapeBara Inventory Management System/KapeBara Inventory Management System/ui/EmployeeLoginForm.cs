using KapeBara_Inventory_Management_System.ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KapeBara_Inventory_Management_System
{
    public partial class EmployeeLoginForm : Form
    {
        public EmployeeLoginForm()
        {
            InitializeComponent();
        }

    
   

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void adminBtn_Click(object sender, EventArgs e)
        {
            AdminLoginForm adminLoginForm = new AdminLoginForm();
            adminLoginForm.Show();
            this.Hide();
        }

        private void inventoryBtn_Click(object sender, EventArgs e)
        {
            InventoryLoginForm inventoryLoginForm = new InventoryLoginForm();
            this.Hide();
            inventoryLoginForm.Show();
        }

        private void purchasingBtn_Click(object sender, EventArgs e)
        {
            PurchasingLoginForm purchasingLoginForm = new PurchasingLoginForm();
            purchasingLoginForm.Show();
            this.Hide();
        }
    }
}
