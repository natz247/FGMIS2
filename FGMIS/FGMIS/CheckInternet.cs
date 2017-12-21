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
    public partial class CheckInternet : Form
    {
        private Form callingForm;
        private bool internetStatus;
        public CheckInternet(Form callingForm)
        {
            InitializeComponent();
            this.callingForm = callingForm;
        }

        private void CheckInternet_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            GenericHelper genericHelper = new GenericHelper();
            internetStatus = genericHelper.CheckInternet("http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress);
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!internetStatus)
            {
                MessageBox.Show("Unable to reach remote server. Please check your internet connection or server settings.", "Unable to reach remote server!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            else
            {
                callingForm.ShowDialog();
                this.Close();
            }
        }
    }
}
