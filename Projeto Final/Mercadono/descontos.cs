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
    public partial class descontos : Form
    {
        public descontos()
        {
            InitializeComponent();
        }

        // Botão 1 - Abre o Form1
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 == null)
            {
                form1 = new Form1();
                form1.Show();
            }
            else
            {
                form1.BringToFront();
            }
            this.Hide();
        }

        // Botão 2 - Abre o próprio Form de Descontos
        private void button2_Click(object sender, EventArgs e)
        {
            descontos formDescontos = Application.OpenForms.OfType<descontos>().FirstOrDefault();
            if (formDescontos == null)
            {
                formDescontos = new descontos();
                formDescontos.Show();
            }
            else
            {
                formDescontos.BringToFront();
            }
            this.Hide();
        }

        // Botão 3 - Abre o Form de Estoque
        private void button3_Click(object sender, EventArgs e)
        {
            estoque formEstoque = Application.OpenForms.OfType<estoque>().FirstOrDefault();
            if (formEstoque == null)
            {
                formEstoque = new estoque();
                formEstoque.Show();
            }
            else
            {
                formEstoque.BringToFront();
            }
            this.Hide();
        }

        // Botão 4 - Abre o Form de Utilizadores
        private void button4_Click(object sender, EventArgs e)
        {
            utilizadores formUtilizadores = Application.OpenForms.OfType<utilizadores>().FirstOrDefault();
            if (formUtilizadores == null)
            {
                formUtilizadores = new utilizadores();
                formUtilizadores.Show();
            }
            else
            {
                formUtilizadores.BringToFront();
            }
            this.Hide();
        }

        // Botão 5 - Abre o Form de Ajuda ao Cliente
        private void button5_Click(object sender, EventArgs e)
        {
            ajuda_ao_cliente formAjuda = Application.OpenForms.OfType<ajuda_ao_cliente>().FirstOrDefault();
            if (formAjuda == null)
            {
                formAjuda = new ajuda_ao_cliente();
                formAjuda.Show();
            }
            else
            {
                formAjuda.BringToFront();
            }
            this.Hide();
        }
    }
}