using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class login : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";

        public login()
        {
            InitializeComponent();

            // Ensure password boxes are masked (there are two sets on the designer)
            if (this.textBox2passe != null) this.textBox2passe.UseSystemPasswordChar = true;
            if (this.textBox1 != null) this.textBox1.UseSystemPasswordChar = true;

            // Defensive event wiring: button2 is "Entrar" (login) in the Designer.
            if (this.button2 != null)
            {
                this.button2.Click -= button2_Click_Login;
                this.button2.Click += button2_Click_Login;
                this.button2.Cursor = Cursors.Hand;
            }

            // button1 is "Criar conta" — open registration Form1
            if (this.button1 != null)
            {
                this.button1.Click -= button1_Click_OpenRegister;
                this.button1.Click += button1_Click_OpenRegister;
                this.button1.Cursor = Cursors.Hand;
            }

            // linkLabel1 (if present) already wired in Designer, keep defensive wiring
            if (this.linkLabel1 != null)
            {
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            }
        }

        // Entrar (uses the controls placed for the login view: textBox2 = email, textBox1 = senha)
        private void button2_Click_Login(object sender, EventArgs e)
        {
            try
            {
                var email = (this.textBox2?.Text ?? string.Empty).Trim();   // Designer: textBox2 (Email) for login
                var senha = (this.textBox1?.Text ?? string.Empty);           // Designer: textBox1 (Senha) for login

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Special-case admin credentials (no DB lookup)
                if (string.Equals(email, "admin@admin.com", StringComparison.OrdinalIgnoreCase) &&
                    senha == "admin123")
                {
                    var adminForm = new Form2();
                    adminForm.StartPosition = FormStartPosition.CenterScreen;
                    adminForm.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                    adminForm.Show();
                    this.Hide();
                    return;
                }

                // Normal user: check DB for gmail+senha
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    const string query = "SELECT id_cliente, nome, is_admin FROM utilizadorTbl WHERE gmail = @email AND senha = @senha";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@senha", senha);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate session if present
                                try
                                {
                                    Session.LoggedUserId = reader.GetInt32(0);
                                    Session.LoggedUserName = reader.GetString(1);
                                    Session.LoggedUserEmail = email;
                                    Session.IsAdmin = reader.GetInt32(2) == 1;
                                }
                                catch { /* ignore if Session class not present */ }

                                var mainForm = new interface_principal();
                                mainForm.StartPosition = FormStartPosition.CenterScreen;
                                mainForm.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                                mainForm.Show();
                                this.Hide();
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show("Email ou senha incorretos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fazer login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Criar conta — open registration form (Form1) using the registration controls present on the same designer
        private void button1_Click_OpenRegister(object sender, EventArgs e)
        {
            try
            {
                var register = new Form1();
                register.StartPosition = FormStartPosition.CenterScreen;
                register.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                register.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Keep designer-wired stubs (no-op) if still referenced by Designer
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox2passe_TextChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void textBox3email_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBoxname_TextChanged(object sender, EventArgs e) { }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { OpenRegisterViaLink(); }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) { OpenRegisterViaLink(); }

        private void OpenRegisterViaLink()
        {
            try
            {
                var register = new Form1();
                register.StartPosition = FormStartPosition.CenterScreen;
                register.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                register.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        // Add this method to handle the button1 Click event (for "Criar conta" button)
        private void button1_Click(object sender, EventArgs e)
        {
            // Call the method to open the register form or registration logic
            button1_Click_OpenRegister(sender, e);
        }
    }
}