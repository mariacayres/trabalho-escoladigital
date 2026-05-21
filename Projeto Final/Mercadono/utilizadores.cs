using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class utilizadores : Form
    {
        public utilizadores()
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

        // Botão 3 - Abre o Form de Estoque (estoque.cs)
        private void button3_Click(object sender, EventArgs e)
        {
            estoque formEstoque = new estoque();
            formEstoque.Show();
            this.Hide();
        }

        // Botão 4 - Abre o próprio Form de Utilizadores (utilizadores.cs)
        private void button4_Click(object sender, EventArgs e)
        {
            utilizadores formUtilizadores = new utilizadores();
            formUtilizadores.Show();
            this.Hide();
        }

        // Botão 5 - Abre o Form de Ajuda ao Cliente (ajuda ao cliente.cs)
        private void button5_Click(object sender, EventArgs e)
        {
            ajuda_ao_cliente formAjuda = new ajuda_ao_cliente();
            formAjuda.Show();
            this.Hide();
        }
    }
}
