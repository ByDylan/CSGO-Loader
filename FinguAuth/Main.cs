using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Web;
using ManualMapInjection.Injection;

using c_auth;

namespace FinguAuth
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            MaximizeBox = false;
            MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("CSGO");
            comboBox1.Items.Add("R6");
            comboBox1.Items.Add("Rust");
            comboBox1.Items.Add("Fortnite");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if (comboBox1.SelectedIndex == 0)
            {
                WebClient fid = new WebClient();
                fid.Proxy = null;

                Process csgo = Process.GetProcessesByName("csgo").FirstOrDefault();

                if (csgo != null)
                {
                    fid.Headers["User-Agent"] = "Mozilla";
                    byte[] dll = fid.DownloadData(c_api.c_var("special"));
                    var injector = new ManualMapInjector(csgo) { AsyncInjection = true };
                    label1.Text = $"hmodule = 0x{injector.Inject(dll).ToInt64():x8}";
                    MessageBox.Show("Injected Successfully", "apex#8105");
                    Close();
                }
                else
                {
                    MessageBox.Show("Please Open CSGO");
                    Close();
                }

            }
            else if (comboBox1.SelectedIndex == 1)
            {
                MessageBox.Show("Currently in development expect updates soon!", "apex#8105");
                return;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                MessageBox.Show("Currently in development expect updates soon!", "apex#8105");
                return;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                MessageBox.Show("Currently in development expect updates soon!", "apex#8105");
                return;
            }
        }
    }
}
