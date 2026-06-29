using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class utilizadores : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private int selectedUserId = 0;

        public utilizadores()
        {
            InitializeComponent();
            CarregarUtilizadores();
            ConfigurarBotoes();
        }

        private void ConfigurarBotoes()
        {
            if (this.button1 != null)
            {
                this.button1.Click -= button1_Click;
                this.button1.Click += button1_Click;
                this.button1.Text = "PRODUTOS";
            }

            if (this.button2 != null)
            {
                this.button2.Click -= button2_Click;
                this.button2.Click += button2_Click;
                this.button2.Text = "COMPRAS";
            }

            if (this.button3 != null)
            {
                this.button3.Click -= button3_Click;
                this.button3.Click += button3_Click;
                this.button3.Text = "ESTOQUE";
            }

            if (this.button4 != null)
            {
                this.button4.Click -= button4_Click;
                this.button4.Click += button4_Click;
                this.button4.Text = "ATUALIZAR";
            }

            if (this.button5 != null)
            {
                this.button5.Click -= button5_Click;
                this.button5.Click += button5_Click;
                this.button5.Text = "REMOVER";
            }

            if (this.button6 != null)
            {
                this.button6.Click -= button6_Click;
                this.button6.Click += button6_Click;
                this.button6.Text = "ELIMINAR";
                this.button6.BackColor = System.Drawing.Color.Red;
                this.button6.ForeColor = System.Drawing.Color.White;
            }

            if (this.button7 != null)
            {
                this.button7.Click -= button7_Click;
                this.button7.Click += button7_Click;
                this.button7.Text = "CRIAR";
                this.button7.BackColor = System.Drawing.Color.Green;
                this.button7.ForeColor = System.Drawing.Color.White;
            }

            if (this.button8 != null)
            {
                this.button8.Click -= button8_Click;
                this.button8.Click += button8_Click;
                this.button8.Text = "EDITAR";
                this.button8.BackColor = System.Drawing.Color.Orange;
                this.button8.ForeColor = System.Drawing.Color.White;
            }

            if (this.dataGridView1 != null)
            {
                this.dataGridView1.CellClick += DataGridView1_CellClick;
            }
        }

        // ============================================================
        // CARREGAR UTILIZADORES
        // ============================================================
        private void CarregarUtilizadores()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            id_cliente AS 'ID',
                            nome AS 'Nome',
                            gmail AS 'Email',
                            dinheiro AS 'Saldo',
                            CASE WHEN is_admin = 1 THEN 'Sim' ELSE 'Não' END AS 'Admin'
                        FROM utilizadorTbl
                        ORDER BY nome";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns["Saldo"] != null)
                    {
                        dataGridView1.Columns["Saldo"].DefaultCellStyle.Format = "C2";
                        dataGridView1.Columns["Saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dataGridView1.Columns["ID"] != null)
                    {
                        dataGridView1.Columns["ID"].Width = 60;
                        dataGridView1.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridView1.Columns["Admin"] != null)
                    {
                        dataGridView1.Columns["Admin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    dataGridView1.ClearSelection();
                    selectedUserId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar utilizadores: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CLICK
        // ============================================================
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["ID"].Value);
            }
        }

        // ============================================================
        // BOTÃO 8 - EDITAR UTILIZADOR
        // ============================================================
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserId == 0)
                {
                    MessageBox.Show("Selecione um utilizador na lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nomeAtual = "";
                string emailAtual = "";
                string adminAtual = "";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() == selectedUserId.ToString())
                    {
                        nomeAtual = row.Cells["Nome"].Value?.ToString() ?? "";
                        emailAtual = row.Cells["Email"].Value?.ToString() ?? "";
                        adminAtual = row.Cells["Admin"].Value?.ToString() ?? "";
                        break;
                    }
                }

                Form formEditar = new Form();
                formEditar.Text = "Editar Utilizador";
                formEditar.Size = new System.Drawing.Size(400, 300);
                formEditar.StartPosition = FormStartPosition.CenterScreen;
                formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formEditar.MaximizeBox = false;
                formEditar.MinimizeBox = false;

                Label lblNome = new Label() { Text = "Nome:", Location = new System.Drawing.Point(20, 30), Size = new System.Drawing.Size(80, 25) };
                TextBox txtNome = new TextBox() { Location = new System.Drawing.Point(110, 30), Size = new System.Drawing.Size(250, 25), Text = nomeAtual };

                Label lblEmail = new Label() { Text = "Email:", Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(80, 25) };
                TextBox txtEmail = new TextBox() { Location = new System.Drawing.Point(110, 70), Size = new System.Drawing.Size(250, 25), Text = emailAtual };

                Label lblSenha = new Label() { Text = "Nova Senha:", Location = new System.Drawing.Point(20, 110), Size = new System.Drawing.Size(80, 25) };
                TextBox txtSenha = new TextBox() { Location = new System.Drawing.Point(110, 110), Size = new System.Drawing.Size(250, 25), Text = "", UseSystemPasswordChar = true };
                Label lblSenhaInfo = new Label() { Text = "(deixar em branco para manter)", Location = new System.Drawing.Point(110, 135), Size = new System.Drawing.Size(200, 20), Font = new System.Drawing.Font("Arial", 8) };

                Label lblAdmin = new Label() { Text = "Admin:", Location = new System.Drawing.Point(20, 160), Size = new System.Drawing.Size(80, 25) };
                ComboBox cbAdmin = new ComboBox() { Location = new System.Drawing.Point(110, 160), Size = new System.Drawing.Size(100, 25), DropDownStyle = ComboBoxStyle.DropDownList };
                cbAdmin.Items.Add("Não");
                cbAdmin.Items.Add("Sim");
                cbAdmin.SelectedIndex = adminAtual == "Sim" ? 1 : 0;

                Button btnSalvar = new Button() { Text = "SALVAR", Location = new System.Drawing.Point(110, 210), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGreen };
                Button btnCancelar = new Button() { Text = "CANCELAR", Location = new System.Drawing.Point(230, 210), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGray };

                btnSalvar.Click += (s, ev) =>
                {
                    string novoNome = txtNome.Text.Trim();
                    string novoEmail = txtEmail.Text.Trim();
                    string novaSenha = txtSenha.Text;
                    int isAdmin = cbAdmin.SelectedIndex;

                    if (string.IsNullOrWhiteSpace(novoNome) || string.IsNullOrWhiteSpace(novoEmail))
                    {
                        MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query;
                        SqlCommand cmd;

                        if (string.IsNullOrWhiteSpace(novaSenha))
                        {
                            query = @"UPDATE utilizadorTbl SET nome = @nome, gmail = @email, is_admin = @isAdmin WHERE id_cliente = @id";
                            cmd = new SqlCommand(query, conn);
                        }
                        else
                        {
                            query = @"UPDATE utilizadorTbl SET nome = @nome, gmail = @email, senha = @senha, is_admin = @isAdmin WHERE id_cliente = @id";
                            cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@senha", novaSenha);
                        }

                        cmd.Parameters.AddWithValue("@id", selectedUserId);
                        cmd.Parameters.AddWithValue("@nome", novoNome);
                        cmd.Parameters.AddWithValue("@email", novoEmail);
                        cmd.Parameters.AddWithValue("@isAdmin", isAdmin);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Utilizador atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarUtilizadores();
                            formEditar.Close();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao atualizar utilizador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                btnCancelar.Click += (s, ev) => formEditar.Close();

                formEditar.Controls.Add(lblNome);
                formEditar.Controls.Add(txtNome);
                formEditar.Controls.Add(lblEmail);
                formEditar.Controls.Add(txtEmail);
                formEditar.Controls.Add(lblSenha);
                formEditar.Controls.Add(txtSenha);
                formEditar.Controls.Add(lblSenhaInfo);
                formEditar.Controls.Add(lblAdmin);
                formEditar.Controls.Add(cbAdmin);
                formEditar.Controls.Add(btnSalvar);
                formEditar.Controls.Add(btnCancelar);

                formEditar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar utilizador: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 8 - CLICK 2 (para o Designer)
        // ============================================================
        private void button8_Click_2(object sender, EventArgs e)
        {
            // Chamar o método principal de editar
            button8_Click(sender, e);
        }

        // ============================================================
        // BOTÃO 7 - CRIAR NOVO UTILIZADOR
        // ============================================================
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 formRegisto = new Form1();
                formRegisto.StartPosition = FormStartPosition.CenterScreen;
                formRegisto.ShowDialog();
                CarregarUtilizadores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir registo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 6 - ELIMINAR UTILIZADOR
        // ============================================================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserId == 0)
                {
                    MessageBox.Show("Selecione um utilizador na lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = selectedUserId;
                string nome = "";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() == id.ToString())
                    {
                        nome = row.Cells["Nome"].Value?.ToString() ?? "";
                        break;
                    }
                }

                DialogResult result = MessageBox.Show(
                    $"Tem certeza que deseja ELIMINAR o utilizador:\n\nID: {id}\nNome: {nome}\n\nEsta ação não pode ser desfeita!",
                    "Confirmar Eliminação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string deleteCompras = "DELETE FROM compraTbl WHERE idcliente = @id";
                    SqlCommand cmdCompras = new SqlCommand(deleteCompras, conn);
                    cmdCompras.Parameters.AddWithValue("@id", id);
                    cmdCompras.ExecuteNonQuery();

                    string deleteUtilizador = "DELETE FROM utilizadorTbl WHERE id_cliente = @id";
                    SqlCommand cmd = new SqlCommand(deleteUtilizador, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Utilizador eliminado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedUserId = 0;
                        CarregarUtilizadores();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao eliminar utilizador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao eliminar utilizador: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 1 - PRODUTOS
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                form2.StartPosition = FormStartPosition.CenterScreen;
                form2.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 2 - COMPRAS
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                compras formCompras = new compras();
                formCompras.StartPosition = FormStartPosition.CenterScreen;
                formCompras.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Compras: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 3 - ESTOQUE
        // ============================================================
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                estoque formEstoque = new estoque();
                formEstoque.StartPosition = FormStartPosition.CenterScreen;
                formEstoque.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 4 - ATUALIZAR
        // ============================================================
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarUtilizadores();
                MessageBox.Show("Lista de utilizadores atualizada!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 5 - IGNORADO
        // ============================================================
        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade removida.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============================================================
        // STUBS DO DESIGNER
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void interface_principal_Load(object sender, EventArgs e) { }
        private void button1_Click_1(object sender, EventArgs e) { }
        private void pictureBox2_Click_1(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void button2_Click_1(object sender, EventArgs e) { }
        private void button3_Click_1(object sender, EventArgs e) { }
        private void button4_Click_1(object sender, EventArgs e) { }
        private void button5_Click_1(object sender, EventArgs e) { }
        private void button7_Click_1(object sender, EventArgs e) { }
        private void button6_Click_1(object sender, EventArgs e) { }
        private void button8_Click_1(object sender, EventArgs e) { }
    }
}