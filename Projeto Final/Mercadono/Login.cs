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
        private Button btnOlho; // Botão para mostrar/ocultar senha

        public login()
        {
            InitializeComponent();

            // Configurar a senha
            if (this.textBox2passe != null)
            {
                this.textBox2passe.UseSystemPasswordChar = true;
                this.textBox2passe.PasswordChar = '●';
            }

            // Configurar o botão olho
            ConfigurarBotaoOlho();

            // Configurar o linkLabel1
            if (this.linkLabel1 != null)
            {
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked_1;
                this.linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            }
        }

        private void ConfigurarBotaoOlho()
        {
            // Criar o botão olho
            btnOlho = new Button();
            btnOlho.Text = "👁";
            btnOlho.Location = new Point(textBox2passe.Right + 4, textBox2passe.Top);
            btnOlho.Size = new Size(30, textBox2passe.Height);
            btnOlho.FlatStyle = FlatStyle.Flat;
            btnOlho.Cursor = Cursors.Hand;
            btnOlho.Font = new Font("Segoe UI", 10f);
            btnOlho.FlatAppearance.BorderSize = 0;

            // Evento para mostrar/ocultar senha
            btnOlho.Click += (sender, args) =>
            {
                if (textBox2passe.PasswordChar == '●')
                {
                    textBox2passe.PasswordChar = '\0'; // Mostrar senha
                    btnOlho.Text = "🙈"; // Ícone olho fechado
                }
                else
                {
                    textBox2passe.PasswordChar = '●'; // Ocultar senha
                    btnOlho.Text = "👁"; // Ícone olho aberto
                }
            };

            this.Controls.Add(btnOlho);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string email = textBox3email.Text.Trim();
                string senha = textBox2passe.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id_cliente, nome, is_admin FROM utilizadorTbl WHERE gmail = @email AND senha = @senha";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@senha", senha);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session.LoggedUserId = reader.GetInt32(0);
                                Session.LoggedUserName = reader.GetString(1);
                                Session.LoggedUserEmail = email;
                                Session.IsAdmin = reader.GetInt32(2) == 1;

                                if (Session.IsAdmin)
                                {
                                    Form2 adminForm = new Form2();
                                    adminForm.StartPosition = FormStartPosition.CenterScreen;
                                    adminForm.FormClosed += (s, args) => { Application.Exit(); };
                                    adminForm.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    interface_principal mainForm = new interface_principal();
                                    mainForm.StartPosition = FormStartPosition.CenterScreen;
                                    mainForm.FormClosed += (s, args) => { Application.Exit(); };
                                    mainForm.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Email ou senha incorretos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fazer login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2passe_TextChanged(object sender, EventArgs e)
        {
            // Este método está vazio porque o botão olho já foi configurado no construtor
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3email_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxname_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLogin();
        }

        // Some designer files reference this name; keep it to avoid missing-method errors.
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLogin();
        }

        private void OpenLogin()
        {
            try
            {
                var form1 = new Form1();
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                form1.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário de Login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}