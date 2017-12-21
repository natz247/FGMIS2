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
    public partial class ManageLocations : Form
    {
        private Location location=null;
        private int bigRemoteId = -1;
        int userAccountsCount = 0;
        int accountDeleteStatus = -1;
        List<Location> locationsList = new List<Location>();

        string _location;

        int addStatus = -1;

        public ManageLocations(string _location)
        {
            InitializeComponent();
            this._location = _location;
        }

        private void button5_Click(object sender, EventArgs e)
        {
              
                string name = (textBox5.Text).Trim();


                location = new Location(name, Properties.Settings.Default.UID);

                button5.Enabled = false;
                statusLabel1.Text = "Adding location...";
                statusLabel1.Visible = true;
                statusProgressBar.Visible = true;
                accountAddBackgroundWorker.RunWorkerAsync();
           
        }

        

        private bool mainFieldsValid()
        {

            string name = (textBox5.Text).Trim();

            if (!validator(name, "Name"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        

        private Boolean validator(string text, string errorMessage)
        {
            //string thisText = text.Trim();
            if (text.CompareTo(string.Empty) == 0)
            {
                MessageBox.Show("Please enter a valid " + errorMessage + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }

       
        private void ManageUsers_Load(object sender, EventArgs e)
        {
            this.Text = "Manage " + _location + "s";
            label1.Text = "Manage " + _location + "s";
            groupBox2.Text = "Add / Edit " + _location;
            groupBox1.Text = "Current " + _location+"s";
            statusLabel1.Text = "Loading "+_location+"s";
            statusProgressBar.Visible = true;
            statusLabel1.Visible = true;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            initialBackgroundWorker.RunWorkerAsync();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            locationsList = genericHelper.GetLocationList(_location);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < locationsList.Count; i++)
            {
                string[] row = { dataGridView1.Rows.Count + "", locationsList[i].Name, locationsList[i].Lid.ToString() };
                    dataGridView1.Rows.Add(row);
                
            }

            //sync users with local database

            button3.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            syncBackgroundWorker.RunWorkerAsync();
            statusLabel1.Text = "Synchronizing locations ...";
            statusProgressBar.Visible = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar!='+'))
            {
                e.Handled=true;
            }
            //only allow one +
            if((e.KeyChar=='+') &&((sender as TextBox).Text.IndexOf('+')>-1))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar == ' '))
            {
                e.Handled = true;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper helper = new GenericHelper();
            addStatus=helper.Insert(_location, location.Name, location.Uid);
        }

        private void accountAddBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (addStatus > 0)
            {
                ResetFields();
                MessageBox.Show("Location added successfully!", "Successful addition", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            //update gridview
            dataGridView1.Rows.Clear();
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
            SyncUserAccounts();
        }

        private void syncBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button5.Enabled = true;
            statusLabel1.Visible = false;
            statusProgressBar.Visible = false;
            button3.Enabled = true;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            //if (userAccountsCount > 0)
                //MessageBox.Show("Successful synchronization!", "Successful synchronization", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //else
               // MessageBox.Show("Synchronization failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Synchronization Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if ((dataGridView1.Rows.Count - 1) != dataGridView1.CurrentCell.RowIndex)
            {
                bigRemoteId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
                
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this "+_location+"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (confirmResult == DialogResult.Yes)
                    {

                        //MessageBox.Show(bigUserName);
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        statusLabel1.Visible = true;
                        statusLabel1.Text = "Deleting location...";
                        statusProgressBar.Visible = true;

                        deleteBackgroundWorker.RunWorkerAsync();
                    }
                
            }
        }

        private void deleteBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GenericHelper genericHelper = new GenericHelper();
            accountDeleteStatus=genericHelper.DeleteLocation(_location, bigRemoteId);
        }

        private void deleteBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            statusLabel1.Visible = false;
            statusLabel1.Text = "Deleting location...";
            statusProgressBar.Visible = false;

            if (accountDeleteStatus > 0)
            {
                MessageBox.Show("Location deleted successfully!", "Successful deletetion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialBackgroundWorker.RunWorkerAsync();
            }
            else
                MessageBox.Show("Failed to delete location! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Organization deletetion Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            accountDeleteStatus = -1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void PopulateFields(Location location)
        {
            textBox5.Text = location.Name;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void updateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void updateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void ResetFields()
        {
            textBox5.Text = string.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFields();
        }


        
    }
}
