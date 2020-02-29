using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using c_auth;


namespace FinguAuth
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread.Sleep(2300);
            Console.Beep();

            c_api.program_key = "";
            c_api.enc_key = "";
            c_api.c_init("1.0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (c_api.c_login(textBox1.Text, textBox2.Text))
            {
                new Main().Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Register().Show();
            this.Hide();
        }
    }
}
