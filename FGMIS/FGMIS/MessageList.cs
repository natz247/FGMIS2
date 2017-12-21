using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class MessageList : UserControl
    {
        public MessageList()
        {
            InitializeComponent();
        }

        private void MessageList_Load(object sender, EventArgs e)
        {

        }

        public void SetSender(string name)
        {
            label1.Text = name;
        }
        public void SetSubject(string subject)
        {
            label2.Text = subject;
        }
        public void SetDate(string date)
        {
            label3.Text = date;
        }

        public void SetMessageStatus(bool status)
        {
            if(status)
            {
                pictureBox1.Image = Properties.Resources.unread;
                pictureBox1.Invalidate();
            }
            else
            {
                pictureBox1.Image = Properties.Resources.read;
                pictureBox1.Invalidate();
            }
        }
        public void SetBackColor(Color color)
        {
            this.BackColor = color;
        }
    }
}
