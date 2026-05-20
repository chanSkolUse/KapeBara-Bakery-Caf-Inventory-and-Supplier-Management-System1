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

            // Add form activated event to refresh when form becomes active
            this.Activated += UserProfileForm_Activated;
        }

        private void UserProfileForm_Activated(object sender, EventArgs e)
        {
            // Refresh profile when form becomes active (after login)
            RefreshProfile();
        }

        public void RefreshProfile()
        {
            LoadProfile();
        }

        private void LoadProfile()
        {
            var currentUser = _authService.GetCurrentUser();

            if (currentUser != null)
            {
                // Set form title with user name
                this.Text = $"Kapebara - {currentUser.FirstName}'s Profile";

                // Load data into edit fields
                txtFullName.Text = $"{currentUser.FirstName} {currentUser.LastName}";
                txtEmail.Text = currentUser.Email;
                txtContact.Text = currentUser.ContactNumber;
                txtAge.Text = currentUser.Age.ToString();
                cmbGender.Text = currentUser.Gender;
                txtEmployeeId.Text = currentUser.EmployeeID;
                txtRole.Text = currentUser.Role;
                txtDepartment.Text = GetDepartmentFromRole(currentUser.Role);
                txtJoinDate.Text = currentUser.CreatedAt.ToString("MMMM dd, yyyy");

                // Update header display
                UpdateHeader(currentUser);

                // Set avatar based on gender
                SetAvatar(currentUser.Gender);

                // Make sure edit mode is off when loading new user
                SetEditMode(false);
            }
            else
            {
                // No user logged in - show demo data but disable editing
                LoadDemoData();
                SetEditMode(false);
                btnEditProfile.Enabled = false;
                btnEditProfile.Visible = false;
            }
        }

        private string GetDepartmentFromRole(string role)
        {
            switch (role)
            {
                case "Admin":
                    return "Management";
                case "Inventory Staff":
                    return "Inventory Department";
                case "Purchasing Staff":
                    return "Purchasing Department";
                case "Staff":
                    return "Operations";
                default:
                    return "General";
            }
        }

        private void SetAvatar(string gender)
        {
            switch (gender)
            {
                case "Male":
                    avatarLabel.Text = "👨";
                    break;
                case "Female":
                    avatarLabel.Text = "👩";
                    break;
                default:
                    avatarLabel.Text = "👤";
                    break;
            }
        }

        private void LoadDemoData()
        {
            txtFullName.Text = "Guest User";
            txtEmail.Text = "guest@kapebara.com";
            txtContact.Text = "Not available";
            txtAge.Text = "0";
            cmbGender.Text = "Prefer not to say";
            txtEmployeeId.Text = "GUEST";
            txtRole.Text = "Guest";
            txtDepartment.Text = "N/A";
            txtJoinDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            UpdateHeader(null);
            avatarLabel.Text = "👤";
        }

        private void UpdateHeader(User user)
        {
            if (user != null)
            {
                // Update profile card header using current user
                lblName.Text = $"{user.FirstName} {user.LastName}".ToUpper();
                lblRole.Text = user.Role;
                lblEmail.Text = "✉  " + user.Email;
                lblContact.Text = "☎  " + user.ContactNumber;

                // Update last login info
                lblLastLogin.Text = $"Last login: {DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")}";

                // Update member since
                lblMemberSince.Text = $"Member since: {user.CreatedAt.ToString("MMM dd, yyyy")}";
            }
            else
            {
                lblName.Text = "GUEST USER";
                lblRole.Text = "Guest";
                lblEmail.Text = "✉  guest@kapebara.com";
                lblContact.Text = "☎  Not available";
                lblLastLogin.Text = "Last login: Never";
                lblMemberSince.Text = "Member since: N/A";
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

            // Always keep these read-only
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

            // Validate inputs
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

            // Parse name into first and last
            string[] nameParts = txtFullName.Text.Trim().Split(new[] { ' ' }, 2);
            string firstName = nameParts[0];
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";

            // Update the user object
            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.Email = txtEmail.Text;
            currentUser.ContactNumber = txtContact.Text;
            currentUser.Age = age;
            currentUser.Gender = cmbGender.Text;

            // Save to service
            _authService.UpdateUser(currentUser);

            // Refresh the display
            UpdateHeader(currentUser);
            SetAvatar(currentUser.Gender);
            SetEditMode(false);

            MessageBox.Show("Profile updated successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditProfile_Click_1(object sender, EventArgs e)
        {

        }
    }
}