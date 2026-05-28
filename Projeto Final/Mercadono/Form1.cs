using System;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Mercadono
{
    public partial class interface_principal : Form
    {
        public interface_principal()
        {
            InitializeComponent();
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            this.Text = "Mercadono - Interface Principal";
            this.Size = new System.Drawing.Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            Label lblWelcome = new Label();
            lblWelcome.Text = $"Bem-vindo, {Session.LoggedUserName}!";
            lblWelcome.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
            lblWelcome.ForeColor = System.Drawing.Color.DarkGreen;
            lblWelcome.Location = new System.Drawing.Point(50, 50);
            lblWelcome.Size = new System.Drawing.Size(700, 40);
            this.Controls.Add(lblWelcome);

            Label lblInfo = new Label();
            lblInfo.Text = $"Email: {Session.LoggedUserEmail}\nID: {Session.LoggedUserId}";
            lblInfo.Font = new System.Drawing.Font("Arial", 12);
            lblInfo.Location = new System.Drawing.Point(50, 100);
            lblInfo.Size = new System.Drawing.Size(700, 60);
            this.Controls.Add(lblInfo);

            Button btnProducts = new Button();
            btnProducts.Text = "Ver Produtos";
            btnProducts.Font = new System.Drawing.Font("Arial", 12);
            btnProducts.Size = new System.Drawing.Size(200, 50);
            btnProducts.Location = new System.Drawing.Point(50, 200);
            btnProducts.BackColor = System.Drawing.Color.LightBlue;
            this.Controls.Add(btnProducts);

            Button btnCart = new Button();
            btnCart.Text = "Meu Carrinho";
            btnCart.Font = new System.Drawing.Font("Arial", 12);
            btnCart.Size = new System.Drawing.Size(200, 50);
            btnCart.Location = new System.Drawing.Point(280, 200);
            btnCart.BackColor = System.Drawing.Color.LightYellow;
            this.Controls.Add(btnCart);

            Button btnHistory = new Button();
            btnHistory.Text = "Minhas Compras";
            btnHistory.Font = new System.Drawing.Font("Arial", 12);
            btnHistory.Size = new System.Drawing.Size(200, 50);
            btnHistory.Location = new System.Drawing.Point(510, 200);
            btnHistory.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(btnHistory);

            Button btnComplaint = new Button();
            btnComplaint.Text = "Ajuda / Reclamação";
            btnComplaint.Font = new System.Drawing.Font("Arial", 12);
            btnComplaint.Size = new System.Drawing.Size(200, 50);
            btnComplaint.Location = new System.Drawing.Point(50, 280);
            btnComplaint.BackColor = System.Drawing.Color.LightCoral;
            this.Controls.Add(btnComplaint);

            Button btnLogout = new Button();
            btnLogout.Text = "Sair";
            btnLogout.Font = new System.Drawing.Font("Arial", 12);
            btnLogout.Size = new System.Drawing.Size(200, 50);
            btnLogout.Location = new System.Drawing.Point(280, 280);
            btnLogout.BackColor = System.Drawing.Color.Red;
            btnLogout.ForeColor = System.Drawing.Color.White;
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja realmente sair?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }
    }
}