using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using c_auth;

namespace FinguAuth
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (c_api.c_register(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text))
            {
                new Login().Show();
                this.Hide();
            }
        }
    }
}
