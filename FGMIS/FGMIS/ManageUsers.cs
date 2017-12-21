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
    public partial class ManageUsers : Form
    {
        private User user;
        private string bigUserName = null;
        int userAccountsCount = 0;
        int accountDeleteStatus = -1;
        int accountUpdateStatus = -1;
        List<User> usersList = new List<User>();

        List<string> partnerNames = new List<string>();
        List<int> partnerOrganizationIds = new List<int>();

        string currentOrganization;
        string currentPartner;
        int currentAccessLevel;
        int currentActiveStatus;
        bool addStatus=false;

        private DataTable organizationTable = null, partnerTable = null, accessLevelTable = null, activeStatusTable = null;
        public ManageUsers()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GenericHelper helper = new GenericHelper(0);
            string serverAddress = "http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress;
            if (helper.CheckInternet(serverAddress))
            {
                string userName = (textBox2.Text).Trim();
                
                string firstName = (textBox5.Text).Trim();
                string lastName = (textBox1.Text).Trim();
                string password = (textBox3.Text).Trim();
                string email = (textBox4.Text).Trim();
                string phone = (textBox6.Text).Trim();

                string organization = comboBox7.Text;
                string partner = comboBox1.Text;
                int accessLevel = comboBox3.SelectedIndex;
                int activeStatus = comboBox2.SelectedIndex;

                user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.UserName = userName;
                user.Password = password;
                user.Email = email;
                user.Phone = phone;
                user.Organization = organization;
                user.Partner = partner;
                user.AccessLevel = accessLevel;
                user.ActiveStatus = activeStatus;
                user.Uuid = Properties.Settings.Default.UID;

                button5.Enabled = false;
                statusLabel1.Text = "Adding account...";
                statusLabel1.Visible = true;
                statusProgressBar.Visible = true;
                accountAddBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Account addition failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Account Additions Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddAccount()
        {
            UserHelper uh = new UserHelper();
            if (mainFieldsValid())
            {
                if (uh.CheckRemoteUser(user.UserName))
                    MessageBox.Show("Account already exixts with the specified user name: " + user.UserName + ". Please choose another user name.", "User name taken!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    addStatus = uh.InsertToRemote(user);
                    if (addStatus)
                    {
                        MessageBox.Show("Account added successfully!", "Account added successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Failed to add account!", "Failed to add account!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool mainFieldsValid()
        {

            string firstName = (textBox5.Text).Trim();
            string lastName = (textBox1.Text).Trim();
            string userName = (textBox2.Text).Trim();
            string password = (textBox3.Text).Trim();
            string password2 = (textBox7.Text).Trim();
            string email = (textBox4.Text).Trim();
            string phone = (textBox6.Text).Trim();

            if (!validator(firstName, "First Name"))
            {
                return false;
            }
            else if (!validator(lastName, "Last Name"))
            {
                return false;
            }
            else if (!validator(userName, "User Name"))
            {
                return false;
            }
            else if (!validator(password, "Password"))
            {
                return false;
            }
            else if (!validator(password2, "Confirmation Password"))
            {
                return false;
            }
            else if (!validator(email, "Email"))
            {
                return false;
            }
            else if (!validator(phone, "Phone"))
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
                    if(!IsValidPhoneNumber(phone))
                    {
                        MessageBox.Show("Please enter a valid " + "Phone" + "!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                        if(password.CompareTo(password2)!=0)//if the two passwords do not match
                        {
                            MessageBox.Show("Please enter matching passwords!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
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
            comboBox2.SelectedIndex = 1;

            statusLabel1.Text = "Loading Users";
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

        private void BindComboBox2(ComboBox comboBox, DataTable dataTable)
        {
            GenericHelper genericHelper = new GenericHelper();
            //MessageBox.Show(""+genericHelper.GetList("Region").Count);
            comboBox.AutoCompleteMode = AutoCompleteMode.None;
            comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox.DataSource = dataTable;
            comboBox.BindingContext = this.BindingContext;
            comboBox.DisplayMember = "title";
            comboBox.ValueMember = "ID";
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            UserHelper userHelper = new UserHelper();
            usersList = userHelper.GetRemoteUsers();


            GenericHelper genericHelper = new GenericHelper();

            organizationTable = genericHelper.GetRemoteList("Organization");
            partnerTable = genericHelper.GetRemoteList("Partner");
            accessLevelTable = genericHelper.GetRemoteList("AccessLevel");

            partnerNames = partnerTable.AsEnumerable().Select(x => x[1].ToString()).ToList();
            partnerOrganizationIds = partnerTable.AsEnumerable().Select(x => Convert.ToInt32(x[13].ToString())).ToList();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindComboBox(comboBox7, organizationTable);
            BindComboBox(comboBox1, partnerTable);
            BindComboBox(comboBox3, accessLevelTable);
            BindComboBox2(comboBox4, organizationTable);

            PopulatePartners(comboBox7);

            ////statusProgressBar.Visible = false;
            ////statusLabel1.Visible = false;
            //////groupBox1.Enabled = true;
            //groupBox2.Enabled = true;
            dataGridView1.Rows.Clear();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (Properties.Settings.Default.USERNAME.ToLower().CompareTo(usersList[i].UserName.ToLower()) != 0)//exclude logged in account
                {
                    string[] row = { dataGridView1.Rows.Count + "", usersList[i].FirstName + " " + usersList[i].LastName, usersList[i].UserName };
                    dataGridView1.Rows.Add(row);
                }
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
            if (addStatus)
                ResetFields();
            //update gridview
            dataGridView1.Rows.Clear();
            initialBackgroundWorker.RunWorkerAsync();
            
        }

        private void SyncUserAccounts()
        {
            UserHelper userHelper = new UserHelper();
            List<User> usersList = userHelper.GetRemoteUsers();
            userAccountsCount = usersList.Count;
            if (usersList.Count > 0)//if we get accounts from the server
            {
                userHelper.EmptyLocalUsersTable();
                userHelper.InsertUsersToLocalDatabase(usersList);
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
                var confirmResult = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (confirmResult == DialogResult.Yes)
                {
                    bigUserName = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    //MessageBox.Show(bigUserName);
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    statusLabel1.Visible = true;
                    statusLabel1.Text = "Deleting account...";
                    statusProgressBar.Visible = true;

                    deleteBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void deleteBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UserHelper uh = new UserHelper();
            accountDeleteStatus=uh.DeleteRemoteUser(bigUserName);
        }

        private void deleteBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            statusLabel1.Visible = false;
            statusLabel1.Text = "Deleting account...";
            statusProgressBar.Visible = false;

            if (accountDeleteStatus > 0)
            {
                MessageBox.Show("User deleted successfully!", "Successful deletetion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialBackgroundWorker.RunWorkerAsync();
            }
            else
                MessageBox.Show("Failed to delete user! There is no internet connection or there is a problem with the server address. Please check your server settings.", "User deletetion Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            accountDeleteStatus = -1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedIndex=dataGridView1.CurrentCell.RowIndex;
            string userName;
            if ((dataGridView1.Rows.Count - 1) != selectedIndex)
            {
                userName=dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                if(usersList.Count>0)
                {
                    for(int i=0; i<usersList.Count; i++)
                    {
                        User user = usersList[i];
                        if (userName.CompareTo(user.UserName) == 0)
                            PopulateFields(user);
                    }

                }
            }
        }

        private void PopulateFields(User user)
        {
            textBox5.Text=user.FirstName;
            textBox1.Text = user.LastName;
            textBox2.Text = user.UserName;
            textBox3.Text = user.Password;
            textBox6.Text = user.Phone;
            textBox4.Text = user.Email;

            comboBox7.SelectedValue = user.Organization;
            //comboBox1.SelectedValue = user.Partner;
            comboBox1.SelectedText = user.Partner;
            comboBox3.SelectedIndex = user.AccessLevel;
            comboBox2.SelectedIndex = user.ActiveStatus;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedIndex=dataGridView1.CurrentCell.RowIndex;
            string userName, name;
            if ((dataGridView1.Rows.Count - 1) != selectedIndex)
            {
                userName = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                name=dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                var confirmResult = MessageBox.Show("Are you sure you want to update "+name+"\'s account?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (confirmResult == DialogResult.Yes)
                {
                    bigUserName = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    //MessageBox.Show(bigUserName);
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    statusLabel1.Visible = true;
                    statusLabel1.Text = "Updating account...";
                    statusProgressBar.Visible = true;

                    currentOrganization=comboBox7.SelectedValue.ToString();
                    currentPartner=comboBox1.SelectedValue.ToString();
                    currentAccessLevel = comboBox3.SelectedIndex;
                    currentActiveStatus=comboBox2.SelectedIndex;

                     updateBackgroundWorker.RunWorkerAsync();
                }
            }
        }

        private void updateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UserHelper uh = new UserHelper();
            accountUpdateStatus = uh.UpdateRemoteUser(bigUserName, currentOrganization, currentPartner, currentAccessLevel, currentActiveStatus);
        }

        private void updateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            statusLabel1.Visible = false;
            statusLabel1.Text = "Updating account...";
            statusProgressBar.Visible = false;

            if (accountUpdateStatus > 0)
            {
                ResetFields();
                MessageBox.Show("User updated successfully!", "Successful update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                initialBackgroundWorker.RunWorkerAsync();
            }
            else
                MessageBox.Show("Failed to update user! There is no internet connection or there is a problem with the server address. Please check your server settings.", "User deletetion Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
            textBox2.Text = string.Empty;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePartners((sender as ComboBox));
        }

        private void PopulatePartners(ComboBox sender)
        {
            comboBox4.SelectedIndex = (sender as ComboBox).SelectedIndex;
            if (comboBox4.SelectedValue != null)
            {
                int selectedId = Convert.ToInt32(comboBox4.SelectedValue.ToString());
                List<string> newPartnerNames = new List<string>();
                newPartnerNames.Add("Own");
                for (int i = 0; i < partnerOrganizationIds.Count; i++)
                {
                    if (selectedId == partnerOrganizationIds[i])
                    {
                        newPartnerNames.Add(partnerNames[i]);
                    }
                }
                comboBox1.DataSource = newPartnerNames;
            }
        }

        
    }
}
