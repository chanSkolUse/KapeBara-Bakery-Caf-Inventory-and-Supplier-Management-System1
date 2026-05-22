using TestProject.Services;
using TestProject.Models;
using System;
using System.Windows.Forms;

namespace TestProject
{
    public partial class UserProfileForm : Form
    {
        private AuthService _authService;
        private bool _isEditing = false;

        public UserProfileForm()
        {
            InitializeComponent();
            _authService = new AuthService();
            LoadProfile();
            SetEditMode(false);
            WireEvents();
        }

        private void WireEvents()
        {
            btnEditProfile.Click += BtnEditProfile_Click;
            btnSaveChanges.Click += BtnSaveChanges_Click;
        }

        private void LoadProfile()
        {
            var currentUser = _authService.GetCurrentUser();

            if (currentUser != null)
            {
                this.Text = $"Kapebara - {currentUser.FullName}'s Profile";

                txtFullName.Text = currentUser.FullName;
                txtEmail.Text = currentUser.Email;
                txtContact.Text = currentUser.ContactNumber;
                txtAge.Text = currentUser.Age.ToString();
                cmbGender.Text = currentUser.Gender;
                txtEmployeeId.Text = currentUser.EmployeeID;
                txtRole.Text = currentUser.Role;
                txtDepartment.Text = GetDepartmentFromRole(currentUser.Role);
                txtJoinDate.Text = currentUser.CreatedAt.ToString("MMMM dd, yyyy");

                UpdateHeader(currentUser);
                SetAvatar(currentUser.Gender);

                btnEditProfile.Enabled = true;
                btnEditProfile.Visible = true;
            }
            else
            {
                LoadNoUserData();
                SetEditMode(false);
                btnEditProfile.Enabled = false;
                btnEditProfile.Visible = false;
                btnSaveChanges.Visible = false;
            }
        }

        private string GetDepartmentFromRole(string role)
        {
            if (string.IsNullOrEmpty(role)) return "General";
            switch (role)
            {
                case "Admin": return "Management";
                case "Staff": return "Operations";
                default: return "General";
            }
        }

        private void SetAvatar(string gender)
        {
            if (string.IsNullOrEmpty(gender))
            {
                avatarLabel.Text = "👤";
                return;
            }
            switch (gender)
            {
                case "Male": avatarLabel.Text = "👨"; break;
                case "Female": avatarLabel.Text = "👩"; break;
                default: avatarLabel.Text = "👤"; break;
            }
        }

        private void LoadNoUserData()
        {
            txtFullName.Text = "Not Logged In";
            txtEmail.Text = "Please login to view profile";
            txtContact.Text = "N/A";
            txtAge.Text = "0";
            cmbGender.Text = "Prefer not to say";
            txtEmployeeId.Text = "N/A";
            txtRole.Text = "Guest";
            txtDepartment.Text = "N/A";
            txtJoinDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");

            lblName.Text = "NOT LOGGED IN";
            lblRole.Text = "Guest";
            lblEmail.Text = "✉  Please login";
            lblContact.Text = "☎  N/A";
            lblLastLogin.Text = "Last login: Never";
            lblMemberSince.Text = "Member since: N/A";
            avatarLabel.Text = "👤";
        }

        private void UpdateHeader(User user)
        {
            if (user != null)
            {
                lblName.Text = user.FullName.ToUpper();
                lblRole.Text = user.Role;
                lblEmail.Text = "✉  " + user.Email;
                lblContact.Text = "☎  " + user.ContactNumber;
                lblLastLogin.Text = $"Last login: {DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")}";
                lblMemberSince.Text = $"Member since: {user.CreatedAt.ToString("MMM dd, yyyy")}";
            }
        }

        private void SetEditMode(bool edit)
        {
            _isEditing = edit;
            txtFullName.ReadOnly = !edit;
            txtEmail.ReadOnly = !edit;
            txtContact.ReadOnly = !edit;
            txtAge.ReadOnly = !edit;
            cmbGender.Enabled = edit;
            btnSaveChanges.Visible = edit;
            btnEditProfile.Visible = !edit;

            txtEmployeeId.ReadOnly = true;
            txtRole.ReadOnly = true;
            txtDepartment.ReadOnly = true;
            txtJoinDate.ReadOnly = true;
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser != null)
            {
                SetEditMode(true);
                MessageBox.Show("You can now edit your profile information.", "Edit Mode",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("You must be logged in to edit your profile.", "Not Logged In",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            var currentUser = _authService.GetCurrentUser();

            if (currentUser == null)
            {
                MessageBox.Show("Unable to save profile. You are not logged in.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Please enter your full name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Please enter your contact number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 18 || age > 100)
            {
                MessageBox.Show("Please enter a valid age (18-100).", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentUser.FullName = txtFullName.Text.Trim();
            currentUser.Email = txtEmail.Text;
            currentUser.ContactNumber = txtContact.Text;
            currentUser.Age = age;
            currentUser.Gender = cmbGender.Text;

            _authService.UpdateUser(currentUser);
            _authService.RefreshCurrentUser();

            LoadProfile();
            SetEditMode(false);

            MessageBox.Show("Profile updated successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void infoCard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}