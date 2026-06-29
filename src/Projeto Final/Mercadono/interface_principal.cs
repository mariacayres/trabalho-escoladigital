
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mercadono
{
    public partial class interface_principal : Form
    {
        // Removed duplicate parameterless constructor from this partial file to fix CS0111.
        // Ensure exactly one parameterless constructor exists across all partial class files
        // (usually the designer file contains the constructor calling InitializeComponent).

        private void Interface_principal_Load(object sender, EventArgs e)
        {
            if (this.pictureBox1 != null)
            {
                if (this.pictureBox1.Image != null)
                {
                    var imgSize = this.pictureBox1.Image.Size;
                    var wa = Screen.FromControl(this).WorkingArea;
                    var target = new Size(
                        Math.Min(imgSize.Width, wa.Width),
                        Math.Min(imgSize.Height, wa.Height)
                    );

                    this.ClientSize = target;
                }
                else
                {
                    this.ClientSize = this.pictureBox1.Size;
                }
            }
        }

        // Removed duplicate pictureBox1_Click method from this partial file to fix CS0111.
        // If you still need custom behavior for pictureBox1.Click and no other partial contains that handler,
        // add a single handler implementation in one partial file and ensure the event is wired once.
    }
}