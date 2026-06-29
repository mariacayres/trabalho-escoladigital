using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class compras : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private int selectedCompraId = 0;

        public compras()
        {
            InitializeComponent();
            CarregarCompras();
            ConfigurarBotoes();
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            if (this.dataGridView1 != null)
            {
                this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.dataGridView1.MultiSelect = false;
                this.dataGridView1.ReadOnly = true;
                this.dataGridView1.CellClick += DataGridView1_CellClick;
            }
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
                this.button4.Text = "UTILIZADORES";
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
        }

        // ============================================================
        // DATAGRIDVIEW1 - CLICK
        // ============================================================
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.dataGridView1.Rows[e.RowIndex].Selected = true;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedCompraId = Convert.ToInt32(row.Cells["ID"].Value);
            }
        }

        // ============================================================
        // CARREGAR COMPRAS
        // ============================================================
        private void CarregarCompras()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.idcompra AS 'ID',
                            u.nome AS 'Cliente',
                            p.nomepd AS 'Produto',
                            c.quantidade AS 'Qtd',
                            c.valorfinal AS 'Valor Total',
                            c.data_compra AS 'Data'
                        FROM compraTbl c
                        INNER JOIN utilizadorTbl u ON c.idcliente = u.id_cliente
                        INNER JOIN ProdutoTbl p ON c.id_produto = p.idproduto
                        ORDER BY c.data_compra DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns["Valor Total"] != null)
                    {
                        dataGridView1.Columns["Valor Total"].DefaultCellStyle.Format = "C2";
                    }
                    if (dataGridView1.Columns["Data"] != null)
                    {
                        dataGridView1.Columns["Data"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }

                    dataGridView1.ClearSelection();
                    selectedCompraId = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 8 - EDITAR COMPRA
        // ============================================================
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCompraId == 0)
                {
                    MessageBox.Show("Selecione uma compra na lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = selectedCompraId;
                string cliente = "";
                string produto = "";
                int qtd = 0;
                decimal valor = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() == id.ToString())
                    {
                        cliente = row.Cells["Cliente"].Value?.ToString() ?? "";
                        produto = row.Cells["Produto"].Value?.ToString() ?? "";
                        qtd = Convert.ToInt32(row.Cells["Qtd"].Value);
                        valor = Convert.ToDecimal(row.Cells["Valor Total"].Value);
                        break;
                    }
                }

                // Criar formulário para editar
                Form formEditar = new Form();
                formEditar.Text = "Editar Compra";
                formEditar.Size = new System.Drawing.Size(400, 300);
                formEditar.StartPosition = FormStartPosition.CenterScreen;
                formEditar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formEditar.MaximizeBox = false;
                formEditar.MinimizeBox = false;

                Label lblProduto = new Label() { Text = "Produto:", Location = new System.Drawing.Point(20, 30), Size = new System.Drawing.Size(80, 25) };
                TextBox txtProduto = new TextBox() { Location = new System.Drawing.Point(110, 30), Size = new System.Drawing.Size(250, 25), Text = produto, ReadOnly = true };

                Label lblCliente = new Label() { Text = "Cliente:", Location = new System.Drawing.Point(20, 70), Size = new System.Drawing.Size(80, 25) };
                TextBox txtCliente = new TextBox() { Location = new System.Drawing.Point(110, 70), Size = new System.Drawing.Size(250, 25), Text = cliente, ReadOnly = true };

                Label lblQtd = new Label() { Text = "Quantidade:", Location = new System.Drawing.Point(20, 110), Size = new System.Drawing.Size(80, 25) };
                NumericUpDown nudQtd = new NumericUpDown() { Location = new System.Drawing.Point(110, 110), Size = new System.Drawing.Size(80, 25), Minimum = 1, Maximum = 999, Value = qtd };

                Label lblValor = new Label() { Text = "Valor Total:", Location = new System.Drawing.Point(20, 150), Size = new System.Drawing.Size(80, 25) };
                TextBox txtValor = new TextBox() { Location = new System.Drawing.Point(110, 150), Size = new System.Drawing.Size(100, 25), Text = valor.ToString("C2"), ReadOnly = true };

                Button btnSalvar = new Button() { Text = "SALVAR", Location = new System.Drawing.Point(110, 200), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGreen };
                Button btnCancelar = new Button() { Text = "CANCELAR", Location = new System.Drawing.Point(230, 200), Size = new System.Drawing.Size(100, 35), BackColor = System.Drawing.Color.LightGray };

                btnSalvar.Click += (s, ev) =>
                {
                    int novaQtd = (int)nudQtd.Value;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE compraTbl SET quantidade = @quantidade WHERE idcompra = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@quantidade", novaQtd);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Compra atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarCompras();
                            formEditar.Close();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao atualizar compra.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                btnCancelar.Click += (s, ev) => formEditar.Close();

                formEditar.Controls.Add(lblProduto);
                formEditar.Controls.Add(txtProduto);
                formEditar.Controls.Add(lblCliente);
                formEditar.Controls.Add(txtCliente);
                formEditar.Controls.Add(lblQtd);
                formEditar.Controls.Add(nudQtd);
                formEditar.Controls.Add(lblValor);
                formEditar.Controls.Add(txtValor);
                formEditar.Controls.Add(btnSalvar);
                formEditar.Controls.Add(btnCancelar);

                formEditar.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar compra: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 6 - ELIMINAR COMPRA
        // ============================================================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCompraId == 0)
                {
                    MessageBox.Show("Selecione uma compra na lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = selectedCompraId;

                DialogResult result = MessageBox.Show(
                    $"Tem certeza que deseja ELIMINAR a compra ID {id}?\n\nEsta ação não pode ser desfeita!",
                    "Confirmar Eliminação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM compraTbl WHERE idcompra = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Compra eliminada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedCompraId = 0;
                        CarregarCompras();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao eliminar compra.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao eliminar compra: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÃO 7 - CRIAR COMPRA
        // ============================================================
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                interface_principal formPrincipal = new interface_principal();
                formPrincipal.StartPosition = FormStartPosition.CenterScreen;
                formPrincipal.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir compras: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        // BOTÃO 2 - COMPRAS (recarregar)
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarCompras();
                MessageBox.Show("Lista atualizada!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        // BOTÃO 4 - UTILIZADORES
        // ============================================================
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                utilizadores formUtilizadores = new utilizadores();
                formUtilizadores.StartPosition = FormStartPosition.CenterScreen;
                formUtilizadores.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Utilizadores: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void pictureBox2_Click_1(object sender, EventArgs e) { }
        private void button8_Click_1(object sender, EventArgs e) { }
        private void button6_Click_1(object sender, EventArgs e) { }
        private void button7_Click_1(object sender, EventArgs e) { }
    }
}