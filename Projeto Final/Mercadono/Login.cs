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

            // Ensure password boxes are masked
            if (this.textBox2passe != null) this.textBox2passe.UseSystemPasswordChar = true;
            if (this.textBox1 != null) this.textBox1.UseSystemPasswordChar = true;

            // button2 is "Entrar" (login)
            if (this.button2 != null)
            {
                this.button2.Click -= button2_Click_Login;
                this.button2.Click += button2_Click_Login;
                this.button2.Cursor = Cursors.Hand;
            }

            // button1 is "Criar conta"
            if (this.button1 != null)
            {
                this.button1.Click -= button1_Click_OpenRegister;
                this.button1.Click += button1_Click_OpenRegister;
                this.button1.Cursor = Cursors.Hand;
            }

            if (this.linkLabel1 != null)
            {
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            }
        }

        // ============================================================
        // LOGIN - Entrar
        // ============================================================
        private void button2_Click_Login(object sender, EventArgs e)
        {
            try
            {
                var email = (this.textBox2?.Text ?? string.Empty).Trim();
                var senha = (this.textBox1?.Text ?? string.Empty);

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ============================================================
                // VERIFICAR ADMIN PRIMEIRO (antes da base de dados)
                // ============================================================
                if (email.ToLower() == "admin@admin.com" && senha == "admin123")
                {
                    // Criar sessão admin
                    Session.LoggedUserId = 1;
                    Session.LoggedUserName = "Administrador";
                    Session.LoggedUserEmail = email;
                    Session.IsAdmin = true;

                    var adminForm = new Form2();
                    adminForm.StartPosition = FormStartPosition.CenterScreen;
                    adminForm.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                    adminForm.Show();
                    this.Hide();
                    return;
                }

                // ============================================================
                // VERIFICAR NA BASE DE DADOS (usuários normais)
                // ============================================================
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
                                Session.LoggedUserId = reader.GetInt32(0);
                                Session.LoggedUserName = reader.GetString(1);
                                Session.LoggedUserEmail = email;
                                Session.IsAdmin = reader.GetInt32(2) == 1;

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

        // ============================================================
        // CRIAR CONTA - Abrir Form1
        // ============================================================
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

        // ============================================================
        // STUBS DO DESIGNER
        // ============================================================
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox2passe_TextChanged(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void textBox3email_TextChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBoxname_TextChanged(object sender, EventArgs e) { }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { OpenRegisterViaLink(); }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e) { OpenRegisterViaLink(); }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }

        // Este método está duplicado? Se tiver dois button1_Click, apaga um!
        private void button1_Click(object sender, EventArgs e)
        {
            button1_Click_OpenRegister(sender, e);
        }
    }
}