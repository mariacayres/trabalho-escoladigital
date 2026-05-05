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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Make Enter trigger the primary button if present
            if (this.button1 != null) this.AcceptButton = this.button1;

            // Ensure password textbox masks input if available
            if (this.textBox2passe != null) this.textBox2passe.UseSystemPasswordChar = true;

            // Defensive: ensure linkLabel1 is wired and clickable
            if (this.linkLabel1 != null)
            {
                this.linkLabel1.Links.Clear();
                this.linkLabel1.Links.Add(0, this.linkLabel1.Text.Length);
                this.linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
                this.linkLabel1.BringToFront();
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void Form1_Load_1(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void pictureBox3_Click(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var username = (textBoxname?.Text ?? string.Empty).Trim();
                var email = (textBox3email?.Text ?? string.Empty).Trim();
                var password = textBox2passe?.Text ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (username.Length < 3)
                {
                    MessageBox.Show("O nome deve ter pelo menos 3 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxname.Focus();
                    return;
                }

                if (password.Length < 6)
                {
                    MessageBox.Show("A senha deve ter pelo menos 6 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2passe.Focus();
                    return;
                }

                if (!email.Contains("@") || email.StartsWith("@") || email.EndsWith("@"))
                {
                    MessageBox.Show("Insira um email válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3email.Focus();
                    return;
                }

                // Prevent double submission UI
                button1.Enabled = false;
                button1.Cursor = Cursors.WaitCursor;

                // Open main interface (replace with real logic)
                var main = new interface_principal();
                main.StartPosition = FormStartPosition.CenterScreen;
                main.FormClosed += (s, args) =>
                {
                    try { this.Show(); }
                    finally
                    {
                        button1.Enabled = true;
                        button1.Cursor = Cursors.Hand;
                    }
                };

                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar a operação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
                button1.Cursor = Cursors.Hand;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var login = new Login();
                login.StartPosition = FormStartPosition.CenterScreen;
                login.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário de Login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
