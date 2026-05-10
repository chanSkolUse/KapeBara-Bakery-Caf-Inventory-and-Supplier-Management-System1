using System.Drawing;

namespace KapeBara_Inventory_Management_System
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pctrBxLogo = new System.Windows.Forms.PictureBox();
            this.pctrBxBackground = new System.Windows.Forms.PictureBox();
            this.lblQuote = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtBxEmail = new System.Windows.Forms.TextBox();
            this.txtBxPassword = new System.Windows.Forms.TextBox();
            this.lblForgotPassword = new System.Windows.Forms.Label();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lblNewAroundHere = new System.Windows.Forms.Label();
            this.lblSignUp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctrBxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctrBxBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // pctrBxLogo
            // 
            this.pctrBxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pctrBxLogo.Image")));
            this.pctrBxLogo.Location = new System.Drawing.Point(512, 12);
            this.pctrBxLogo.Name = "pctrBxLogo";
            this.pctrBxLogo.Size = new System.Drawing.Size(179, 145);
            this.pctrBxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctrBxLogo.TabIndex = 0;
            this.pctrBxLogo.TabStop = false;
            // 
            // pctrBxBackground
            // 
            this.pctrBxBackground.Image = ((System.Drawing.Image)(resources.GetObject("pctrBxBackground.Image")));
            this.pctrBxBackground.Location = new System.Drawing.Point(0, -1);
            this.pctrBxBackground.Name = "pctrBxBackground";
            this.pctrBxBackground.Size = new System.Drawing.Size(413, 453);
            this.pctrBxBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctrBxBackground.TabIndex = 1;
            this.pctrBxBackground.TabStop = false;
            // 
            // lblQuote
            // 
            this.lblQuote.AutoSize = true;
            this.lblQuote.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(68)))), ((int)(((byte)(60)))));
            this.lblQuote.Location = new System.Drawing.Point(459, 160);
            this.lblQuote.Name = "lblQuote";
            this.lblQuote.Size = new System.Drawing.Size(310, 16);
            this.lblQuote.TabIndex = 2;
            this.lblQuote.Text = "Relax Like a Capybara, Work Like a System";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblEmail.Location = new System.Drawing.Point(429, 193);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Email";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPassword.Location = new System.Drawing.Point(429, 233);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtBxEmail
            // 
            this.txtBxEmail.Location = new System.Drawing.Point(484, 193);
            this.txtBxEmail.Name = "txtBxEmail";
            this.txtBxEmail.Size = new System.Drawing.Size(264, 20);
            this.txtBxEmail.TabIndex = 5;
            // 
            // txtBxPassword
            // 
            this.txtBxPassword.Location = new System.Drawing.Point(484, 230);
            this.txtBxPassword.Name = "txtBxPassword";
            this.txtBxPassword.Size = new System.Drawing.Size(264, 20);
            this.txtBxPassword.TabIndex = 6;
            // 
            // lblForgotPassword
            // 
            this.lblForgotPassword.AutoSize = true;
            this.lblForgotPassword.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblForgotPassword.Location = new System.Drawing.Point(662, 253);
            this.lblForgotPassword.Name = "lblForgotPassword";
            this.lblForgotPassword.Size = new System.Drawing.Size(86, 13);
            this.lblForgotPassword.TabIndex = 7;
            this.lblForgotPassword.Text = "Forgot Password";
            // 
            // btnSignIn
            // 
            this.btnSignIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(71)))), ((int)(((byte)(70)))));
            this.btnSignIn.ForeColor = System.Drawing.Color.White;
            this.btnSignIn.Location = new System.Drawing.Point(533, 308);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(123, 47);
            this.btnSignIn.TabIndex = 8;
            this.btnSignIn.Text = "Sign In";
            this.btnSignIn.UseVisualStyleBackColor = false;
            // 
            // lblNewAroundHere
            // 
            this.lblNewAroundHere.AutoSize = true;
            this.lblNewAroundHere.BackColor = System.Drawing.Color.Transparent;
            this.lblNewAroundHere.Location = new System.Drawing.Point(500, 367);
            this.lblNewAroundHere.Name = "lblNewAroundHere";
            this.lblNewAroundHere.Size = new System.Drawing.Size(95, 13);
            this.lblNewAroundHere.TabIndex = 9;
            this.lblNewAroundHere.Text = "New around here?";
            // 
            // lblSignUp
            // 
            this.lblSignUp.AutoSize = true;
            this.lblSignUp.BackColor = System.Drawing.Color.Transparent;
            this.lblSignUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(70)))), ((int)(((byte)(39)))));
            this.lblSignUp.Location = new System.Drawing.Point(598, 367);
            this.lblSignUp.Name = "lblSignUp";
            this.lblSignUp.Size = new System.Drawing.Size(118, 13);
            this.lblSignUp.TabIndex = 10;
            this.lblSignUp.Text = "Create an account now";
            this.lblSignUp.Click += new System.EventHandler(this.lblSignUp_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblSignUp);
            this.Controls.Add(this.lblNewAroundHere);
            this.Controls.Add(this.btnSignIn);
            this.Controls.Add(this.lblForgotPassword);
            this.Controls.Add(this.txtBxPassword);
            this.Controls.Add(this.txtBxEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblQuote);
            this.Controls.Add(this.pctrBxBackground);
            this.Controls.Add(this.pctrBxLogo);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pctrBxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctrBxBackground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctrBxLogo;
        private System.Windows.Forms.PictureBox pctrBxBackground;
        private System.Windows.Forms.Label lblQuote;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtBxEmail;
        private System.Windows.Forms.TextBox txtBxPassword;
        private System.Windows.Forms.Label lblForgotPassword;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label lblNewAroundHere;
        private System.Windows.Forms.Label lblSignUp;
    }
}

