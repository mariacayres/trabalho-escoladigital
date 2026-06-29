using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private Button btnOlho;

        public Form1()
        {
            InitializeComponent();

            if (this.button1 != null) this.AcceptButton = this.button1;

            if (this.textBox2passe != null)
            {
                this.textBox2passe.UseSystemPasswordChar = true;
                this.textBox2passe.PasswordChar = '●';
            }

            ConfigurarBotaoOlho();

            if (this.linkLabel1 != null)
            {
                this.linkLabel1.Links.Clear();
                this.linkLabel1.Links.Add(0, this.linkLabel1.Text.Length);
                this.linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
                this.linkLabel1.BringToFront();
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked_1;
                this.linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            }
        }

        private void ConfigurarBotaoOlho()
        {
            btnOlho = new Button();
            btnOlho.Text = "👁";
            btnOlho.Location = new Point(textBox2passe.Right + 4, textBox2passe.Top);
            btnOlho.Size = new Size(30, textBox2passe.Height);
            btnOlho.FlatStyle = FlatStyle.Flat;
            btnOlho.Cursor = Cursors.Hand;
            btnOlho.Font = new Font("Segoe UI", 10f);
            btnOlho.FlatAppearance.BorderSize = 0;

            btnOlho.Click += (sender, args) =>
            {
                if (textBox2passe.PasswordChar == '●')
                {
                    textBox2passe.PasswordChar = '\0';
                    btnOlho.Text = "🙈";
                }
                else
                {
                    textBox2passe.PasswordChar = '●';
                    btnOlho.Text = "👁";
                }
            };

            this.Controls.Add(btnOlho);
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
        private void textBox2passe_TextChanged(object sender, EventArgs e) { }
        private void textBox3email_TextChanged(object sender, EventArgs e) { }
        private void textBoxname_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var username = (textBoxname?.Text ?? string.Empty).Trim();
                var email = (textBox3email?.Text ?? string.Empty).Trim();
                var password = textBox2passe?.Text ?? string.Empty;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
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

                button1.Enabled = false;
                button1.Cursor = Cursors.WaitCursor;
                button1.Text = "A criar conta...";

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (var checkCmd = new SqlCommand("SELECT COUNT(1) FROM utilizadorTbl WHERE gmail = @gmail", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@gmail", email);
                        var exists = Convert.ToInt32(checkCmd.ExecuteScalar() ?? 0) > 0;
                        if (exists)
                        {
                            MessageBox.Show("Email já cadastrado. Use outro email ou faça login.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBox3email.Focus();
                            textBox3email.SelectAll();
                            button1.Enabled = true;
                            button1.Cursor = Cursors.Hand;
                            button1.Text = "Criar conta";
                            return;
                        }
                    }

                    bool isAdmin = (email == "admin@admin.com" && password == "admin123");

                    using (var insertCmd = new SqlCommand(
                        "INSERT INTO utilizadorTbl (nome, gmail, senha, dinheiro, is_admin) VALUES (@nome, @gmail, @senha, @dinheiro, @isAdmin); SELECT SCOPE_IDENTITY();", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@nome", username);
                        insertCmd.Parameters.AddWithValue("@gmail", email);
                        insertCmd.Parameters.AddWithValue("@senha", password);
                        insertCmd.Parameters.AddWithValue("@dinheiro", 0.00);
                        insertCmd.Parameters.AddWithValue("@isAdmin", isAdmin ? 1 : 0);

                        int newId = Convert.ToInt32(insertCmd.ExecuteScalar());

                        Session.LoggedUserId = newId;
                        Session.LoggedUserName = username;
                        Session.LoggedUserEmail = email;
                        Session.IsAdmin = isAdmin;
                    }
                }

                MessageBox.Show("Conta criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (Session.IsAdmin)
                {
                    var adminForm = new Form2();
                    adminForm.StartPosition = FormStartPosition.CenterScreen;
                    adminForm.FormClosed += (s, args) =>
                    {
                        try { this.Show(); }
                        finally
                        {
                            button1.Enabled = true;
                            button1.Cursor = Cursors.Hand;
                            button1.Text = "Criar conta";
                        }
                    };
                    adminForm.Show();
                    this.Hide();
                }
                else
                {
                    var main = new interface_principal();
                    main.StartPosition = FormStartPosition.CenterScreen;
                    main.FormClosed += (s, args) =>
                    {
                        try { this.Show(); }
                        finally
                        {
                            button1.Enabled = true;
                            button1.Cursor = Cursors.Hand;
                            button1.Text = "Criar conta";
                        }
                    };
                    main.Show();
                    this.Hide();
                }
                CloseExistingLoginForms();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Erro de banco de dados: " + sqlEx.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    button1.Enabled = true;
                    button1.Cursor = Cursors.Hand;
                    button1.Text = "Criar conta";
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar a operação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    button1.Enabled = true;
                    button1.Cursor = Cursors.Hand;
                    button1.Text = "Criar conta";
                }
                catch { }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLogin();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLogin();
        }

        private void CloseExistingLoginForms()
        {
            try
            {
                var openLogins = Application.OpenForms
                    .Cast<Form>()
                    .Where(f => f.GetType().Name.Equals("Login", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var f in openLogins)
                {
                    try { f.Close(); }
                    catch { }
                }
            }
            catch { }
        }

        private void OpenLogin()
        {
            try
            {
                CloseExistingLoginForms();
                var login = new login();
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

        private void LimparCampos()
        {
            textBoxname.Clear();
            textBox3email.Clear();
            textBox2passe.Clear();
            textBoxname.Focus();
        }

        private bool UserExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM utilizadorTbl WHERE gmail = @email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}