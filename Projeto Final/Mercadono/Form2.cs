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

        public Form2()
        {
            InitializeComponent();
            ConfigurarLayout();
            this.Load += Form2_Load;
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
                    this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // MUDOU: NONE para controlar tamanhos
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

                    // ============================================================
                    // COLUNAS COM TAMANHOS REDUZIDOS
                    // ============================================================

                    // --- COLUNA ID (pequena) ---
                    if (this.dataGridView1.Columns["ID"] != null)
                    {
                        this.dataGridView1.Columns["ID"].Width = 50;   // REDUZIDO
                        this.dataGridView1.Columns["ID"].HeaderText = "ID";
                        this.dataGridView1.Columns["ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    // --- COLUNA Produto (média) ---
                    if (this.dataGridView1.Columns["Produto"] != null)
                    {
                        this.dataGridView1.Columns["Produto"].Width = 150; // REDUZIDO
                        this.dataGridView1.Columns["Produto"].HeaderText = "Produto";
                    }

                    // --- COLUNA Preço (pequena) ---
                    if (this.dataGridView1.Columns["Preço"] != null)
                    {
                        this.dataGridView1.Columns["Preço"].Width = 80;   // REDUZIDO
                        this.dataGridView1.Columns["Preço"].DefaultCellStyle.Format = "C2";
                        this.dataGridView1.Columns["Preço"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        this.dataGridView1.Columns["Preço"].HeaderText = "Preço";
                    }

                    // --- COLUNA Desconto (pequena) ---
                    if (this.dataGridView1.Columns["Desconto"] != null)
                    {
                        this.dataGridView1.Columns["Desconto"].Width = 70; // REDUZIDO
                        this.dataGridView1.Columns["Desconto"].DefaultCellStyle.Format = "0.00'%'";
                        this.dataGridView1.Columns["Desconto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        this.dataGridView1.Columns["Desconto"].HeaderText = "Desconto";
                    }

                    // --- COLUNA Quantidade (pequena) ---
                    if (this.dataGridView1.Columns["Quantidade"] != null)
                    {
                        this.dataGridView1.Columns["Quantidade"].Width = 70; // REDUZIDO
                        this.dataGridView1.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        this.dataGridView1.Columns["Quantidade"].HeaderText = "Qtd";
                    }

                    // Esconder colunas extras
                    foreach (DataGridViewColumn col in this.dataGridView1.Columns)
                    {
                        if (col.Name != "ID" && col.Name != "Produto" &&
                            col.Name != "Preço" && col.Name != "Desconto" &&
                            col.Name != "Quantidade")
                        {
                            col.Visible = false;
                        }
                    }

                    // Ajustar altura das linhas
                    this.dataGridView1.RowTemplate.Height = 30;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // CLICK NO DATAGRIDVIEW - MOSTRA DETALHES
        // ============================================================
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && this.dataGridView1 != null)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    string id = row.Cells["ID"]?.Value?.ToString() ?? "N/A";
                    string produto = row.Cells["Produto"]?.Value?.ToString() ?? "N/A";
                    string preco = row.Cells["Preço"]?.Value?.ToString() ?? "0";
                    string desconto = row.Cells["Desconto"]?.Value?.ToString() ?? "0";
                    string quantidade = row.Cells["Quantidade"]?.Value?.ToString() ?? "0";

                    MessageBox.Show(
                        $"📦 DETALHES DO PRODUTO\n\n" +
                        $"ID: {id}\n" +
                        $"Produto: {produto}\n" +
                        $"Preço: {Convert.ToDecimal(preco):C2}\n" +
                        $"Desconto: {desconto}%\n" +
                        $"Quantidade: {quantidade}",
                        "Detalhes do Produto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao mostrar detalhes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
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