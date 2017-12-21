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
    public partial class Loading : Form
    {
        public Loading(string loadingMessage)
        {
            InitializeComponent();
            label1.Text = loadingMessage;
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }
    }
}
