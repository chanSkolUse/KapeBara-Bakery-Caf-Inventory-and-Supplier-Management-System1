using System;
using System.Drawing;
using System.Windows.Forms;

namespace KapebaraOrganizedWinForms
{
    partial class UserProfileForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label pageTitle;
        private Button btnEditProfile;
        private Panel profileCard;
        private Panel activityCard;
        private Label avatarLabel;
        private Label lblName;
        private Label lblRole;
        private Label lblActive;
        private Label lblEmail;
        private Label lblContact;
        private Label activityHeader;
        private Label activityText;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pageTitle = new System.Windows.Forms.Label();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.profileCard = new System.Windows.Forms.Panel();
            this.avatarLabel = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblActive = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.activityCard = new System.Windows.Forms.Panel();
            this.activityHeader = new System.Windows.Forms.Label();
            this.activityText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.line4 = new System.Windows.Forms.Panel();
            this.line3 = new System.Windows.Forms.Panel();
            this.line2 = new System.Windows.Forms.Panel();
            this.line1 = new System.Windows.Forms.Panel();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.infoHeader = new System.Windows.Forms.Label();
            this.infoCard = new System.Windows.Forms.Panel();
            this.profileCard.SuspendLayout();
            this.activityCard.SuspendLayout();
            this.panel1.SuspendLayout();
            this.infoCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageTitle
            // 
            this.pageTitle.AutoSize = true;
            this.pageTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.pageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.pageTitle.Location = new System.Drawing.Point(67, 10);
            this.pageTitle.Name = "pageTitle";
            this.pageTitle.Size = new System.Drawing.Size(246, 45);
            this.pageTitle.TabIndex = 4;
            this.pageTitle.Text = "STAFF PROFILE";
            // 
            // btnEditProfile
            // 
            this.btnEditProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.btnEditProfile.FlatAppearance.BorderSize = 0;
            this.btnEditProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditProfile.ForeColor = System.Drawing.Color.White;
            this.btnEditProfile.Location = new System.Drawing.Point(729, 10);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(150, 30);
            this.btnEditProfile.TabIndex = 0;
            this.btnEditProfile.Text = "✎ Edit Profile";
            this.btnEditProfile.UseVisualStyleBackColor = false;
            this.btnEditProfile.Click += new System.EventHandler(this.btnEditProfile_Click);
            // 
            // profileCard
            // 
            this.profileCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(205)))), ((int)(((byte)(202)))));
            this.profileCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.profileCard.Controls.Add(this.avatarLabel);
            this.profileCard.Controls.Add(this.lblName);
            this.profileCard.Controls.Add(this.lblRole);
            this.profileCard.Controls.Add(this.lblActive);
            this.profileCard.Controls.Add(this.lblEmail);
            this.profileCard.Controls.Add(this.lblContact);
            this.profileCard.Location = new System.Drawing.Point(67, 62);
            this.profileCard.Name = "profileCard";
            this.profileCard.Size = new System.Drawing.Size(820, 150);
            this.profileCard.TabIndex = 1;
            // 
            // avatarLabel
            // 
            this.avatarLabel.AutoSize = true;
            this.avatarLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 62F);
            this.avatarLabel.Location = new System.Drawing.Point(5, 10);
            this.avatarLabel.Name = "avatarLabel";
            this.avatarLabel.Size = new System.Drawing.Size(144, 111);
            this.avatarLabel.TabIndex = 0;
            this.avatarLabel.Text = "👤";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(210, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(122, 28);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "ASHEN LILY";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.lblRole.Location = new System.Drawing.Point(212, 70);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(107, 19);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Purchasing Staff";
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblActive.ForeColor = System.Drawing.Color.Green;
            this.lblActive.Location = new System.Drawing.Point(212, 96);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(63, 19);
            this.lblActive.TabIndex = 3;
            this.lblActive.Text = "● Active";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblEmail.Location = new System.Drawing.Point(510, 45);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(194, 20);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "✉  employee@gmail.com";
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblContact.Location = new System.Drawing.Point(510, 90);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(146, 20);
            this.lblContact.TabIndex = 5;
            this.lblContact.Text = "☎  0927 675 7014";
            // 
            // activityCard
            // 
            this.activityCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(219)))), ((int)(((byte)(215)))));
            this.activityCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activityCard.Controls.Add(this.activityHeader);
            this.activityCard.Controls.Add(this.activityText);
            this.activityCard.Location = new System.Drawing.Point(594, 260);
            this.activityCard.Name = "activityCard";
            this.activityCard.Size = new System.Drawing.Size(293, 285);
            this.activityCard.TabIndex = 3;
            // 
            // activityHeader
            // 
            this.activityHeader.BackColor = System.Drawing.Color.White;
            this.activityHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.activityHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.activityHeader.Location = new System.Drawing.Point(-1, 0);
            this.activityHeader.Name = "activityHeader";
            this.activityHeader.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.activityHeader.Size = new System.Drawing.Size(293, 45);
            this.activityHeader.TabIndex = 0;
            this.activityHeader.Text = "Activity Status";
            this.activityHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // activityText
            // 
            this.activityText.AutoSize = true;
            this.activityText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.activityText.ForeColor = System.Drawing.Color.Gray;
            this.activityText.Location = new System.Drawing.Point(35, 78);
            this.activityText.Name = "activityText";
            this.activityText.Size = new System.Drawing.Size(264, 95);
            this.activityText.TabIndex = 1;
            this.activityText.Text = "●──── Active 15 minutes ago\n│\n●──── Active 30 minutes ago\n│\n●──── Created an orde" +
    "r on April 28, 2026";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pageTitle);
            this.panel1.Controls.Add(this.btnEditProfile);
            this.panel1.Controls.Add(this.activityCard);
            this.panel1.Controls.Add(this.profileCard);
            this.panel1.Controls.Add(this.infoCard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 554);
            this.panel1.TabIndex = 5;
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.btnSaveChanges.FlatAppearance.BorderSize = 0;
            this.btnSaveChanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveChanges.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveChanges.ForeColor = System.Drawing.Color.White;
            this.btnSaveChanges.Location = new System.Drawing.Point(352, 8);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(125, 28);
            this.btnSaveChanges.TabIndex = 15;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = false;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // line4
            // 
            this.line4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line4.Location = new System.Drawing.Point(0, 228);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(500, 1);
            this.line4.TabIndex = 14;
            // 
            // line3
            // 
            this.line3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line3.Location = new System.Drawing.Point(0, 183);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(500, 1);
            this.line3.TabIndex = 13;
            // 
            // line2
            // 
            this.line2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line2.Location = new System.Drawing.Point(0, 138);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(500, 1);
            this.line2.TabIndex = 12;
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line1.Location = new System.Drawing.Point(0, 93);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(500, 1);
            this.line1.TabIndex = 11;
            // 
            // cmbGender
            // 
            this.cmbGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.cmbGender.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Prefer not to say"});
            this.cmbGender.Location = new System.Drawing.Point(220, 237);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(220, 25);
            this.cmbGender.TabIndex = 10;
            // 
            // txtAge
            // 
            this.txtAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtAge.Location = new System.Drawing.Point(220, 192);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(220, 25);
            this.txtAge.TabIndex = 9;
            // 
            // txtContact
            // 
            this.txtContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtContact.Location = new System.Drawing.Point(220, 147);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(220, 25);
            this.txtContact.TabIndex = 8;
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtEmail.Location = new System.Drawing.Point(220, 102);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(220, 25);
            this.txtEmail.TabIndex = 7;
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtFullName.Location = new System.Drawing.Point(220, 57);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(220, 25);
            this.txtFullName.TabIndex = 6;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGender.ForeColor = System.Drawing.Color.Gray;
            this.lblGender.Location = new System.Drawing.Point(18, 240);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(58, 19);
            this.lblGender.TabIndex = 5;
            this.lblGender.Text = "Gender";
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAge.ForeColor = System.Drawing.Color.Gray;
            this.lblAge.Location = new System.Drawing.Point(18, 195);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(36, 19);
            this.lblAge.TabIndex = 4;
            this.lblAge.Text = "Age";
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.AutoSize = true;
            this.lblContactNumber.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblContactNumber.ForeColor = System.Drawing.Color.Gray;
            this.lblContactNumber.Location = new System.Drawing.Point(18, 150);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(119, 19);
            this.lblContactNumber.TabIndex = 3;
            this.lblContactNumber.Text = "Contact Number";
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmailAddress.ForeColor = System.Drawing.Color.Gray;
            this.lblEmailAddress.Location = new System.Drawing.Point(18, 105);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(103, 19);
            this.lblEmailAddress.TabIndex = 2;
            this.lblEmailAddress.Text = "Email Address";
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFullName.ForeColor = System.Drawing.Color.Gray;
            this.lblFullName.Location = new System.Drawing.Point(18, 60);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(76, 19);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "Full Name";
            // 
            // infoHeader
            // 
            this.infoHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(205)))), ((int)(((byte)(202)))));
            this.infoHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.infoHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.infoHeader.Location = new System.Drawing.Point(0, -1);
            this.infoHeader.Name = "infoHeader";
            this.infoHeader.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.infoHeader.Size = new System.Drawing.Size(499, 45);
            this.infoHeader.TabIndex = 0;
            this.infoHeader.Text = "User Information";
            this.infoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // infoCard
            // 
            this.infoCard.BackColor = System.Drawing.Color.White;
            this.infoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoCard.Controls.Add(this.btnSaveChanges);
            this.infoCard.Controls.Add(this.infoHeader);
            this.infoCard.Controls.Add(this.lblFullName);
            this.infoCard.Controls.Add(this.lblEmailAddress);
            this.infoCard.Controls.Add(this.lblContactNumber);
            this.infoCard.Controls.Add(this.lblAge);
            this.infoCard.Controls.Add(this.lblGender);
            this.infoCard.Controls.Add(this.txtFullName);
            this.infoCard.Controls.Add(this.txtEmail);
            this.infoCard.Controls.Add(this.txtContact);
            this.infoCard.Controls.Add(this.txtAge);
            this.infoCard.Controls.Add(this.cmbGender);
            this.infoCard.Controls.Add(this.line1);
            this.infoCard.Controls.Add(this.line2);
            this.infoCard.Controls.Add(this.line3);
            this.infoCard.Controls.Add(this.line4);
            this.infoCard.Location = new System.Drawing.Point(67, 260);
            this.infoCard.Name = "infoCard";
            this.infoCard.Size = new System.Drawing.Size(500, 285);
            this.infoCard.TabIndex = 2;
            // 
            // StaffProfileForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(954, 554);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StaffProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kapebara - Staff Profile";
            this.Load += new System.EventHandler(this.StaffProfileForm_Load);
            this.profileCard.ResumeLayout(false);
            this.profileCard.PerformLayout();
            this.activityCard.ResumeLayout(false);
            this.activityCard.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.infoCard.ResumeLayout(false);
            this.infoCard.PerformLayout();
            this.ResumeLayout(false);

        }


        private void SetupInfoLabel(Label label, string text, int x, int y)
        {
            label.Text = text;
            label.Location = new Point(x, y);
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label.ForeColor = Color.Gray;
            label.AutoSize = true;
        }

        private void SetupTextBox(TextBox box, int x, int y)
        {
            box.Location = new Point(x, y);
            box.Size = new Size(220, 28);
            box.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            box.BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetupLine(Panel line, int x, int y)
        {
            line.Location = new Point(x, y);
            line.Size = new Size(500, 1);
        }

        private Panel panel1;
        private Panel infoCard;
        private Label infoHeader;
        private Label lblFullName;
        private Label lblEmailAddress;
        private Label lblContactNumber;
        private Label lblAge;
        private Label lblGender;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtContact;
        private TextBox txtAge;
        private ComboBox cmbGender;
        private Panel line1;
        private Panel line2;
        private Panel line3;
        private Panel line4;
        private Button btnSaveChanges;
    }
}
