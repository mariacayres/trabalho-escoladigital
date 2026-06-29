using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class estoque : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private int selectedEstoqueId = 0;

        public estoque()
        {
            InitializeComponent();
            ConfigurarBotoes();
            ConfigurarDataGridView();
            CarregarEstoque();
        }

        private void ConfigurarBotoes()
        {
            if (this.button6 != null)
            {
                this.button6.Click -= button6_Click;
                this.button6.Click += button6_Click;
                this.button6.Text = "EDITAR";
                this.button6.BackColor = Color.Orange;
                this.button6.ForeColor = Color.White;
            }
        }

        private void ConfigurarDataGridView()
        {
            if (this.dataGridView1 != null)
            {
                this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.dataGridView1.MultiSelect = false;
                this.dataGridView1.ReadOnly = true;
                // Adicionar evento CellClick para garantir a seleção
                this.dataGridView1.CellClick += DataGridView1_CellClick;
            }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CELL CLICK (selecionar)
        // ============================================================
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedEstoqueId = Convert.ToInt32(row.Cells["ID"].Value);

                // Mensagem para confirmar que selecionou
                string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                MessageBox.Show($"Item selecionado: {produto} (ID: {selectedEstoqueId})", "Selecionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CELL CONTENT CLICK (usado pelo Designer)
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedEstoqueId = Convert.ToInt32(row.Cells["ID"].Value);
            }
        }

        // ============================================================
        // CARREGAR ESTOQUE
        // ============================================================
        private void CarregarEstoque()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            e.idestoque AS 'ID',
                            p.nomepd AS 'Produto',
                            e.quantidade_estoque AS 'Qtd',
                            e.preco_do_produtoEt AS 'Preço'
                        FROM estoqueTbl e
                        INNER JOIN ProdutoTbl p ON e.idproduto = p.idproduto
                        ORDER BY p.nomepd";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns["ID"] != null)
                    {
                        dataGridView1.Columns["ID"].Width = 60;
                        dataGridView1.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridView1.Columns["Produto"] != null)
                    {
                        dataGridView1.Columns["Produto"].Width = 200;
                    }
                    if (dataGridView1.Columns["Qtd"] != null)
                    {
                        dataGridView1.Columns["Qtd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridView1.Columns["Preço"] != null)
                    {
                        dataGridView1.Columns["Preço"].DefaultCellStyle.Format = "C2";
                        dataGridView1.Columns["Preço"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    dataGridView1.ClearSelection();
                    selectedEstoqueId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 6 - EDITAR ESTOQUE
        // ============================================================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedEstoqueId == 0)
                {
                    MessageBox.Show("Clique primeiro num item da lista para selecionar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = selectedEstoqueId;
                string produto = "";
                int quantidade = 0;
                decimal preco = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() == id.ToString())
                    {
                        produto = row.Cells["Produto"].Value?.ToString() ?? "";
                        quantidade = Convert.ToInt32(row.Cells["Qtd"].Value);
                        preco = Convert.ToDecimal(row.Cells["Preço"].Value);
                        break;
                    }
                }

                Form formEditar = new Form();
                formEditar.Text = "Editar Estoque";
                formEditar.Size = new System.Drawing.Size(400, 250);
                formEditar.StartPosition = FormStartPosition.CenterScreen;
                formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formEditar.MaximizeBox = false;
                formEditar.MinimizeBox = false;

                Label lblProduto = new Label() { Text = "Produto:", Location = new System.Drawing.Point(20, 30), Size = new System.Drawing.Size(80, 25) };
                TextBox txtProduto = new TextBox() { Location = new System.Drawing.Point(110, 30), Size = new System.Drawing.Size(250, 25), Text = produto, ReadOnly = true };

                Label lblQtd = new Label() { Text = "Quantidade:", Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(80, 25) };
                NumericUpDown nudQtd = new NumericUpDown() { Location = new System.Drawing.Point(110, 70), Size = new System.Drawing.Size(100, 25), Minimum = 0, Maximum = 99999, Value = quantidade };

                Label lblPreco = new Label() { Text = "Preço:", Location = new System.Drawing.Point(20, 110), Size = new System.Drawing.Size(80, 25) };
                TextBox txtPreco = new TextBox() { Location = new System.Drawing.Point(110, 110), Size = new System.Drawing.Size(100, 25), Text = preco.ToString("C2"), ReadOnly = true };

                Button btnSalvar = new Button() { Text = "SALVAR", Location = new System.Drawing.Point(110, 160), Size = new System.Drawing.Size(100, 35), BackColor = Color.LightGreen };
                Button btnCancelar = new Button() { Text = "CANCELAR", Location = new System.Drawing.Point(230, 160), Size = new System.Drawing.Size(100, 35), BackColor = Color.LightGray };

                btnSalvar.Click += (s, ev) =>
                {
                    int novaQtd = (int)nudQtd.Value;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string updateEstoque = @"
                                    UPDATE estoqueTbl 
                                    SET quantidade_estoque = @quantidade
                                    WHERE idestoque = @id";

                                SqlCommand cmdEstoque = new SqlCommand(updateEstoque, conn, transaction);
                                cmdEstoque.Parameters.AddWithValue("@id", id);
                                cmdEstoque.Parameters.AddWithValue("@quantidade", novaQtd);
                                cmdEstoque.ExecuteNonQuery();

                                string updateProduto = @"
                                    UPDATE ProdutoTbl 
                                    SET quantidadepd = @quantidade
                                    WHERE idproduto = (SELECT idproduto FROM estoqueTbl WHERE idestoque = @id)";

                                SqlCommand cmdProduto = new SqlCommand(updateProduto, conn, transaction);
                                cmdProduto.Parameters.AddWithValue("@id", id);
                                cmdProduto.Parameters.AddWithValue("@quantidade", novaQtd);
                                cmdProduto.ExecuteNonQuery();

                                transaction.Commit();

                                MessageBox.Show("Estoque atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CarregarEstoque();
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

                formEditar.Controls.Add(lblProduto);
                formEditar.Controls.Add(txtProduto);
                formEditar.Controls.Add(lblQtd);
                formEditar.Controls.Add(nudQtd);
                formEditar.Controls.Add(lblPreco);
                formEditar.Controls.Add(txtPreco);
                formEditar.Controls.Add(btnSalvar);
                formEditar.Controls.Add(btnCancelar);

                formEditar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 1 - PRODUTOS
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 2 - COMPRAS
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            compras formCompras = new compras();
            formCompras.StartPosition = FormStartPosition.CenterScreen;
            formCompras.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 3 - ESTOQUE (recarregar)
        // ============================================================
        private void button3_Click(object sender, EventArgs e)
        {
            CarregarEstoque();
            MessageBox.Show("Estoque atualizado!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============================================================
        // BOTÃO 4 - UTILIZADORES
        // ============================================================
        private void button4_Click(object sender, EventArgs e)
        {
            utilizadores formUtilizadores = new utilizadores();
            formUtilizadores.StartPosition = FormStartPosition.CenterScreen;
            formUtilizadores.Show();
            this.Hide();
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
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void interface_principal_Load(object sender, EventArgs e) { }
        private void button1_Click_1(object sender, EventArgs e) { }
        private void button2_Click_1(object sender, EventArgs e) { }
        private void button3_Click_1(object sender, EventArgs e) { }
        private void button4_Click_1(object sender, EventArgs e) { }
        private void button5_Click_1(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void button6_Click_1(object sender, EventArgs e) { }
    }
}