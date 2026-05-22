using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestProject
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
        private Panel panel1;
        private Button btnSaveChanges;
        private Panel line4;
        private Panel line3;
        private Panel line2;
        private Panel line1;
        private ComboBox cmbGender;
        private TextBox txtAge;
        private TextBox txtContact;
        private TextBox txtEmail;
        private TextBox txtFullName;
        private Label lblGender;
        private Label lblAge;
        private Label lblContactNumber;
        private Label lblEmailAddress;
        private Label lblFullName;
        private Label infoHeader;
        private Panel infoCard;

        // Additional fields for employee information
        private Label lblEmployeeId;
        private TextBox txtEmployeeId;
        private Label lblRoleField;
        private TextBox txtRole;
        private Label lblDepartment;
        private TextBox txtDepartment;
        private Label lblJoinDate;
        private TextBox txtJoinDate;
        private Label lblLastLogin;
        private Label lblMemberSince;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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
            this.lblLastLogin = new System.Windows.Forms.Label();
            this.lblMemberSince = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.infoCard = new System.Windows.Forms.Panel();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.infoHeader = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblEmailAddress = new System.Windows.Forms.Label();
            this.lblContactNumber = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.lblEmployeeId = new System.Windows.Forms.Label();
            this.txtEmployeeId = new System.Windows.Forms.TextBox();
            this.lblRoleField = new System.Windows.Forms.Label();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.lblJoinDate = new System.Windows.Forms.Label();
            this.txtJoinDate = new System.Windows.Forms.TextBox();
            this.line1 = new System.Windows.Forms.Panel();
            this.line2 = new System.Windows.Forms.Panel();
            this.line3 = new System.Windows.Forms.Panel();
            this.line4 = new System.Windows.Forms.Panel();
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
            this.lblName.Size = new System.Drawing.Size(134, 28);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "STAFF NAME";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(75)))), ((int)(((byte)(74)))));
            this.lblRole.Location = new System.Drawing.Point(212, 70);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(66, 19);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Staff Role";
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
            this.lblEmail.Size = new System.Drawing.Size(184, 20);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "✉  email@example.com";
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblContact.Location = new System.Drawing.Point(510, 90);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(150, 20);
            this.lblContact.TabIndex = 5;
            this.lblContact.Text = "☎  contact number";
            // 
            // activityCard
            // 
            this.activityCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(219)))), ((int)(((byte)(215)))));
            this.activityCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activityCard.Controls.Add(this.activityHeader);
            this.activityCard.Controls.Add(this.activityText);
            this.activityCard.Controls.Add(this.lblLastLogin);
            this.activityCard.Controls.Add(this.lblMemberSince);
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
            this.activityText.Size = new System.Drawing.Size(186, 95);
            this.activityText.TabIndex = 1;
            this.activityText.Text = "●──── Active 15 minutes ago\n│\n●──── Active 30 minutes ago\n│\n●──── Logged in today" +
    "";
            // 
            // lblLastLogin
            // 
            this.lblLastLogin.AutoSize = true;
            this.lblLastLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLastLogin.ForeColor = System.Drawing.Color.Gray;
            this.lblLastLogin.Location = new System.Drawing.Point(35, 190);
            this.lblLastLogin.Name = "lblLastLogin";
            this.lblLastLogin.Size = new System.Drawing.Size(144, 15);
            this.lblLastLogin.TabIndex = 2;
            this.lblLastLogin.Text = "Last login: Today, 9:30 AM";
            // 
            // lblMemberSince
            // 
            this.lblMemberSince.AutoSize = true;
            this.lblMemberSince.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMemberSince.ForeColor = System.Drawing.Color.Gray;
            this.lblMemberSince.Location = new System.Drawing.Point(35, 215);
            this.lblMemberSince.Name = "lblMemberSince";
            this.lblMemberSince.Size = new System.Drawing.Size(112, 15);
            this.lblMemberSince.TabIndex = 3;
            this.lblMemberSince.Text = "Member since: 2024";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pageTitle);
            this.panel1.Controls.Add(this.btnEditProfile);
            this.panel1.Controls.Add(this.activityCard);
            this.panel1.Controls.Add(this.profileCard);
            this.panel1.Controls.Add(this.infoCard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 600);
            this.panel1.TabIndex = 5;
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
            this.infoCard.Controls.Add(this.lblEmployeeId);
            this.infoCard.Controls.Add(this.txtEmployeeId);
            this.infoCard.Controls.Add(this.lblRoleField);
            this.infoCard.Controls.Add(this.txtRole);
            this.infoCard.Controls.Add(this.lblDepartment);
            this.infoCard.Controls.Add(this.txtDepartment);
            this.infoCard.Controls.Add(this.lblJoinDate);
            this.infoCard.Controls.Add(this.txtJoinDate);
            this.infoCard.Controls.Add(this.line1);
            this.infoCard.Controls.Add(this.line2);
            this.infoCard.Controls.Add(this.line3);
            this.infoCard.Controls.Add(this.line4);
            this.infoCard.Location = new System.Drawing.Point(67, 261);
            this.infoCard.Name = "infoCard";
            this.infoCard.Size = new System.Drawing.Size(500, 419);
            this.infoCard.TabIndex = 2;
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
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFullName.ForeColor = System.Drawing.Color.Gray;
            this.lblFullName.Location = new System.Drawing.Point(18, 50);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(70, 19);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "Full Name";
            // 
            // lblEmailAddress
            // 
            this.lblEmailAddress.AutoSize = true;
            this.lblEmailAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailAddress.ForeColor = System.Drawing.Color.Gray;
            this.lblEmailAddress.Location = new System.Drawing.Point(18, 81);
            this.lblEmailAddress.Name = "lblEmailAddress";
            this.lblEmailAddress.Size = new System.Drawing.Size(94, 19);
            this.lblEmailAddress.TabIndex = 2;
            this.lblEmailAddress.Text = "Email Address";
            // 
            // lblContactNumber
            // 
            this.lblContactNumber.AutoSize = true;
            this.lblContactNumber.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblContactNumber.ForeColor = System.Drawing.Color.Gray;
            this.lblContactNumber.Location = new System.Drawing.Point(18, 123);
            this.lblContactNumber.Name = "lblContactNumber";
            this.lblContactNumber.Size = new System.Drawing.Size(111, 19);
            this.lblContactNumber.TabIndex = 3;
            this.lblContactNumber.Text = "Contact Number";
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAge.ForeColor = System.Drawing.Color.Gray;
            this.lblAge.Location = new System.Drawing.Point(18, 168);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(33, 19);
            this.lblAge.TabIndex = 4;
            this.lblAge.Text = "Age";
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGender.ForeColor = System.Drawing.Color.Gray;
            this.lblGender.Location = new System.Drawing.Point(18, 213);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(54, 19);
            this.lblGender.TabIndex = 5;
            this.lblGender.Text = "Gender";
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFullName.Location = new System.Drawing.Point(220, 47);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(220, 25);
            this.txtFullName.TabIndex = 6;
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.Location = new System.Drawing.Point(220, 78);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(220, 25);
            this.txtEmail.TabIndex = 7;
            // 
            // txtContact
            // 
            this.txtContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContact.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtContact.Location = new System.Drawing.Point(220, 120);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(220, 25);
            this.txtContact.TabIndex = 8;
            // 
            // txtAge
            // 
            this.txtAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAge.Location = new System.Drawing.Point(220, 165);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(220, 25);
            this.txtAge.TabIndex = 9;
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbGender.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Prefer not to say"});
            this.cmbGender.Location = new System.Drawing.Point(220, 210);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(220, 25);
            this.cmbGender.TabIndex = 10;
            // 
            // lblEmployeeId
            // 
            this.lblEmployeeId.AutoSize = true;
            this.lblEmployeeId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmployeeId.ForeColor = System.Drawing.Color.Gray;
            this.lblEmployeeId.Location = new System.Drawing.Point(18, 260);
            this.lblEmployeeId.Name = "lblEmployeeId";
            this.lblEmployeeId.Size = new System.Drawing.Size(89, 19);
            this.lblEmployeeId.TabIndex = 16;
            this.lblEmployeeId.Text = "Employee ID:";
            // 
            // txtEmployeeId
            // 
            this.txtEmployeeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmployeeId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmployeeId.Location = new System.Drawing.Point(220, 257);
            this.txtEmployeeId.Name = "txtEmployeeId";
            this.txtEmployeeId.ReadOnly = true;
            this.txtEmployeeId.Size = new System.Drawing.Size(220, 25);
            this.txtEmployeeId.TabIndex = 17;
            // 
            // lblRoleField
            // 
            this.lblRoleField.AutoSize = true;
            this.lblRoleField.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoleField.ForeColor = System.Drawing.Color.Gray;
            this.lblRoleField.Location = new System.Drawing.Point(18, 295);
            this.lblRoleField.Name = "lblRoleField";
            this.lblRoleField.Size = new System.Drawing.Size(38, 19);
            this.lblRoleField.TabIndex = 18;
            this.lblRoleField.Text = "Role:";
            // 
            // txtRole
            // 
            this.txtRole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRole.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRole.Location = new System.Drawing.Point(220, 292);
            this.txtRole.Name = "txtRole";
            this.txtRole.ReadOnly = true;
            this.txtRole.Size = new System.Drawing.Size(220, 25);
            this.txtRole.TabIndex = 19;
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDepartment.ForeColor = System.Drawing.Color.Gray;
            this.lblDepartment.Location = new System.Drawing.Point(18, 330);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(86, 19);
            this.lblDepartment.TabIndex = 20;
            this.lblDepartment.Text = "Department:";
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDepartment.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDepartment.Location = new System.Drawing.Point(220, 327);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(220, 25);
            this.txtDepartment.TabIndex = 21;
            // 
            // lblJoinDate
            // 
            this.lblJoinDate.AutoSize = true;
            this.lblJoinDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblJoinDate.ForeColor = System.Drawing.Color.Gray;
            this.lblJoinDate.Location = new System.Drawing.Point(18, 365);
            this.lblJoinDate.Name = "lblJoinDate";
            this.lblJoinDate.Size = new System.Drawing.Size(69, 19);
            this.lblJoinDate.TabIndex = 22;
            this.lblJoinDate.Text = "Join Date:";
            // 
            // txtJoinDate
            // 
            this.txtJoinDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJoinDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtJoinDate.Location = new System.Drawing.Point(220, 362);
            this.txtJoinDate.Name = "txtJoinDate";
            this.txtJoinDate.ReadOnly = true;
            this.txtJoinDate.Size = new System.Drawing.Size(220, 25);
            this.txtJoinDate.TabIndex = 23;
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line1.Location = new System.Drawing.Point(0, 105);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(500, 1);
            this.line1.TabIndex = 11;
            // 
            // line2
            // 
            this.line2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line2.Location = new System.Drawing.Point(0, 150);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(500, 1);
            this.line2.TabIndex = 12;
            // 
            // line3
            // 
            this.line3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line3.Location = new System.Drawing.Point(0, 195);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(500, 1);
            this.line3.TabIndex = 13;
            // 
            // line4
            // 
            this.line4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(181)))), ((int)(((byte)(175)))));
            this.line4.Location = new System.Drawing.Point(0, 240);
            this.line4.Name = "line4";
            this.line4.Size = new System.Drawing.Size(500, 1);
            this.line4.TabIndex = 14;
            // 
            // UserProfileForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(244)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(954, 600);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UserProfileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kapebara - Staff Profile";
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
    }
}