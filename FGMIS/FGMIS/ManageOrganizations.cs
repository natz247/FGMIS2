﻿using Domain;
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
    public partial class ManageOrganizations : Form
    {
        private Organization organization, newOrganization=null;
        private int bigRemoteId = -1;
        int userAccountsCount = 0;
        int accountDeleteStatus = -1;
        int accountUpdateStatus = -1;
        List<Organization> organizationsList = new List<Organization>();

        string currentOrganization;
        string currentPartner;
        int currentAccessLevel;
        int currentActiveStatus;

        bool addStatus = false;

        private DataTable organizationTable = null, partnerTable = null, accessLevelTable = null, activeStatusTable = null;
        public ManageOrganizations()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GenericHelper helper = new GenericHelper(0);
            string serverAddress = "http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress;
            if (helper.CheckInternet(serverAddress))
            {
                
                string name = (textBox5.Text).Trim();
                string address = (textBox6.Text).Trim();
                string phone1 = (textBox1.Text).Trim();
                string phone2 = (textBox3.Text).Trim();
                string email = (textBox7.Text).Trim();
                string website = (textBox4.Text).Trim();


                organization = new Organization();
                organization.Name = name;
                organization.Address=address;
                organization.Phone1 = phone1;
                organization.Phone2 = phone2;
                organization.Email = email;
                organization.Website = website;
                organization.Uid = Properties.Settings.Default.UID;

                button5.Enabled = false;
                statusLabel1.Text = "Adding organization...";
                statusLabel1.Visible = true;
                statusProgressBar.Visible = true;
                accountAddBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Organization addition failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Organization addition Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddAccount()
        {
            OrganizationPartnerHelper uh = new OrganizationPartnerHelper();
            if (mainFieldsValid())
            {
                if (uh.CheckRemoteOrganization(organization.Name, OrganizationPartnerHelper.TYPE_ORGANIZATION))
                    MessageBox.Show("Organization already exixts with the specified name: " + organization.Name + ". Please choose another organization name.", "Organization name taken!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    addStatus=uh.InsertToRemote(organization, OrganizationPartnerHelper.TYPE_ORGANIZATION);
                    if (addStatus)
                    {
                        
                        MessageBox.Show("Organization added successfully!", "Organization added successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Failed to add organization!", "Failed to add organization!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool mainFieldsValid()
        {

            string name = (textBox5.Text).Trim();
            string address = (textBox6.Text).Trim();
            string phone1 = (textBox1.Text).Trim();
            string phone2 = (textBox3.Text).Trim();
            string email = (textBox7.Text).Trim();

            if (!validator(name, "Name"))
            {
                return false;
            }
            else if (!validator(address, "Address"))
            {
                return false;
            }
            else if (!validator(phone1, "Phone number"))
            {
                return false;
            }
            else if (!validator(email, "Email address"))
            {
                return false;
            }
            
            else
            {
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Please enter a valid " + "Email" + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                    if(!IsValidPhoneNumber(phone1))
                    {
                        MessageBox.Show("Please enter a valid " + "Phone 1" + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (phone2.Length > 0)
                    {
                        if (!IsValidPhoneNumber(phone2))
                        {
                            MessageBox.Show("Please enter a valid " + "Phone 2" + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                        return true;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length >= 10 && phoneNumber.Length <= 13)
                return true;
            else
                return false;
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

        private bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            this.Text = "Manage Organizations";
            statusLabel1.Text = "Loading Organizations";
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

        private void BindComboBox(ComboBox comboBox, DataTable dataTable)
        {
            GenericHelper genericHelper = new GenericHelper();
            //MessageBox.Show(""+genericHelper.GetList("Region").Count);
            comboBox.AutoCompleteMode = AutoCompleteMode.None;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.DataSource = dataTable;
            comboBox.BindingContext = this.BindingContext;
            comboBox.DisplayMember = "title";
            comboBox.ValueMember = "title";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            OrganizationPartnerHelper organizationPartnerHelper = new OrganizationPartnerHelper();
            organizationsList = organizationPartnerHelper.GetRemoteOrganizations(OrganizationPartnerHelper.TYPE_ORGANIZATION);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < organizationsList.Count; i++)
            {
                string[] row = { dataGridView1.Rows.Count + "", organizationsList[i].Name, organizationsList[i].Address, organizationsList[i].Remoteid.ToString() };
                    dataGridView1.Rows.Add(row);
                
            }

            //sync users with local database

            button3.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            syncBackgroundWorker.RunWorkerAsync();
            statusLabel1.Text = "Synchronizing accounts ...";
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
            AddAccount();
        }

        private void accountAddBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(addStatus)
                ResetFields();
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
                bigRemoteId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                if (bigRemoteId == 1 || bigRemoteId == 2)//Save and Norwegian aid
                {
                    MessageBox.Show("Sorry you can not delete this organization.", "Unable to delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this organization?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (confirmResult == DialogResult.Yes)
                    {

                        //MessageBox.Show(bigUserName);
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        statusLabel1.Visible = true;
                        statusLabel1.Text = "Deleting organization...";
                        statusProgressBar.Visible = true;

                        deleteBackgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void deleteBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OrganizationPartnerHelper uh = new OrganizationPartnerHelper();
            accountDeleteStatus = uh.DeleteRemoteOrganization(bigRemoteId, OrganizationPartnerHelper.TYPE_ORGANIZATION);
        }

        private void deleteBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            statusLabel1.Visible = false;
            statusLabel1.Text = "Deleting organization...";
            statusProgressBar.Visible = false;

            if (accountDeleteStatus > 0)
            {
                MessageBox.Show("Organization deleted successfully!", "Successful deletetion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialBackgroundWorker.RunWorkerAsync();
            }
            else
                MessageBox.Show("Failed to delete organization! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Organization deletetion Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            accountDeleteStatus = -1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedIndex=dataGridView1.CurrentCell.RowIndex;
            int remoteId=-1;
            if ((dataGridView1.Rows.Count - 1) != selectedIndex)
            {
                remoteId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                if(organizationsList.Count>0)
                {
                    for (int i = 0; i < organizationsList.Count; i++)
                    {
                        Organization organization = organizationsList[i];
                        if (organization.Remoteid == remoteId)
                            PopulateFields(organization);
                    }

                }
            }
        }

        private void PopulateFields(Organization organization)
        {
            textBox5.Text = organization.Name;
            textBox6.Text = organization.Address;
            textBox1.Text = organization.Phone1;
            textBox3.Text = organization.Phone2;
            textBox7.Text = organization.Email;
            textBox4.Text = organization.Website;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex=dataGridView1.CurrentCell.RowIndex;
            string name;
            if ((dataGridView1.Rows.Count - 1) != selectedIndex)
            {
                name=dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                var confirmResult = MessageBox.Show("Are you sure you want to update "+name+"?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (confirmResult == DialogResult.Yes)
                {
                    if (mainFieldsValid())
                    {
                        string oname = (textBox5.Text).Trim()+"";
                        string address = (textBox6.Text).Trim() + "";
                        string phone1 = (textBox1.Text).Trim() + "";
                        string phone2 = (textBox3.Text).Trim() + "";
                        string email = (textBox7.Text).Trim() + "";
                        string website = (textBox4.Text).Trim() + "";


                        newOrganization = new Organization();
                        newOrganization.Name = oname;
                        newOrganization.Address = address;
                        newOrganization.Phone1 = phone1;
                        newOrganization.Phone2 = phone2;
                        newOrganization.Email = email;
                        newOrganization.Website = website;

                        bigRemoteId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                        //MessageBox.Show(bigUserName);
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        statusLabel1.Visible = true;
                        statusLabel1.Text = "Updating account...";
                        statusProgressBar.Visible = true;

                        updateBackgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void updateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if(newOrganization!=null)
            {
                OrganizationPartnerHelper uh = new OrganizationPartnerHelper();
                accountUpdateStatus = uh.UpdateRemoteOrganization(bigRemoteId, newOrganization, OrganizationPartnerHelper.TYPE_ORGANIZATION);
            }
        }

        private void updateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            statusLabel1.Visible = false;
            statusLabel1.Text = "Updating organization...";
            statusProgressBar.Visible = false;

            if (accountUpdateStatus > 0)
            {
                ResetFields();
                
                MessageBox.Show("Organization updated successfully!", "Successful update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialBackgroundWorker.RunWorkerAsync();
            }
            else
                MessageBox.Show("Failed to update organization! There is no internet connection or there is a problem with the server address. Please check your server settings.", "User deletetion Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            accountUpdateStatus = -1;
        }

        private void ResetFields()
        {
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox1.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox7.Text = string.Empty;
            textBox4.Text = string.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ResetFields();
        }


        
    }
}
