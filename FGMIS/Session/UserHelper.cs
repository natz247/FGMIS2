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
    public class UserHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command=connection.CreateCommand();
        }

        public UserHelper()
        {
            ConnectTo();
        }

        public void Insert(User user)
        {
            try
            {
                OleDbCommand myCommand = connection.CreateCommand();
                myCommand.CommandText = "INSERT INTO Users ([firstname], [lastname], [username], [password], [accesslevel], [active_status], [localtimestamp], [mac], [remoteid], [remotetimestamp], [sync_status], [email], [phone], [organization], [partner], [uid]) VALUES(@FirstName, @LastName, @UserName, @Password, @AccessLevel, @ActiveStatus, @LocalTimeStamp, @Mac, @RemoteId, @RemoteTimeStamp, @SyncStatus, @Email, @Phone, @Organization, @Partner, @uid)";
                myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                myCommand.Parameters.AddWithValue("@UserName", user.UserName);
                myCommand.Parameters.AddWithValue("@Password", user.Password);
                myCommand.Parameters.AddWithValue("@AccessLevel", user.AccessLevel);
                myCommand.Parameters.AddWithValue("@AtiveStatus", user.ActiveStatus);
                myCommand.Parameters.AddWithValue("@LocalTimeStamp", user.LocalTimeStamp);
                myCommand.Parameters.AddWithValue("@Mac", user.Mac);
                myCommand.Parameters.AddWithValue("@RemoteId", user.Remoteid);
                myCommand.Parameters.AddWithValue("@RemoteTimeStamp", user.RemoteTimeStamp);
                myCommand.Parameters.AddWithValue("@SyncStatus", user.SyncStatus);
                myCommand.Parameters.AddWithValue("@Email", user.Email);
                myCommand.Parameters.AddWithValue("@Phone", user.Phone);
                myCommand.Parameters.AddWithValue("@Organization", user.Organization);
                myCommand.Parameters.AddWithValue("@Partner", user.Partner);
                myCommand.Parameters.AddWithValue("@uid", user.Uuid);
                myCommand.CommandType = CommandType.Text;
                connection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(connection!=null)
                {
                    connection.Close();
                }
            }
        }



        public Boolean CheckUser(string username, string password)
        {
            try
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@UserName AND password=@Password";
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                int numberOfUsers = (int)command.ExecuteScalar();
                if (numberOfUsers >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public bool IsUserOfOrganization(int userId, string organization, string partner)
        {
            try
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM Users WHERE [remoteid]=@UserId AND [organization]=@Organization AND [partner]=@Partner";
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Organization", organization);
                command.Parameters.AddWithValue("@Partner", partner);       

                int numberOfUsers = (int)command.ExecuteScalar();
                if (numberOfUsers >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        

        public User GetUser(string username, string password)
        {
            User user = null;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Users WHERE username=@UserName AND password=@Password";
            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@Password", password); 
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                user = new User();
                user.Uid = Convert.ToInt32(dr[0].ToString());
                user.FirstName = dr[1].ToString();
                user.LastName = dr[2].ToString();
                user.UserName = username;
                user.Password = StringCipher.Decrypt(password);
                user.AccessLevel = Convert.ToInt32(dr[5].ToString());
                user.ActiveStatus = Convert.ToInt32(dr[6].ToString());
                user.LocalTimeStamp = dr.GetDateTime(7);
                user.Mac = dr[8].ToString();
                user.Remoteid = Convert.ToInt32(dr[9].ToString());
                user.RemoteTimeStamp = dr.GetDateTime(10);
                user.SyncStatus = Convert.ToInt32(dr[11].ToString());
                user.Email = dr[12].ToString();
                user.Phone = dr[13].ToString();
                user.Organization = dr[14].ToString();
                user.Partner = dr[15].ToString();
                user.Uuid = Convert.ToInt32(dr[16].ToString());

            }
            connection.Close();
            dr.Close();
            return user;
        }

        public User GetUser(int remoteid)
        {
            User user = null;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM Users WHERE remoteid=@RemoteId";
            command.Parameters.AddWithValue("@RemoteId", remoteid);
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                user = new User();
                user.Uid = Convert.ToInt32(dr[0].ToString());
                user.FirstName = dr[1].ToString();
                user.LastName = dr[2].ToString();
                user.UserName = dr[3].ToString();
                user.Password = StringCipher.Decrypt(dr[4].ToString());
                user.AccessLevel = Convert.ToInt32(dr[5].ToString());
                user.ActiveStatus = Convert.ToInt32(dr[6].ToString());
                user.LocalTimeStamp = dr.GetDateTime(7);
                user.Mac = dr[8].ToString();
                user.Remoteid = Convert.ToInt32(dr[9].ToString());
                user.RemoteTimeStamp = dr.GetDateTime(10);
                user.SyncStatus = Convert.ToInt32(dr[11].ToString());
                user.Email = dr[12].ToString();
                user.Phone = dr[13].ToString();
                user.Organization = dr[14].ToString();
                user.Partner = dr[15].ToString();
                user.Uuid = Convert.ToInt32(dr[16].ToString());

            }
            connection.Close();
            dr.Close();
            return user;
        }

        public string GetUserPosition(int accessLevel)
        {
            string position = null;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM AccessLevel WHERE accesslevel=@AccessLevel";
            command.Parameters.AddWithValue("@AccessLevel", accessLevel);
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                position = dr[1].ToString();

            }
            connection.Close();
            dr.Close();
            return position;
        }

        public static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    sMacAddress = adapter.GetPhysicalAddress().ToString();

                }
            }
            return sMacAddress;
        }

        public void EmptyLocalUsersTable()
        {
            string tableName = "users";
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "DELETE FROM "+tableName;
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void InsertUsersToLocalDatabase(List<User> usersList)
        {
            for(int i=0; i<usersList.Count; i++)
            {
                User user=(User)usersList[i];
                Insert(user);
            }
        }

        public List<User> GetRemoteUsers()
        {
            List<User> usersList = new List<User>();
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            string tableName = "users";
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;

                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT * FROM " + tableName;

                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    User user = new User();
                    //user.Uid = Convert.ToInt32(dr[0].ToString());
                    user.FirstName = dr[1].ToString();
                    user.LastName = dr[2].ToString();
                    user.UserName = dr[3].ToString();
                    user.Password = dr[4].ToString();
                    user.AccessLevel = Convert.ToInt32(dr[5].ToString());
                    user.ActiveStatus = Convert.ToInt32(dr[6].ToString());
                    user.LocalTimeStamp = dr.GetDateTime(7);
                    user.Mac = dr[8].ToString();
                    user.Remoteid = Convert.ToInt32(dr[0].ToString());
                    user.RemoteTimeStamp = dr.GetDateTime(10);
                    user.SyncStatus = Convert.ToInt32(dr[11].ToString());
                    user.Email= dr[12].ToString();
                    user.Phone = dr[13].ToString();
                    user.Organization = dr[14].ToString();
                    user.Partner = dr[15].ToString();
                    user.Uuid = Convert.ToInt32(dr[16].ToString());

                    usersList.Add(user);
                }
                dr.Close();
                mySqlHelper.CloseConnection();
            }
            return usersList;

        }

        public bool InsertToRemote(User user)
        {
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            bool insertStatus=false;
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;
                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "INSERT INTO Users (`firstname`, `lastname`, `username`, `password`, `accesslevel`, `active_status`, `localtimestamp`, `mac`, `remotetimestamp`, `email`, `phone`, `organization`, `partner`, `uid`) VALUES(@FirstName, @LastName, @UserName, @Password, @AccessLevel, @ActiveStatus, '" + user.LocalTimeStamp.ToString("yyyy-MM-dd H:mm:ss") + "', '" + GetMacAddress() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', @Email, @Phone, @Organization, @Partner, @uid)";
                    myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    myCommand.Parameters.AddWithValue("@UserName", user.UserName);
                    myCommand.Parameters.AddWithValue("@Password", StringCipher.Encrypt(user.Password));
                    myCommand.Parameters.AddWithValue("@AccessLevel", user.AccessLevel);
                    myCommand.Parameters.AddWithValue("@ActiveStatus", user.ActiveStatus);
                    //myCommand.Parameters.AddWithValue("@RemoteId", user.Remoteid);
                    //myCommand.Parameters.AddWithValue("@RemoteTimeStamp", user.RemoteTimeStamp);
                    //myCommand.Parameters.AddWithValue("@SyncStatus", user.SyncStatus);
                    myCommand.Parameters.AddWithValue("@Email", user.Email);
                    myCommand.Parameters.AddWithValue("@Phone", user.Phone);
                    myCommand.Parameters.AddWithValue("@Organization", user.Organization);
                    myCommand.Parameters.AddWithValue("@Partner", user.Partner);
                    myCommand.Parameters.AddWithValue("@uid", user.Uuid);
                    myCommand.CommandType = CommandType.Text;
                    myCommand.ExecuteNonQuery();
                    insertStatus = true;
                }
                else
                {
                    insertStatus = false;
                }
            }
            catch (Exception)
            {
                insertStatus = false;
                throw;
            }
            finally
            {
                if (mySqlHelper != null)
                {
                    mySqlHelper.CloseConnection();
                }
            }
            return insertStatus;
        }

        public Boolean CheckRemoteUser(string username)
        {
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            string tableName = "users";
            bool userExists = false;
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;

                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "SELECT COUNT(*) FROM Users WHERE `username`=@UserName";
                    myCommand.Parameters.AddWithValue("@UserName", username);

                    var result = myCommand.ExecuteScalar();
                    int numberOfUsers = Convert.ToInt32(result);
                    if (numberOfUsers >= 1)
                        userExists = true;
                    else
                        userExists = false;
                }
                return userExists;
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
            finally
            {
                if (mySqlHelper != null)
                {
                    mySqlHelper.CloseConnection();
                }
            }
        }

        public int DeleteRemoteUser(string userName)
        {
            int resultValue = -1;
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;

                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "DELETE FROM Users WHERE `username`=@UserName";
                    myCommand.Parameters.AddWithValue("@UserName", userName);

                    resultValue = myCommand.ExecuteNonQuery();
                    mySqlHelper.CloseConnection();

                }

            }
            catch
            {
                throw;
            }

            return resultValue;
        }
        
        public int UpdateRemoteUser(string userName, string organization, string partner, int accessLevel, int activeStatus)
        {
            int resultValue = -1;
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;

                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "UPDATE Users SET `organization`=@Organization, `partner`=@Partner, `accesslevel`=@AccessLevel, `active_status`=@ActiveStatus WHERE `username`=@UserName";
                    myCommand.Parameters.AddWithValue("@Organization", organization); 
                    myCommand.Parameters.AddWithValue("@Partner", partner);
                    myCommand.Parameters.AddWithValue("@AccessLevel", accessLevel);
                    myCommand.Parameters.AddWithValue("@ActiveStatus", activeStatus);
                    myCommand.Parameters.AddWithValue("@UserName", userName);

                    resultValue = myCommand.ExecuteNonQuery();
                    mySqlHelper.CloseConnection();

                }

            }
            catch
            {
                throw;
            }

            return resultValue;
        }
    }
}
