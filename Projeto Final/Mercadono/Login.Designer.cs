
namespace Mercadono
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel linkBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.linkBack = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1 - background
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Mercadono.Properties.Resources.Captura_de_ecrã_2026_04_28_100734;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 450);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Stencil", 36F);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(320, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 57);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "LOGIN";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.BackColor = System.Drawing.SystemColors.Control;
            this.lblUser.Location = new System.Drawing.Point(220, 160);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(38, 13);
            this.lblUser.TabIndex = 2;
            this.lblUser.Text = "Nome";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(279, 157);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(240, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.BackColor = System.Drawing.SystemColors.Control;
            this.lblPass.Location = new System.Drawing.Point(220, 200);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(38, 13);
            this.lblPass.TabIndex = 4;
            this.lblPass.Text = "Passe";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(279, 197);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(240, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(319, 240);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(160, 28);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Entrar";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // linkBack
            // 
            this.linkBack.AutoSize = true;
            this.linkBack.Location = new System.Drawing.Point(373, 280);
            this.linkBack.Name = "linkBack";
            this.linkBack.Size = new System.Drawing.Size(36, 13);
            this.linkBack.TabIndex = 7;
            this.linkBack.TabStop = true;
            this.linkBack.Text = "Voltar";
            this.linkBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack_LinkClicked);
            // 
            // Login (form)
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            // add background first
            this.Controls.Add(this.pictureBox1);
            // then controls (so they appear on top)
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.linkBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 450);
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
