using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class compras : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";

        public compras()
        {
            InitializeComponent();
            CarregarCompras();

            // ============================================================
            // LIGAR OS BOTÕES MANUALMENTE (CASO O DESIGNER NÃO ESTEJA A LIGAR)
            // ============================================================
            this.Load += (s, e) =>
            {
                // Procurar botões pelo nome e ligar os eventos
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        // Se o botão tiver "1" no nome ou for "button1"
                        if (btn.Name.Contains("1") || btn.Name == "button1")
                        {
                            btn.Click -= button1_Click;
                            btn.Click += button1_Click;
                            btn.Text = "PRODUTOS";
                        }
                        // Se o botão tiver "2" no nome ou for "button2"
                        else if (btn.Name.Contains("2") || btn.Name == "button2")
                        {
                            btn.Click -= button2_Click;
                            btn.Click += button2_Click;
                            btn.Text = "COMPRAS";
                        }
                        // Se o botão tiver "3" no nome ou for "button3"
                        else if (btn.Name.Contains("3") || btn.Name == "button3")
                        {
                            btn.Click -= button3_Click;
                            btn.Click += button3_Click;
                            btn.Text = "ESTOQUE";
                        }
                        // Se o botão tiver "4" no nome ou for "button4"
                        else if (btn.Name.Contains("4") || btn.Name == "button4")
                        {
                            btn.Click -= button4_Click;
                            btn.Click += button4_Click;
                            btn.Text = "UTILIZADORES";
                        }
                        // Se o botão tiver "5" no nome ou for "button5"
                        else if (btn.Name.Contains("5") || btn.Name == "button5")
                        {
                            btn.Click -= button5_Click;
                            btn.Click += button5_Click;
                            btn.Text = "REMOVER";
                        }
                    }
                }
            };
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CLICK
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                MessageBox.Show(
                    $"📋 DETALHES DA COMPRA\n\n" +
                    $"ID: {row.Cells["ID"].Value}\n" +
                    $"Cliente: {row.Cells["Cliente"].Value}\n" +
                    $"Produto: {row.Cells["Produto"].Value}\n" +
                    $"Quantidade: {row.Cells["Qtd"].Value}\n" +
                    $"Valor Total: {row.Cells["Valor Total"].Value:C2}\n" +
                    $"Data: {row.Cells["Data"].Value}",
                    "Detalhes da Compra",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}