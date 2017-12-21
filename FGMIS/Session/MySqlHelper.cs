using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.Net.NetworkInformation;
using System.Data;
using MySql.Data.MySqlClient;

namespace Session
{
    public class MySqlHelper
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public MySqlConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }
        
        private void ConnectTo()
        {
            server = Properties.Settings.Default.RemoteDatabaseAddress;
            database= Properties.Settings.Default.RemoteDatabaseName;
            uid= Properties.Settings.Default.RemoteDatabaseUserName;
            password= Properties.Settings.Default.RemoteDatabasePassword;

            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        public MySqlHelper()
        {
            ConnectTo();
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


    }
}
