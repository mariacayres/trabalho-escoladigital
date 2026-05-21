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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.pictureBox1 == null) return;

                if (this.pictureBox1.Image != null)
                {
                    var imgSize = this.pictureBox1.Image.Size;
                    var wa = Screen.FromControl(this).WorkingArea;
                    var target = new Size(
                        Math.Min(imgSize.Width, wa.Width),
                        Math.Min(imgSize.Height, wa.Height)
                    );

                    this.pictureBox1.Location = new Point(0, 0);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    this.pictureBox1.Size = target;
                    this.ClientSize = target;
                }
                else
                {
                    this.pictureBox1.Location = Point.Empty;
                    this.ClientSize = this.pictureBox1.Size;
                }
            }
            catch
            {
                // Fail silently
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // descontos formDescontos = new descontos();
            // formDescontos.Show();
            MessageBox.Show("Abrir Form de Descontos");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // estoque formEstoque = new estoque();
            // formEstoque.Show();
            MessageBox.Show("Abrir Form de Estoque");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}