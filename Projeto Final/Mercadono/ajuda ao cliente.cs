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
    public partial class ajuda_ao_cliente : Form
    {
        public ajuda_ao_cliente()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.pictureBox1 == null) return;

                // Carrega a imagem da pictureBox1
                if (this.pictureBox1.Image != null)
                {
                    var imgSize = this.pictureBox1.Image.Size;
                    var wa = Screen.FromControl(this).WorkingArea;
                    var target = new Size(
                        Math.Min(imgSize.Width, wa.Width),
                        Math.Min(imgSize.Height, wa.Height)
                    );

                    // Configura a picture box para preencher todo o formulário
                    this.pictureBox1.Location = new Point(0, 0);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Muda para StretchImage
                    this.pictureBox1.Size = target;

                    // Faz o formulário ter o mesmo tamanho da imagem
                    this.ClientSize = target;

                    // Opcional: Remove bordas para parecer integrado
                    this.FormBorderStyle = FormBorderStyle.FixedSingle;
                    this.MaximizeBox = false;
                    this.MinimizeBox = false;
                }
                else
                {
                    this.pictureBox1.Location = Point.Empty;
                    this.ClientSize = this.pictureBox1.Size;
                }

                // Centraliza o formulário na tela
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch
            {
                // Falha silenciosa
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Fecha o formulário se pressionar Ctrl + clique
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                this.Close();
            }
        }
    }
} 