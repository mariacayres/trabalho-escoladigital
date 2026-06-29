using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class interface_principal : Form
    {
        private readonly string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=mercadono;Integrated Security=True;";
        private Label lblTotal;
        private Panel panelProdutos;
        private bool isCarregado = false;

        public interface_principal()
        {
            InitializeComponent();
            InicializarControles();

            if (!isCarregado)
            {
                isCarregado = true;
                CarregarProdutos();
            }
        }

        private void InicializarControles()
        {
            if (panelProdutos == null)
            {
                panelProdutos = new Panel
                {
                    Name = "panelProdutos",
                    Location = new Point(10, 10),
                    Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 150),
                    AutoScroll = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    BackColor = Color.Transparent
                };
                this.Controls.Add(panelProdutos);
            }

            if (lblTotal == null)
            {
                lblTotal = new Label
                {
                    Name = "lblTotal",
                    Text = "Total: R$ 0,00",
                    Location = new Point(20, this.ClientSize.Height - 110),
                    Size = new Size(400, 30),
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.DarkGreen,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                };
                this.Controls.Add(lblTotal);
            }

            if (this.button1 != null)
            {
                this.button1.Text = "COMPRAR";
                this.button1.BackColor = Color.LightGreen;
                this.button1.Font = new Font("Arial", 12, FontStyle.Bold);
                this.button1.Size = new Size(150, 40);
                this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
                this.button1.Location = new Point(20, this.ClientSize.Height - 60);
                this.button1.Click -= button1_Click;
                this.button1.Click += button1_Click;
            }

            this.Resize += (s, e) => {
                if (this.button1 != null)
                    this.button1.Location = new Point(20, this.ClientSize.Height - 60);
                if (lblTotal != null)
                    lblTotal.Location = new Point(20, this.ClientSize.Height - 110);
                if (panelProdutos != null)
                    panelProdutos.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 150);
            };
        }

        private void CarregarProdutos()
        {
            try
            {
                if (panelProdutos != null)
                {
                    panelProdutos.Controls.Clear();
                    panelProdutos.Refresh();
                }

                DataTable dt = BuscarProdutosEmEstoque();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Nenhum produto encontrado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.SuspendLayout();

                int y = 10;
                const int x = 10;

                foreach (DataRow row in dt.Rows)
                {
                    int idProduto = Convert.ToInt32(row["idproduto"]);
                    string nome = row["Produto"]?.ToString() ?? "";
                    decimal preco = ParseDecimalSafe(row["Preço"]);
                    decimal desconto = ParseDecimalSafe(row["Desconto"]);
                    int estoque = Convert.ToInt32(row["Estoque_Disponível"] ?? 0);

                    CheckBox chk = new CheckBox
                    {
                        Name = $"chk{idProduto}",
                        Text = $"{idProduto} - {nome} | Preço: {preco:C2} | Desconto: {desconto}% | Estoque: {estoque}",
                        Location = new Point(x, y),
                        Width = Math.Max(600, panelProdutos.Width - 30),
                        Height = 30,
                        Font = new Font("Arial", 10),
                        Tag = row,
                        AutoSize = false,
                        BackColor = Color.Transparent,
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                    };

                    chk.CheckedChanged -= Chk_CheckedChanged;
                    chk.CheckedChanged += Chk_CheckedChanged;

                    if (estoque == 0)
                    {
                        chk.Enabled = false;
                        chk.Text += " (SEM ESTOQUE)";
                    }

                    panelProdutos.Controls.Add(chk);
                    y += 35;
                }

                panelProdutos.BringToFront();
                this.button1?.BringToFront();
                lblTotal.BringToFront();

                this.ResumeLayout();
                AtualizarTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarTotal()
        {
            try
            {
                decimal total = 0;
                int count = 0;

                if (panelProdutos != null)
                {
                    foreach (Control ctrl in panelProdutos.Controls)
                    {
                        if (ctrl is CheckBox chk && chk.Checked && chk.Tag is DataRow row)
                        {
                            decimal preco = ParseDecimalSafe(row["Preço"]);
                            decimal desconto = ParseDecimalSafe(row["Desconto"]);
                            decimal precoComDesconto = preco * (1 - desconto / 100m);
                            total += precoComDesconto;
                            count++;
                        }
                    }
                }

                if (lblTotal != null)
                {
                    lblTotal.Text = $"Total: {total:C2} | Itens: {count}";
                }
            }
            catch { }
        }

        private static decimal ParseDecimalSafe(object value)
        {
            if (value == null || value == DBNull.Value) return 0m;
            if (value is decimal d) return d;
            var s = value.ToString();
            decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d);
            if (d == 0m)
                decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d);
            return d;
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarTotal();
        }

        // ============================================================
        // BOTÃO COMPRAR
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int idCliente = 0;
                try { idCliente = Session.LoggedUserId; } catch { }

                if (idCliente <= 0)
                {
                    MessageBox.Show("Faça login novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selecionados = panelProdutos.Controls
                    .OfType<CheckBox>()
                    .Where(chk => chk.Checked && chk.Tag is DataRow)
                    .Select(chk => (Check: chk, Row: (DataRow)chk.Tag))
                    .ToList();

                if (selecionados.Count == 0)
                {
                    MessageBox.Show("Selecione pelo menos um produto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mensagem = "ITENS SELECIONADOS:\n\n";
                decimal totalGeral = 0;

                foreach (var item in selecionados)
                {
                    DataRow row = item.Row;
                    string nome = row["Produto"]?.ToString() ?? "";
                    decimal preco = ParseDecimalSafe(row["Preço"]);
                    decimal desconto = ParseDecimalSafe(row["Desconto"]);
                    int estoque = Convert.ToInt32(row["Estoque_Disponível"] ?? 0);

                    if (estoque <= 0)
                    {
                        MessageBox.Show($"Produto {nome} sem estoque!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    decimal precoComDesconto = preco * (1 - desconto / 100m);
                    mensagem += $"• {nome} = {precoComDesconto:C2}";
                    if (desconto > 0) mensagem += $" (desconto: {desconto}%)";
                    mensagem += "\n";
                    totalGeral += precoComDesconto;
                }

                mensagem += $"\nTOTAL: {totalGeral:C2}\n\nConfirmar compra?";

                if (MessageBox.Show(mensagem, "CONFIRMAR COMPRA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                bool tudoOk = true;

                foreach (var item in selecionados)
                {
                    DataRow row = item.Row;
                    int idProduto = Convert.ToInt32(row["idproduto"]);
                    decimal preco = ParseDecimalSafe(row["Preço"]);
                    decimal desconto = ParseDecimalSafe(row["Desconto"]);
                    decimal precoComDesconto = preco * (1 - desconto / 100m);

                    bool ok = InserirCompra(idCliente, idProduto, 1, precoComDesconto);
                    if (!ok)
                    {
                        tudoOk = false;
                        MessageBox.Show($"Erro ao comprar {row["Produto"]}.", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (tudoOk)
                {
                    MessageBox.Show("COMPRA REALIZADA COM SUCESSO!", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (CheckBox chk in panelProdutos.Controls.OfType<CheckBox>().ToList())
                        chk.Checked = false;

                    CarregarProdutos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DATABASE HELPERS
        // ============================================================

        public DataTable BuscarProdutosEmEstoque()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            p.idproduto,
                            p.nomepd AS Produto,
                            e.quantidade_estoque AS [Estoque_Disponível],
                            p.precopd AS [Preço],
                            p.descontopd AS [Desconto]
                        FROM ProdutoTbl p
                        INNER JOIN estoqueTbl e ON p.idproduto = e.idproduto
                        ORDER BY p.idproduto";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar produtos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public bool InserirCompra(int idCliente, int idProduto, int quantidade, decimal valorFinal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string checkEstoque = "SELECT quantidade_estoque FROM estoqueTbl WITH (UPDLOCK) WHERE idproduto = @idProduto";
                            SqlCommand checkCmd = new SqlCommand(checkEstoque, conn, transaction);
                            checkCmd.Parameters.AddWithValue("@idProduto", idProduto);

                            object result = checkCmd.ExecuteScalar();
                            int estoqueAtual = result == null ? 0 : Convert.ToInt32(result);

                            if (estoqueAtual < quantidade)
                            {
                                transaction.Rollback();
                                return false;
                            }

                            string query = @"
                                INSERT INTO compraTbl (idcliente, id_produto, quantidade, valorfinal, data_compra)
                                VALUES (@idCliente, @idProduto, @quantidade, @valorFinal, GETDATE())";

                            SqlCommand cmd = new SqlCommand(query, conn, transaction);
                            cmd.Parameters.AddWithValue("@idCliente", idCliente);
                            cmd.Parameters.AddWithValue("@idProduto", idProduto);
                            cmd.Parameters.AddWithValue("@quantidade", quantidade);
                            cmd.Parameters.AddWithValue("@valorFinal", valorFinal);

                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                string updateEstoque = @"
                                    UPDATE estoqueTbl 
                                    SET quantidade_estoque = quantidade_estoque - @quantidade,
                                        ultima_atualizacao = GETDATE()
                                    WHERE idproduto = @idProduto";

                                SqlCommand cmdEstoque = new SqlCommand(updateEstoque, conn, transaction);
                                cmdEstoque.Parameters.AddWithValue("@quantidade", quantidade);
                                cmdEstoque.Parameters.AddWithValue("@idProduto", idProduto);
                                cmdEstoque.ExecuteNonQuery();

                                string updateProduto = @"
                                    UPDATE ProdutoTbl 
                                    SET quantidadepd = quantidadepd - @quantidade
                                    WHERE idproduto = @idProduto";

                                SqlCommand cmdProduto = new SqlCommand(updateProduto, conn, transaction);
                                cmdProduto.Parameters.AddWithValue("@quantidade", quantidade);
                                cmdProduto.Parameters.AddWithValue("@idProduto", idProduto);
                                cmdProduto.ExecuteNonQuery();

                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir compra: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Stubs do Designer
        private void pictureBox3_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void interface_principal_Load(object sender, EventArgs e) { }
    }
}