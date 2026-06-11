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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Mercadono
{
    public partial class Form1 : Form
    {
        // Update this connection string if your server/database differ
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";

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

                // Wire both possible handler names used in designer to be safe
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked;
                this.linkLabel1.LinkClicked -= linkLabel1_LinkClicked_1;
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

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check existing email
                    using (var checkCmd = new SqlCommand("SELECT COUNT(1) FROM utilizadorTbl WHERE gmail = @gmail", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@gmail", email);
                        var exists = Convert.ToInt32(checkCmd.ExecuteScalar() ?? 0) > 0;
                        if (exists)
                        {
                            MessageBox.Show("Email já cadastrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBox3email.Focus();
                            button1.Enabled = true;
                            button1.Cursor = Cursors.Hand;
                            return;
                        }
                    }

                    // Insert new user (adjust columns if your schema differs)
                    using (var insertCmd = new SqlCommand(
                        "INSERT INTO utilizadorTbl (nome, gmail, senha, is_admin) VALUES (@nome, @gmail, @senha, 0)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@nome", username);
                        insertCmd.Parameters.AddWithValue("@gmail", email);
                        insertCmd.Parameters.AddWithValue("@senha", password); // consider hashing in production
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Conta criada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open main interface
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
                CloseExistingLoginForms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar a operação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { button1.Enabled = true; button1.Cursor = Cursors.Hand; } catch { }
            }
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

        /// <summary>
        /// Close any existing login form instances to avoid duplicates.
        /// </summary>
        private void CloseExistingLoginForms()
        {
            try
            {
                // Find any open forms whose runtime type name is "login" and close them.
                var openLogins = Application.OpenForms
                    .Cast<Form>()
                    .Where(f => f.GetType().Name.Equals("login", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var f in openLogins)
                {
                    try { f.Close(); }
                    catch { /* ignore individual close errors */ }
                }
            }
            catch
            {
                // Swallow exceptions to avoid UI disruption
            }
        }

        private void OpenLogin()
        {
            try
            {
                // Ensure previous login windows are closed before opening a new one
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
            // Closed existing login forms before creating a new one to prevent duplicates.
        }

        private void textBox2passe_TextChanged(object sender, EventArgs e)

        {

            textBox2passe.PasswordChar = '●';

            textBox2passe btnOlho = new textBox2passe

            {

                Text = "👁",

                Location = new Point(textBox2passe.Right + 4, textBox2passe.Top),

                Size = new Size(30, textBox2passe.Height),

                FlatStyle = FlatStyle.Flat,

                Cursor = Cursors.Hand,

                Font = new Font("Segoe UI", 10f),

            };

            btnOlho.FlatAppearance.BorderSize = 0;

            btnOlho.Click += (s, e) =>

            {

                textBox2passe.PasswordChar = textBox2passe.PasswordChar == '\0' ? '●' : '\0';

            };

            this.Controls.Add(btnOlho);

        }

    }
}
