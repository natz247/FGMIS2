using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class ServerAddress : Form
    {
        public ServerAddress()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ServerAddress_Load(object sender, EventArgs e)
        {
            textBox1.Text = Session.Properties.Settings.Default.RemoteDatabaseAddress;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string serverAddress = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(serverAddress))
            {
                SaveServerAddress();
                timerDelay("Settings saved!");
            }
        }

        private void SaveServerAddress()
        {
            Session.Properties.Settings.Default.RemoteDatabaseAddress = textBox1.Text;
            Session.Properties.Settings.Default.Save();
        }

        private async Task timerDelay(string message)
        {

            panel1.Visible = true;
            label6.Text = message;
            await Task.Delay(1000);
            panel1.Visible = false;
            this.Close();
        }

        private async Task timerDelay2(Color color, string message)
        {
            panel1.Visible = true;
            label6.BackColor = color;
            panel1.BackColor = color;
            label6.Text = message;
            await Task.Delay(2000);
            panel1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox2.Text+textBox1.Text.Trim();
            if(!string.IsNullOrEmpty(url))
            {
                //SaveServerAddress();
                GenericHelper helper = new GenericHelper(0);
                if (helper.CheckInternet(url))
                    timerDelay2(Color.Chartreuse, "Successful connection!");
                else
                    timerDelay2(Color.Crimson, "Connection test failed!");
            }
                else
                {

                }
        }
    }
}
