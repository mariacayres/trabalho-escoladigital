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
    public partial class interface_principal : Form
    {
        // Constructor: ensure designer controls are initialized
        public interface_principal()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.pictureBox1 == null) return;

                // If an image exists, size the form and picture box to the image (clamped to working area).
                if (this.pictureBox1.Image != null)
                {
                    var imgSize = this.pictureBox1.Image.Size;
                    var wa = Screen.FromControl(this).WorkingArea;
                    var target = new Size(
                        Math.Min(imgSize.Width, wa.Width),
                        Math.Min(imgSize.Height, wa.Height)
                    );

                    // Ensure picture box is placed at the client origin and displays the image at native size
                    this.pictureBox1.Location = new Point(0, 0);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    this.pictureBox1.Size = target;

                    // Make the form client area match the image size
                    this.ClientSize = target;
                }
                else
                {
                    // No image: ensure picture is aligned and form matches the control size
                    this.pictureBox1.Location = Point.Empty;
                    this.ClientSize = this.pictureBox1.Size;
                }
            }
            catch
            {
                // Fail silently to avoid preventing the form from showing.
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Optional: custom click behavior for the picture.
            // Example: close on Ctrl+click:
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}