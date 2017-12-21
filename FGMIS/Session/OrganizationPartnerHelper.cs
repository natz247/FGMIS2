using Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class OrganizationPartnerHelper
    {
        OleDbConnection connection;
        OleDbCommand command;
        public static int TYPE_ORGANIZATION = 0;
        public static int TYPE_PARTNER = 1;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command=connection.CreateCommand();
        }

        public OrganizationPartnerHelper()
        {
            ConnectTo();
        }

        public int Insert(Organization organization, int type)
        {
            int result = -1 ;
            string tableName = "Organization";
            if(type==TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";
            try
            {
                string phone2="";
                string website="";
                string firstStage = "INSERT INTO " + tableName + " ([title], [address], [phone1], [phone2], [email], [web], [localtimestamp], [mac], [sync_status], [uid]";
                string secondStage=" VALUES(@title, @address, @phone1, @phone2, @email, @web, '" + DateTime.Now + "', @mac, @sync_status, @uid";
                if (type == TYPE_PARTNER)
                {
                    firstStage += ", [organizationid])";
                    secondStage += ", @organizationid)";
                }
                else
                {
                    firstStage += ")";
                    secondStage += ")";
                }
                OleDbCommand myCommand = connection.CreateCommand();
                myCommand.CommandText = firstStage+secondStage;
                myCommand.Parameters.AddWithValue("@title", organization.Name);
                myCommand.Parameters.AddWithValue("@address", organization.Address);
                myCommand.Parameters.AddWithValue("@phone1", organization.Phone1);
                if (organization.Phone2 != null)
                    phone2 = organization.Phone2;
                myCommand.Parameters.AddWithValue("@phone2", phone2);
                myCommand.Parameters.AddWithValue("@email", organization.Email);
                if (organization.Website != null)
                    website = organization.Website;
                myCommand.Parameters.AddWithValue("@web", website);
                myCommand.Parameters.AddWithValue("@mac", GetMacAddress());
                myCommand.Parameters.AddWithValue("@sync_status", 0);
                myCommand.Parameters.AddWithValue("@uid", organization.Uid);
                if (type == TYPE_PARTNER)
                {
                    myCommand.Parameters.AddWithValue("@organizationid", ((Partner)organization).Organizationid);
                }
                myCommand.CommandType = CommandType.Text;
                connection.Open();
                result=myCommand.ExecuteNonQuery();
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
            return result;
        }

        public Boolean CheckOrganization(string name, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";
            try
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM "+tableName +" WHERE title=@title";
                command.Parameters.AddWithValue("@title", name);

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

        public Organization GetOrganization(int remoteid, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

            Organization organization = null;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM "+tableName+" WHERE remoteid=@RemoteId";
            command.Parameters.AddWithValue("@RemoteId", remoteid);
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (type == TYPE_PARTNER)
                    organization = new Partner();
                else
                    organization = new Organization();

                organization.Oid = Convert.ToInt32(dr[0].ToString());
                organization.Name = dr[1].ToString();
                organization.Address = dr[2].ToString();
                organization.Phone1 = dr[3].ToString();
                organization.Phone2 = dr[4].ToString();
                organization.Email = dr[5].ToString();
                organization.Website = dr[6].ToString();
                organization.LocalTimeStamp = dr.GetDateTime(7);
                organization.Mac = dr[8].ToString();
                organization.Remoteid = Convert.ToInt32(dr[9].ToString());
                organization.RemoteTimeStamp = dr.GetDateTime(10);
                organization.SyncStatus = Convert.ToInt32(dr[11].ToString());
                organization.Uid = Convert.ToInt32(dr[12].ToString());
                if (type == TYPE_PARTNER)
                    ((Partner)organization).Organizationid = Convert.ToInt32(dr[13].ToString());
            }
            connection.Close();
            dr.Close();
            return organization;
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
        public void EmptyLocalUsersTable(int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "DELETE FROM " + tableName;
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        public void InsertUsersToLocalDatabase(List<Organization> organizationsList, int type)
        {
            for (int i = 0; i < organizationsList.Count; i++)
            {
                Organization organization = (Organization)organizationsList[i];
                Insert(organization, type);
            }
        }

        public List<Organization> GetRemoteOrganizations(int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

            List<Organization> organizationsList = new List<Organization>();
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();

            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;

                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT * FROM " + tableName;
                Organization organization = null;
                    
                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    if (type == TYPE_PARTNER)
                        organization = new Partner();
                    else
                        organization = new Organization();
                    //user.Uid = Convert.ToInt32(dr[0].ToString());
                    organization.Name = dr[1].ToString();
                    organization.Address = dr[2].ToString();
                    organization.Phone1 = dr[3].ToString();
                    organization.Phone2 = dr[4].ToString();
                    organization.Email = dr[5].ToString();
                    organization.Website = dr[6].ToString();
                    organization.LocalTimeStamp = dr.GetDateTime(7);
                    organization.Mac = dr[8].ToString();
                    organization.Remoteid = Convert.ToInt32(dr[0].ToString());
                    organization.RemoteTimeStamp = dr.GetDateTime(10);
                    organization.SyncStatus = Convert.ToInt32(dr[11].ToString());
                    organization.Uid = Convert.ToInt32(dr[12].ToString());

                    if (type == TYPE_PARTNER)
                        ((Partner)organization).Organizationid = Convert.ToInt32(dr[13].ToString());

                    organizationsList.Add(organization);
                }
                dr.Close();
                mySqlHelper.CloseConnection();
            }
            return organizationsList;

        }

        public bool InsertToRemote(Organization organization, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            bool insertStatus = false;
            string firstStage = "INSERT INTO " + tableName + " (`title`, `address`, `phone1`, `phone2`, `email`, `web`, `localtimestamp`, `mac`, `uid`";
            string secondStage = " VALUES(@title, @address, @phone1, @phone2, @email, @web, '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', @mac, @uid";
            
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;
                    myCommand = myConnection.CreateCommand();
                    if (type == TYPE_PARTNER)
                    {
                        firstStage += ", `organizationid`)";
                        secondStage += ", @organizationid)";
                    }
                    else
                    {
                        firstStage += ")";
                        secondStage += ")";
                    }
                    myCommand.CommandText = firstStage + secondStage;
                    myCommand.Parameters.AddWithValue("@title", organization.Name);
                    myCommand.Parameters.AddWithValue("@address", organization.Address);
                    myCommand.Parameters.AddWithValue("@phone1", organization.Phone1);
                    myCommand.Parameters.AddWithValue("@phone2", organization.Phone2);
                    myCommand.Parameters.AddWithValue("@email", organization.Email);
                    myCommand.Parameters.AddWithValue("@web", organization.Website);
                    //myCommand.Parameters.AddWithValue("@RemoteId", user.Remoteid);
                    //myCommand.Parameters.AddWithValue("@RemoteTimeStamp", user.RemoteTimeStamp);
                    //myCommand.Parameters.AddWithValue("@SyncStatus", user.SyncStatus);
                    myCommand.Parameters.AddWithValue("@mac", GetMacAddress());
                    myCommand.Parameters.AddWithValue("@uid", organization.Uid);
                    if (type == TYPE_PARTNER)
                    {
                        myCommand.Parameters.AddWithValue("@organizationid", ((Partner)organization).Organizationid);
                    }
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
        public Boolean CheckRemoteOrganization(string name, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();

            bool userExists = false;
            try
            {
                if (mySqlHelper.OpenConnection())
                {
                    MySqlConnection myConnection = mySqlHelper.Connection;

                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "SELECT COUNT(*) FROM "+tableName+" WHERE `title`=@title";
                    myCommand.Parameters.AddWithValue("@title", name);

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
        public int DeleteRemoteOrganization(int organizationRemoteId, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

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
                    myCommand.CommandText = "DELETE FROM " + tableName + " WHERE `ID`=@organizationRemoteId";
                    myCommand.Parameters.AddWithValue("@organizationRemoteId", organizationRemoteId);

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

        public int UpdateRemoteOrganization(int organizationRemoteId, Organization newOrganization, int type)
        {
            string tableName = "Organization";
            if (type == TYPE_ORGANIZATION)
                tableName = "Organization";
            else
                tableName = "Partner";

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
                    if(type==TYPE_PARTNER)
                        myCommand.CommandText = "UPDATE " + tableName + " SET `title`=@title, `address`=@address, `phone1`=@phone1, `phone2`=@phone2, `email`=@email, `web`=@web, `organizationid`=@organizationid WHERE `ID`=@organizationRemoteId";
                    else
                        myCommand.CommandText = "UPDATE " + tableName + " SET `title`=@title, `address`=@address, `phone1`=@phone1, `phone2`=@phone2, `email`=@email, `web`=@web WHERE `ID`=@organizationRemoteId";
                    myCommand.Parameters.AddWithValue("@title", newOrganization.Name);
                    myCommand.Parameters.AddWithValue("@address", newOrganization.Address);
                    myCommand.Parameters.AddWithValue("@phone1", newOrganization.Phone1);
                    myCommand.Parameters.AddWithValue("@phone2", newOrganization.Phone2);
                    myCommand.Parameters.AddWithValue("@email", newOrganization.Email);
                    myCommand.Parameters.AddWithValue("@web", newOrganization.Website);
                    if (type == TYPE_PARTNER)
                        myCommand.Parameters.AddWithValue("@organizationid", ((Partner)newOrganization).Organizationid);

                    myCommand.Parameters.AddWithValue("@organizationRemoteId", organizationRemoteId);

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
