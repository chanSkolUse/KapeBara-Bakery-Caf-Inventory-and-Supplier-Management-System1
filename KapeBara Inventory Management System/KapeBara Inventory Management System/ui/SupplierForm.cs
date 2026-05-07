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
    public partial class supplierForm : Form
    {
        public supplierForm()
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
            ProductForm adminLoginForm = new ProductForm();
            this.Hide();
        }

        private void inventoryBtn_Click(object sender, EventArgs e)
        {
            userProfileForm inventoryLoginForm = new userProfileForm();
            this.Hide();
            inventoryLoginForm.Show();
        }

        private void purchasingBtn_Click(object sender, EventArgs e)
        {
            RegisterForm purchasingLoginForm = new RegisterForm();
            purchasingLoginForm.Show();
            this.Hide();
        }
    }
}
