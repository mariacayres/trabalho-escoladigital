using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            if (this.btnLogin != null) this.AcceptButton = this.btnLogin;
            if (this.txtPassword != null) this.txtPassword.UseSystemPasswordChar = true;

            // Defensive: ensure link is a real link, visible on top and wired
            if (this.linkBack != null)
            {
                this.linkBack.Links.Clear();
                this.linkBack.Links.Add(0, this.linkBack.Text.Length);
                this.linkBack.LinkBehavior = LinkBehavior.HoverUnderline;
                this.linkBack.Enabled = true;
                this.linkBack.TabStop = true;
                this.linkBack.BringToFront();
                // re-wire in case designer wiring was lost
                this.linkBack.LinkClicked -= linkBack_LinkClicked;
                this.linkBack.LinkClicked += linkBack_LinkClicked;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = (txtUsername?.Text ?? string.Empty).Trim();
            var password = txtPassword?.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("O nome deve ter pelo menos 3 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("A senha deve ter pelo menos 6 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            var main = new interface_principal();
            main.StartPosition = FormStartPosition.CenterScreen;
            main.FormClosed += (s, args) => { try { this.Show(); } catch { } };
            main.Show();
            this.Hide();
        }

        private void linkBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        // keep stubs to avoid designer missing-method errors
        private void textBox2passe_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
