using TestProject.Services;
using TestProject.ui;
using System;
using System.Windows.Forms;
using TestProject;


namespace TestProject.Forms
{
    public partial class Login : Form
    {
        private AuthService _authService;

        public Login()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string email = txtBxEmail.Text.Trim();
            string password = txtBxPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_authService.ValidateCredentials(email, password))
            {
                MessageBox.Show($"Welcome back, {_authService.GetCurrentUser()?.FullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DashboardForm dashboard = new DashboardForm();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblSignUp_Click(object sender, EventArgs e)
        {
            Registration registrationForm = new Registration();
            registrationForm.Show();
            this.Hide();
        }
    }
}