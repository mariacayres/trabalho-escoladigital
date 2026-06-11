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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // aqui vc vai ler a txt1, txt2 e fazer a validação do login a 1 e o gmail e a 2 a senha, se for valido vc abre o form2
            // depois vc pode fechar o forms de login e abrir a interface proncipal
        }

        private void textBox2passe_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3email_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxname_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            OpenLogin();
        }

        // Some designer files reference this name; keep it to avoid missing-method errors.
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLogin();
        }

        private void OpenLogin()
        {
            try
            {
                var form1 = new Form1();
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.FormClosed += (s, args) => { try { this.Show(); } catch { } };
                form1.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário de Login: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }



        private void textBox2passe_TextChanged(object sender, EventArgs e)

        {

            textBox2passe.PasswordChar = '●';

            textBox2passe btnOlho = new Button

            {

                Text = "👁",

                Location = new Point(textBox2passe.Right + 4, textBox2passe.Top),

                Size = new Size(30, textBox2passe.Height),

                FlatStyle = FlatStyle.Flat,

                Cursor = Cursors.Hand,

                Font = new Font("Segoe UI", 10f),

            };

            btnOlho.FlatAppearance.BorderSize = 0;

            btnOlho.Click += (s, e) =>

            {

                textBox2passe.PasswordChar = textBox2passe.PasswordChar == '\0' ? '●' : '\0';

            };

            this.Controls.Add(btnOlho);

        }


    }
}
