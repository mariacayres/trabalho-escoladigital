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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // reserved for designer wiring
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // runtime initialization if needed
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // optional: image click handler
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // optional: label click handler
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // optional
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // optional: password changed
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // optional: username changed
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // optional
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // basic input validation and open main interface
            try
            {
                var username = (textBoxname?.Text ?? string.Empty).Trim();
                var email = (textBox3email?.Text ?? string.Empty).Trim();
                var password = textBox2passe?.Text ?? string.Empty;

                // All fields required
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Username length
                if (username.Length < 3)
                {
                    MessageBox.Show("O nome deve ter pelo menos 3 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxname.Focus();
                    return;
                }

                // Password length
                if (password.Length < 6)
                {
                    MessageBox.Show("A senha deve ter pelo menos 6 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2passe.Focus();
                    return;
                }

                // Basic email sanity check
                if (!email.Contains("@") || email.StartsWith("@") || email.EndsWith("@"))
                {
                    MessageBox.Show("Insira um email válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3email.Focus();
                    return;
                }

                // Prevent double submission
                button1.Enabled = false;
                button1.Cursor = Cursors.WaitCursor;

                // TODO: Replace with real authentication/registration logic.
                // For now, open the main interface.
                var main = new interface_principal();
                main.StartPosition = FormStartPosition.CenterScreen;

                // When main closes, show this login form again and re-enable the button
                main.FormClosed += (s, args) =>
                {
                    try
                    {
                        this.Show();
                    }
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var login = new Login();
                login.StartPosition = FormStartPosition.CenterScreen;
                login.FormClosed += (s, args) =>
                {
                    try { this.Show(); } catch { }
                };
                login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário de Login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
