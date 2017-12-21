using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.Net.NetworkInformation;
using System.Data;

namespace Session
{
    public class ActivitySelectorHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public ActivitySelectorHelper()
        {
            ConnectTo();
        }

        public void Insert(Activity activity)
        {
            try
            {
                command.CommandText = "INSERT INTO Activities (title) VALUES('"+activity.Title+ "')";
                command.CommandType = CommandType.Text;
                connection.Open();
                command.ExecuteNonQuery();
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

        public List<Activity> GetActivityList()
        {
            List<Activity> activityList = new List<Activity>();
            try
            {
                command.CommandText = "SELECT * FROM Activities";
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Activity activity = new Activity();
                    activity.Aid = Convert.ToInt32(reader["ID"].ToString());
                    activity.Title = reader["title"].ToString();
                    activity.TableName = reader["tablename"].ToString();

                    activityList.Add(activity);
                }
                return activityList;
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


        public List<ActivityListItem> GetActivityList(string tableName)
        {
            List<ActivityListItem> activityList = new List<ActivityListItem>();
            try
            {
                command.CommandText = "SELECT * FROM "+tableName+" where sync_status=0";
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ActivityListItem activity = new ActivityListItem();
                    activity.Aid = Convert.ToInt32(reader["ID"].ToString());
                    activity.LocalTimeStamp = reader.GetDateTime(11);
                    activity.Title = reader["region"].ToString() + ", "+ reader["zone"].ToString()+", "+ reader["woreda"].ToString();
                    activity.TableName = tableName;

                    activityList.Add(activity);
                }
                return activityList;
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



    }
}
