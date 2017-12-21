using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using Session;
using Domain;
using System.Collections.Generic;

namespace FGMIS
{
    public partial class Login : Form
    {
        private UserHelper uh;
        private User user;
        int userAccountsCount = 0;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //panel3.Visible = true;
            authenticateLabel.Visible = true;
            authenticateProgressBar.Visible = true;
            button4.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            
        }

        private void ResetFields()
        {
            textBox1.Text=string.Empty;
            textBox2.Text = string.Empty;
            this.ActiveControl = textBox1;
        }
        
        private void Login_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FIRSTRUN == true)
            {
                SyncUsers();
                Properties.Settings.Default.FIRSTRUN = false;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ServerAddress serverAddress = new ServerAddress();
            serverAddress.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            uh = new UserHelper();
            user = uh.GetUser(textBox1.Text, StringCipher.Encrypt(textBox2.Text));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //panel3.Visible = false;
            authenticateProgressBar.Visible = false;
            authenticateLabel.Visible = false;
            button4.Enabled = true;

            if (user != null)
            {
                Properties.Settings.Default.USERNAME = user.UserName;
                Properties.Settings.Default.USERPASSWORD = user.Password;
                //UID should be remote id after syncing
                Properties.Settings.Default.UID = user.Remoteid;
                Properties.Settings.Default.USERFIRSTNAME = user.FirstName;
                Properties.Settings.Default.USERLASTNAME = user.LastName;
                Properties.Settings.Default.USERACCESSLEVEL = user.AccessLevel;
                Properties.Settings.Default.USERPOSITION = uh.GetUserPosition(user.AccessLevel);
                Properties.Settings.Default.USERORGANIZATION=user.Organization;
                Properties.Settings.Default.USERPARTNER=user.Partner;
                Properties.Settings.Default.Save();
                ResetFields();
                Main mainWindow = new Main(this);
                mainWindow.Show();
                //mainWindow.Parent = this;
                this.Hide();
            }
            else
            {
                MessageBox.Show("User name and or password incorrect!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ManageUsers manageUsers = new ManageUsers();
            manageUsers.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            SyncUsers();
        }

        private void SyncUsers()
        {
            GenericHelper helper = new GenericHelper(0);
            string serverAddress = "http://" + Session.Properties.Settings.Default.RemoteDatabaseAddress;
            if (helper.CheckInternet(serverAddress))
            {
                authenticateLabel.Visible = true;
                authenticateProgressBar.Visible = true;
                button1.Enabled = false;
                authenticateLabel.Text = "Syncing user accounts...";
                backgroundWorker2.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Synchronization failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Synchronization Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SyncUserAccounts();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            authenticateLabel.Visible = false;
            authenticateProgressBar.Visible = false;
            button1.Enabled = true;
            if (userAccountsCount>0)
                MessageBox.Show("Successful synchronization!", "Successful synchronization", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Synchronization failed! There is no internet connection or there is a problem with the server address. Please check your server settings.", "Synchronization Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
