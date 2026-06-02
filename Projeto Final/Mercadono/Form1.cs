using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Mercadono
{
    public partial class Login : Form
    {
        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Mercadono;Integrated Security=True;";

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id_cliente, nome, is_admin FROM utilizadorTbl WHERE gmail = @email AND senha = @senha";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@senha", password);

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
                                    adminForm.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    interface_principal mainForm = new interface_principal();
                                    mainForm.StartPosition = FormStartPosition.CenterScreen;
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

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 registerForm = new Form1();
            registerForm.Show();
            this.Hide();
        }
    }
    public partial class interface_principal : Form
    {
        // Constructor: ensure designer controls are initialized
        public interface_principal()
        {
            InitializeComponent();

            // Wire button1 to open ajuda_ao_cliente if the control exists.
            if (this.button1 != null)
            {
                // Prevent double-subscription
                this.button1.Click -= Button1_OpenAjuda_Click;
                this.button1.Click += Button1_OpenAjuda_Click;
                this.button1.Cursor = Cursors.Hand;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.pictureBox1 == null) return;

                // If an image exists, size the form and picture box to the image (clamped to working area).
                if (this.pictureBox1.Image != null)
                {
                    var imgSize = this.pictureBox1.Image.Size;
                    var wa = Screen.FromControl(this).WorkingArea;
                    var target = new Size(
                        Math.Min(imgSize.Width, wa.Width),
                        Math.Min(imgSize.Height, wa.Height)
                    );

                    // Ensure picture box is placed at the client origin and displays the image at native size
                    this.pictureBox1.Location = new Point(0, 0);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    this.pictureBox1.Size = target;

                    // Make the form client area match the image size
                    this.ClientSize = target;
                }
                else
                {
                    // No image: ensure picture is aligned and form matches the control size
                    this.pictureBox1.Location = Point.Empty;
                    this.ClientSize = this.pictureBox1.Size;
                }
            }
            catch
            {
                // Fail silently to avoid preventing the form from showing.
            }
        }

        private void Button1_OpenAjuda_Click(object sender, EventArgs e)
        {
            try
            {
                var ajudaForm = new ajuda_ao_cliente();
                ajudaForm.StartPosition = FormStartPosition.CenterScreen;
                // When ajuda form closes, show this form again
                ajudaForm.FormClosed -= AjudaForm_FormClosed;
                ajudaForm.FormClosed += AjudaForm_FormClosed;
                ajudaForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Ajuda ao Cliente: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AjudaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { this.Show(); } catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: custom click behavior for the picture.
            // Example: close on Ctrl+click:
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                this.Close();
            }
        }
    }
}