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
        // No constructor here to avoid duplicate-constructor errors when the designer
        // or another partial file already defines one. Use OnLoad to perform sizing.

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.pictureBox1 == null) return;

                // Prefer the image native size; if no image, use the PictureBox control size
                Size desired = this.pictureBox1.Image != null
                    ? this.pictureBox1.Image.Size
                    : this.pictureBox1.Size;

                // Clamp size to the available screen working area
                var wa = Screen.FromControl(this).WorkingArea;
                var target = new Size(
                    Math.Min(desired.Width, wa.Width),
                    Math.Min(desired.Height, wa.Height)
                );

                // Apply as client size so docked/stretch PictureBox fills correctly
                this.ClientSize = target;
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
    }
}