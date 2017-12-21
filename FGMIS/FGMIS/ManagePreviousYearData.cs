using Domain;
using Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGMIS
{
    public partial class ManagePreviousYearData : Form
    {
        int userAccountsCount = 0;
        List<Organization> organizationsList = new List<Organization>();

        int addStatus = 0;
        int selectedIndex = 1;
        PreviousYear previousYear = null;
        int selectedYear = 2016;

        public ManagePreviousYearData()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GenericHelper helper = new GenericHelper(0);
            string serverAddress = "http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress;
            if (helper.CheckInternet(serverAddress))
            {
                
                int year = Convert.ToInt32(comboBox1.Text);
                int output11 = (int)numericUpDown1.Value;
                int output12 = (int)numericUpDown2.Value;
                int output13 = (int)numericUpDown3.Value;

                int output21 = (int)numericUpDown6.Value;
                int output22 = (int)numericUpDown5.Value;
                int output23 = (int)numericUpDown4.Value;
                int output24 = (int)numericUpDown8.Value;
                int output25 = (int)numericUpDown7.Value;

                int output31 = (int)numericUpDown11.Value;
                int output32 = (int)numericUpDown10.Value;

                previousYear = new PreviousYear();
                previousYear.Year = year;
                previousYear.Output11 = output11;
                previousYear.Output12 = output12;
                previousYear.Output13 = output13;

                previousYear.Output21 = output21;
                previousYear.Output22 = output22;
                previousYear.Output23 = output23;
                previousYear.Output24 = output24;
                previousYear.Output25 = output25;

                previousYear.Output31 = output31;
                previousYear.Output32 = output32;

                previousYear.Uid = Properties.Settings.Default.UID;


                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                statusLabel1.Text = "Adding previous year data - "+year+"...";
                statusLabel1.Visible = true;
                statusProgressBar.Visible = true;
                accountAddBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Previous year data addition failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Previous year data addition Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddAccount()
        {
            PreviousYearDataHelper previousYearDataHelper = new PreviousYearDataHelper(selectedIndex);
            addStatus = previousYearDataHelper.UpdateYearsData(previousYear);
                if (addStatus>0)
                    MessageBox.Show("Data updated successfully! ", "Data updated successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Failed to update data!", "Failed to update data!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
        }

        

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 1;
            selectedIndex = comboBox2.SelectedIndex;
            selectedYear = Convert.ToInt32(comboBox1.Text);
            this.Text = "Manage Previous Year Data";
            statusLabel1.Text = "Loading Previous Year Data";
            statusProgressBar.Visible = true;
            statusLabel1.Visible = true;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            //initialBackgroundWorker.RunWorkerAsync();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            previousYear = new PreviousYear();
            PreviousYearDataHelper previousYearDataHelper = new PreviousYearDataHelper(selectedIndex);
            previousYear=previousYearDataHelper.GetPreviousYearData(selectedYear);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PopulateFields(previousYear);
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            statusLabel1.Visible = false;
            statusProgressBar.Visible = false;
        }

        private void PopulateFields(PreviousYear previousYear)
        {
            numericUpDown1.Value = previousYear.Output11;
            numericUpDown2.Value = previousYear.Output12;
            numericUpDown3.Value = previousYear.Output13;

            numericUpDown6.Value = previousYear.Output21;
            numericUpDown5.Value = previousYear.Output22;
            numericUpDown4.Value = previousYear.Output23;
            numericUpDown8.Value = previousYear.Output24;
            numericUpDown7.Value = previousYear.Output25;

            numericUpDown11.Value = previousYear.Output31;
            numericUpDown10.Value = previousYear.Output32;
        }
        

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            AddAccount();
        }

        private void accountAddBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            initialBackgroundWorker.RunWorkerAsync();
            
        }

        private void SyncUserAccounts()
        {
            OrganizationPartnerHelper userHelper = new OrganizationPartnerHelper();
            List<Organization> usersList = userHelper.GetRemoteOrganizations(OrganizationPartnerHelper.TYPE_ORGANIZATION);
            userAccountsCount = usersList.Count;
            if (usersList.Count > 0)//if we get accounts from the server
            {
                userHelper.EmptyLocalUsersTable(OrganizationPartnerHelper.TYPE_ORGANIZATION);
                userHelper.InsertUsersToLocalDatabase(usersList, OrganizationPartnerHelper.TYPE_ORGANIZATION);
            }
        }

        private void syncBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //SyncUserAccounts();
        }

        private void syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button5.Enabled = true;
            statusLabel1.Visible = false;
            statusProgressBar.Visible = false;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void ResetFields()
        {
            comboBox1.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown10.Value = 0;
            numericUpDown11.Value = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndex = comboBox2.SelectedIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedYear = Convert.ToInt32((sender as ComboBox).Text);


            statusLabel1.Text = "Loading Previous Year Data";
            statusProgressBar.Visible = true;
            statusLabel1.Visible = true;
            button3.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            initialBackgroundWorker.RunWorkerAsync();
        }
        
    }
}
