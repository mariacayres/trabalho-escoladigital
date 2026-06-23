using System;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class compras : Form
    {
        public compras()
        {
            InitializeComponent();
        }

        // Botão 1 - Abre o Form1 (Form1.cs)
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        // Botão 2 - Abre o Form de Descontos (descontos.cs)
        private void button2_Click(object sender, EventArgs e)
        {
            descontos formDescontos = new descontos();
            formDescontos.Show();
            this.Hide();
        }

        // Botão 3 - Abre o próprio Form de Estoque (estoque.cs)
        private void button3_Click(object sender, EventArgs e)
        {
            compras formEstoque = new compras();
            formEstoque.Show();
            this.Hide();
        }

        // Botão 4 - Abre o Form de Utilizadores (utilizadores.cs)
        private void button4_Click(object sender, EventArgs e)
        {
            utilizadores formUtilizadores = new utilizadores();
            formUtilizadores.Show();
            this.Hide();
        }
    }
}