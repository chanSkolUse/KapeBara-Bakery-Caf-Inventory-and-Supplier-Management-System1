using System;
using System.IO;
using System.Windows.Forms;

namespace KapebaraOrganizedWinForms
{
    public partial class UserProfileForm : Form
    {
        string SavePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "profile.txt"); }
        }

        public UserProfileForm()
        {
            InitializeComponent();
            LoadProfile();
            SetEditMode(false);
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
            MessageBox.Show("You can now edit the profile.", "Edit Profile");
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            File.WriteAllText(SavePath, txtFullName.Text + "|" + txtEmail.Text + "|" + txtContact.Text + "|" + txtAge.Text + "|" + cmbGender.Text);
            UpdateHeader();
            SetEditMode(false);
            MessageBox.Show("Profile saved successfully.", "Saved");
        }

        private void LoadProfile()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            if (!File.Exists(SavePath))
            {
                File.WriteAllText(SavePath, "Ashen Lily|employee@gmail.com|0927 675 7014|28|Female");
            }

            string[] data = File.ReadAllText(SavePath).Split('|');
            txtFullName.Text = data.Length > 0 ? data[0] : "Ashen Lily";
            txtEmail.Text = data.Length > 1 ? data[1] : "employee@gmail.com";
            txtContact.Text = data.Length > 2 ? data[2] : "0927 675 7014";
            txtAge.Text = data.Length > 3 ? data[3] : "28";
            cmbGender.Text = data.Length > 4 ? data[4] : "Female";
            UpdateHeader();
        }

        private void UpdateHeader()
        {
            lblName.Text = txtFullName.Text.ToUpper();
            lblEmail.Text = "✉  " + txtEmail.Text;
            lblContact.Text = "☎  " + txtContact.Text;
        }

        private void SetEditMode(bool edit)
        {
            txtFullName.ReadOnly = !edit;
            txtEmail.ReadOnly = !edit;
            txtContact.ReadOnly = !edit;
            txtAge.ReadOnly = !edit;
            cmbGender.Enabled = edit;
        }

       

        
        private void StaffProfileForm_Load(object sender, EventArgs e)
        {

        }
    }
}
