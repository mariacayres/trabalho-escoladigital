using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class estoque : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";

        public estoque()
        {
            InitializeComponent();
            CarregarEstoque();
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
                            e.quantidade_estoque AS 'Qtd'
                        FROM estoqueTbl e
                        INNER JOIN ProdutoTbl p ON e.idproduto = p.idproduto
                        ORDER BY p.nomepd";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Ajustar tamanho das colunas
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estoque: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DATAGRIDVIEW1 - CLICK (mostrar detalhes)
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string id = row.Cells["ID"].Value?.ToString() ?? "";
                string produto = row.Cells["Produto"].Value?.ToString() ?? "";
                string qtd = row.Cells["Qtd"].Value?.ToString() ?? "";

                MessageBox.Show(
                    $"📦 DETALHES DO ESTOQUE\n\n" +
                    $"ID: {id}\n" +
                    $"Produto: {produto}\n" +
                    $"Quantidade: {qtd}",
                    "Detalhes do Estoque",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        // ============================================================
        // BOTÃO 1 - PRODUTOS → Form2.cs
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 2 - COMPRAS → compras.cs
        // ============================================================
        private void button2_Click(object sender, EventArgs e)
        {
            compras formCompras = new compras();
            formCompras.StartPosition = FormStartPosition.CenterScreen;
            formCompras.Show();
            this.Hide();
        }

        // ============================================================
        // BOTÃO 3 - ESTOQUE → recarregar
        // ============================================================
        private void button3_Click(object sender, EventArgs e)
        {
            CarregarEstoque();
            MessageBox.Show("Estoque atualizado!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============================================================
        // BOTÃO 4 - UTILIZADORES → utilizadores.cs
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
    }
}