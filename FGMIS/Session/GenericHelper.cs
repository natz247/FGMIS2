using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.Net.NetworkInformation;
using System.Data;
using System.Net;
using MySql.Data.MySqlClient;

namespace Session
{
    public class GenericHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        static int TOP_ACTIVITY_INDEX=10;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public GenericHelper()
        {
            ConnectTo();
        }

        public GenericHelper(int forConnection)
        {

        }

        public bool CheckInternet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;//15 seconds
            request.Method = "HEAD";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        public DataTable GetList(string tableName)
        {
            //List<string> list = new List<string>();
            try
            {
                command.CommandText = "SELECT * FROM "+tableName;
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                /*while(reader.Read())
                {
                    list.Add(reader["title"].ToString());
                }
                return list;*/
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
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

        public List<Location> GetLocationList(string tableName)
        {
            List<Location> list = new List<Location>();
            try
            {
                command.CommandText = "SELECT * FROM " + tableName;
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Location location = new Location(reader["title"].ToString(), Convert.ToInt32(reader["uid"].ToString()));
                    location.Lid = Convert.ToInt32(reader["ID"].ToString());
                    list.Add(location);
                }
                return list;
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

        public void EmptyLocalUsersTable(string tableName)
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "DELETE FROM " + tableName;
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        public int DeleteLocation(string tableName, int locationId)
        {
            int result = -1;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "DELETE FROM " + tableName+" WHERE [ID]="+locationId+"";
            connection.Open();

            result=command.ExecuteNonQuery();

            connection.Close();

            return result;
        }

        public DataTable GetRemoteList(string tableName)
        {
            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            MySqlConnection myConnection = mySqlHelper.Connection;
            try
            {
                DataTable table = new DataTable();
                if (mySqlHelper.OpenConnection())
                {

                    myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "SELECT * FROM " + tableName;

                    MySqlDataReader dr = myCommand.ExecuteReader();

                    table.Load(dr);
                }
                return table;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (myConnection != null)
                {
                    mySqlHelper.CloseConnection();
                }
            }
        }

        public int Insert(string tableName, string title, int uid)
        {
            int result = -1;
            string newTitle = FirstCharToUpper(title);
            string cmdStr = "SELECT COUNT(*) FROM " + tableName + " WHERE title='" + newTitle + "'";
            OleDbCommand cmd = new OleDbCommand(cmdStr, connection);
            connection.Open();
            int count = (int)cmd.ExecuteScalar();
            connection.Close();

            if (count <= 0)//data doesnt exist
            {
                try
                {
                    command.CommandText = "INSERT INTO " + tableName + " ([title], [uid]) VALUES('" + title + "', '" + uid + "')";
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    result=command.ExecuteNonQuery();
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
            return result;
        }

       public static string FirstCharToUpper(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
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


       public bool TableExists(string tableName)
       {
           OleDbConnection myConnection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
           string[] restrictionValues = new string[4] { null, null, null, "TABLE" };
           List<string> tableNames = new List<string>();
           try
           {
               myConnection.Open();
               DataTable schemaInfo = myConnection.GetSchema("Tables", restrictionValues);
               foreach (DataRow row in schemaInfo.Rows)
               {
                   tableNames.Add(row.ItemArray[2].ToString());
               }

           }
           finally
           {
               myConnection.Close();
           }

           for (int i = 0; i < tableNames.Count; i++)
           {
               if (tableName.CompareTo(tableNames[i]) == 0)//table found
                   return true;
           }

           return false;

       }

        public int GetAllActivitiesCount(int year)
        {
            int bigCounter = 0;

            for(int i=1; i<=TOP_ACTIVITY_INDEX; i++)
            {
                bigCounter+=GetRowCount(i, year);
            }
            return bigCounter;
        }

        private int GetRowCount(int activityIndex, int year)
        {
            int rowCounter = 0;
            string tableName = "activity" + activityIndex;
            Activity0 currentActivity = null;
            if(TableExists(tableName))
            {
                User user = null;
                connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM "+tableName;
                connection.Open();

                OleDbDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    currentActivity = new Activity0();
                    currentActivity.ActivityDate = dr.GetDateTime(5);
                    int currentYear = currentActivity.ActivityDate.Year;
                    if (currentYear == year)
                        rowCounter += 1;
                }
                connection.Close();
                dr.Close();
                return rowCounter;
            }
            else
            {
                return 0;
            }
        }

        public int GetOutcomeTotalNumber(int outcomeIndex, int year)
        {
            string tableName = "outcome" + outcomeIndex;
            if (TableExists(tableName))
            {
                int totalNumber = 0;
                connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM " + tableName + " WHERE [oyear]="+year;
                connection.Open();

                OleDbDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                     totalNumber = Convert.ToInt32(dr[2].ToString());
                    
                }
                connection.Close();
                dr.Close();
                return totalNumber;
            }
            else
            {
                return 0;
            }
        }

        private int GetParticipantRowCount(int activityIndex, int year)
        {
            int rowCounter = 0;
            string tableName = "participants" + activityIndex;
            Participant currentParticipant = null;
            if (TableExists(tableName))
            {
                connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM " + tableName;
                connection.Open();

                OleDbDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    currentParticipant = new Participant();
                    currentParticipant.ActivityId=Convert.ToInt32(dr[7].ToString());
                    if (IsOfYear(activityIndex, currentParticipant.ActivityId, year))
                        rowCounter += 1;
                }
                connection.Close();
                dr.Close();
                return rowCounter;
            }
            else
            {
                return 0;
            }
        }

        private bool IsOfYear(int activityIndex, int activityId, int year)
        {
            string tableName = "activity" + activityIndex;
            bool isOfYear = false;
            Activity0 currentActivity = null;
            if (TableExists(tableName))
            {
                int totalNumber = 0;
                connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM " + tableName + " WHERE [ID]=" + activityId;
                connection.Open();

                OleDbDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    currentActivity = new Activity0();
                    currentActivity.ActivityDate = dr.GetDateTime(5);
                    int currentYear = currentActivity.ActivityDate.Year;
                    if (currentYear == year)
                        isOfYear=true;

                }
                connection.Close();
                dr.Close();
                return isOfYear;
            }
            else
            {
                return isOfYear;
            }
        }

        public int GetDataForOutcome(int outcomeIndex, int year)
        {
            int returnValue = 0;
            if(outcomeIndex==1)//outcome1
            {
                returnValue += GetParticipantRowCount(1, year);//activity1
                returnValue += GetParticipantRowCount(2, year);//activity2
                returnValue += GetParticipantRowCount(3, year);//activity3
                returnValue += GetParticipantRowCount(4, year);//activity4
            }
            else if (outcomeIndex == 2)//outcome2
            {
                returnValue += GetParticipantRowCount(3, year);//activity3
                returnValue += GetParticipantRowCount(5, year);//activity5
                returnValue += GetParticipantRowCount(6, year);//activity6
                returnValue += GetParticipantRowCount(7, year);//activity7
                returnValue += GetParticipantRowCount(8, year);//activity8
                returnValue += GetParticipantRowCount(9, year);//activity9
                returnValue += GetParticipantRowCount(10, year);//activity10
            }
            else if (outcomeIndex == 3)//outcome3
            {
                returnValue += GetParticipantRowCount(1, year);//activity1
                returnValue += GetParticipantRowCount(3, year);//activity3
            }
            return returnValue;
        }

    }
}
