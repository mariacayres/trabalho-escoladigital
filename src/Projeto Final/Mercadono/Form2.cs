using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class Form2 : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private int selectedProdutoId = 0;

        public Form2()
        {
            InitializeComponent();
            ConfigurarLayout();
            ConfigurarBotoes();
            this.Load += Form2_Load;
        }

        private void ConfigurarBotoes()
        {
            if (this.button6 != null)
            {
                this.button6.Click -= button6_Click;
                this.button6.Click += button6_Click;
                this.button6.Text = "EDITAR";
                this.button6.BackColor = System.Drawing.Color.Orange;
                this.button6.ForeColor = System.Drawing.Color.White;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private void ConfigurarLayout()
        {
            try
            {
                if (this.pictureBox1 != null)
                {
                    this.pictureBox1.Dock = DockStyle.Fill;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.pictureBox1.SendToBack();
                }

                if (this.dataGridView1 != null)
                {
                    this.dataGridView1.BringToFront();
                    this.dataGridView1.BackgroundColor = Color.White;
                    this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    this.dataGridView1.MultiSelect = false;
                    this.dataGridView1.ReadOnly = true;
                    this.dataGridView1.AllowUserToAddRows = false;
                    this.dataGridView1.RowHeadersVisible = false;
                }
            }
            catch { }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CELL CONTENT CLICK (nome exato que o Designer espera)
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.dataGridView1.Rows[e.RowIndex].Selected = true;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedProdutoId = Convert.ToInt32(row.Cells["ID"].Value);
            }
        }

        // ============================================================
        // CARREGAR PRODUTOS
        // ============================================================
        private void CarregarProdutos()
        {
            try
            {
                if (this.dataGridView1 == null)
                {
                    MessageBox.Show("DataGridView não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            idproduto AS ID,
                            nomepd AS Produto,
                            precopd AS Preço,
                            descontopd AS Desconto,
                            quantidadepd AS Quantidade
                        FROM ProdutoTbl
                        ORDER BY nomepd";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Nenhum produto encontrado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    this.dataGridView1.DataSource = dt;

                    if (this.dataGridView1.Columns["ID"] != null)
                    {
                        this.dataGridView1.Columns["ID"].Width = 50;
                        this.dataGridView1.Columns["ID"].HeaderText = "ID";
                        this.dataGridView1.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (this.dataGridView1.Columns["Produto"] != null)
                    {
                        this.dataGridView1.Columns["Produto"].Width = 150;
                        this.dataGridView1.Columns["Produto"].HeaderText = "Produto";
                    }

                    if (this.dataGridView1.Columns["Preço"] != null)
                    {
                        this.dataGridView1.Columns["Preço"].Width = 80;
                        this.dataGridView1.Columns["Preço"].DefaultCellStyle.Format = "C2";
                        this.dataGridView1.Columns["Preço"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        this.dataGridView1.Columns["Preço"].HeaderText = "Preço";
                    }

                    if (this.dataGridView1.Columns["Desconto"] != null)
                    {
                        this.dataGridView1.Columns["Desconto"].Width = 70;
                        this.dataGridView1.Columns["Desconto"].DefaultCellStyle.Format = "0.00'%'";
                        this.dataGridView1.Columns["Desconto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        this.dataGridView1.Columns["Desconto"].HeaderText = "Desconto";
                    }

                    if (this.dataGridView1.Columns["Quantidade"] != null)
                    {
                        this.dataGridView1.Columns["Quantidade"].Width = 70;
                        this.dataGridView1.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        this.dataGridView1.Columns["Quantidade"].HeaderText = "Qtd";
                    }

                    foreach (DataGridViewColumn col in this.dataGridView1.Columns)
                    {
                        if (col.Name != "ID" && col.Name != "Produto" &&
                            col.Name != "Preço" && col.Name != "Desconto" &&
                            col.Name != "Quantidade")
                        {
                            col.Visible = false;
                        }
                    }

                    this.dataGridView1.RowTemplate.Height = 30;
                    this.dataGridView1.ClearSelection();
                    selectedProdutoId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 6 - EDITAR PRODUTO
        // ============================================================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProdutoId == 0)
                {
                    MessageBox.Show("Selecione um produto na lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = selectedProdutoId;
                string nome = "";
                decimal preco = 0;
                decimal desconto = 0;
                int quantidade = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() == id.ToString())
                    {
                        nome = row.Cells["Produto"].Value?.ToString() ?? "";
                        preco = Convert.ToDecimal(row.Cells["Preço"].Value);
                        desconto = Convert.ToDecimal(row.Cells["Desconto"].Value);
                        quantidade = Convert.ToInt32(row.Cells["Quantidade"].Value);
                        break;
                    }
                }

                Form formEditar = new Form();
                formEditar.Text = "Editar Produto";
                formEditar.Size = new System.Drawing.Size(400, 320);
                formEditar.StartPosition = FormStartPosition.CenterScreen;
                formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formEditar.MaximizeBox = false;
                formEditar.MinimizeBox = false;

                Label lblNome = new Label() { Text = "Nome:", Location = new System.Drawing.Point(20, 30), Size = new System.Drawing.Size(80, 25) };
                TextBox txtNome = new TextBox() { Location = new System.Drawing.Point(110, 30), Size = new System.Drawing.Size(250, 25), Text = nome };

                Label lblPreco = new Label() { Text = "Preço:", Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(80, 25) };
                TextBox txtPreco = new TextBox() { Location = new System.Drawing.Point(110, 70), Size = new System.Drawing.Size(100, 25), Text = preco.ToString() };

                Label lblDesconto = new Label() { Text = "Desconto %:", Location = new System.Drawing.Point(20, 110), Size = new System.Drawing.Size(80, 25) };
                TextBox txtDesconto = new TextBox() { Location = new System.Drawing.Point(110, 110), Size = new System.Drawing.Size(80, 25), Text = desconto.ToString() };

                Label lblQuantidade = new Label() { Text = "Quantidade:", Location = new System.Drawing.Point(20, 150), Size = new System.Drawing.Size(80, 25) };
                NumericUpDown nudQuantidade = new NumericUpDown() { Location = new System.Drawing.Point(110, 150), Size = new System.Drawing.Size(80, 25), Minimum = 0, Maximum = 99999, Value = quantidade };

                Button btnSalvar = new Button() { Text = "SALVAR", Location = new System.Drawing.Point(110, 200), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGreen };
                Button btnCancelar = new Button() { Text = "CANCELAR", Location = new System.Drawing.Point(230, 200), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGray };

                btnSalvar.Click += (s, ev) =>
                {
                    string novoNome = txtNome.Text.Trim();
                    if (string.IsNullOrWhiteSpace(novoNome))
                    {
                        MessageBox.Show("Digite o nome do produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtPreco.Text, out decimal novoPreco) || novoPreco <= 0)
                    {
                        MessageBox.Show("Preço inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!decimal.TryParse(txtDesconto.Text, out decimal novoDesconto) || novoDesconto < 0 || novoDesconto > 100)
                    {
                        MessageBox.Show("Desconto inválido (0-100).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int novaQuantidade = (int)nudQuantidade.Value;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string query = @"
                                    UPDATE ProdutoTbl 
                                    SET nomepd = @nome, precopd = @preco, descontopd = @desconto, quantidadepd = @quantidade
                                    WHERE idproduto = @id";

                                SqlCommand cmd = new SqlCommand(query, conn, transaction);
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@nome", novoNome);
                                cmd.Parameters.AddWithValue("@preco", novoPreco);
                                cmd.Parameters.AddWithValue("@desconto", novoDesconto);
                                cmd.Parameters.AddWithValue("@quantidade", novaQuantidade);
                                cmd.ExecuteNonQuery();

                                string updateEstoque = @"
                                    UPDATE estoqueTbl 
                                    SET nomeEt = @nome, preco_do_produtoEt = @preco, quantidade_estoque = @quantidade
                                    WHERE idproduto = @id";

                                SqlCommand cmdEstoque = new SqlCommand(updateEstoque, conn, transaction);
                                cmdEstoque.Parameters.AddWithValue("@id", id);
                                cmdEstoque.Parameters.AddWithValue("@nome", novoNome);
                                cmdEstoque.Parameters.AddWithValue("@preco", novoPreco);
                                cmdEstoque.Parameters.AddWithValue("@quantidade", novaQuantidade);
                                cmdEstoque.ExecuteNonQuery();

                                transaction.Commit();

                                MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CarregarProdutos();
                                formEditar.Close();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                };

                btnCancelar.Click += (s, ev) => formEditar.Close();

                formEditar.Controls.Add(lblNome);
                formEditar.Controls.Add(txtNome);
                formEditar.Controls.Add(lblPreco);
                formEditar.Controls.Add(txtPreco);
                formEditar.Controls.Add(lblDesconto);
                formEditar.Controls.Add(txtDesconto);
                formEditar.Controls.Add(lblQuantidade);
                formEditar.Controls.Add(nudQuantidade);
                formEditar.Controls.Add(btnSalvar);
                formEditar.Controls.Add(btnCancelar);

                formEditar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 1 → PRODUTOS (atualizar)
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            CarregarProdutos();
            MessageBox.Show("Produtos atualizados!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============================================================
        // BOTÃO 2 → COMPRAS
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            compras form = new compras();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 3 → ESTOQUE
        // ============================================================
        private void button3_Click(object sender, EventArgs e)
        {
            estoque form = new estoque();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 4 → UTILIZADORES
        // ============================================================
        private void button4_Click(object sender, EventArgs e)
        {
            utilizadores form = new utilizadores();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 5 → IGNORADO
        // ============================================================
        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Funcionalidade removida.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============================================================
        // STUBS DO DESIGNER
        // ============================================================
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void button1_Click_1(object sender, EventArgs e) { }
        private void button2_Click_1(object sender, EventArgs e) { }
        private void button3_Click_1(object sender, EventArgs e) { }
        private void button4_Click_1(object sender, EventArgs e) { }
        private void button5_Click_1(object sender, EventArgs e) { }

        // ============================================================
        // REDIMENSIONAR
        // ============================================================
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            try
            {
                if (this.pictureBox1 != null)
                {
                    this.pictureBox1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
                }
                if (this.dataGridView1 != null)
                {
                    this.dataGridView1.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 150);
                }
            }
            catch { }
        }
    }
}