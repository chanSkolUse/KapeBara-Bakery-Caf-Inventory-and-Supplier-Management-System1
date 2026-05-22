using TestProject.Models;
using TestProject.Services;
using System;
using System.Windows.Forms;
using TestProject.Forms;

namespace TestProject
{
    public partial class Registration : Form
    {
        private AuthService _authService;

        public Registration()
        {
            InitializeComponent();
            _authService = new AuthService();

            userPosition.Items.Clear();
            userPosition.Items.Add("Staff");
            userPosition.Items.Add("Admin");
            userPosition.SelectedIndex = 0;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtBxStaffID.Text))
            {
                MessageBox.Show("Please enter your Staff ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxStaffID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtBxAge.Text) || !int.TryParse(txtBxAge.Text, out int age) || age < 18 || age > 65)
            {
                MessageBox.Show("Please enter a valid age (18-65).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxAge.Focus();
                return;
            }

            if (!maleRb.Checked && !femaleRb.Checked)
            {
                MessageBox.Show("Please select your gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtBxWorkEmail.Text) || !txtBxWorkEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxWorkEmail.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtBxContactNum.Text))
            {
                MessageBox.Show("Please enter your contact number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxContactNum.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtBxPassword.Text) || txtBxPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxPassword.Focus();
                return;
            }

            if (txtBxPassword.Text != txtBxConfirmPasswd.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBxConfirmPasswd.Focus();
                return;
            }

            string gender = maleRb.Checked ? "Male" : "Female";

            var user = new User
            {
                FullName = txtFullName.Text.Trim(),
                EmployeeID = txtBxStaffID.Text.Trim(),
                Age = age,
                Gender = gender,
                Email = txtBxWorkEmail.Text.Trim(),
                ContactNumber = txtBxContactNum.Text.Trim(),
                Password = txtBxPassword.Text,
                Role = userPosition.SelectedItem?.ToString() ?? "Staff",
                CreatedAt = DateTime.Now
            };

            if (_authService.Register(user))
            {
                MessageBox.Show("Registration successful! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear form
                txtFullName.Clear();
                txtBxStaffID.Clear();
                txtBxAge.Clear();
                txtBxWorkEmail.Clear();
                txtBxContactNum.Clear();
                txtBxPassword.Clear();
                txtBxConfirmPasswd.Clear();
                maleRb.Checked = false;
                femaleRb.Checked = false;
                userPosition.SelectedIndex = 0;

                // Go back to login
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email already exists. Please use a different email.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}