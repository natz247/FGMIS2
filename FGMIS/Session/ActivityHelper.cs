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
    public class ActivityHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public ActivityHelper()
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

        public List<Activity0> GetActivityListByPartner(string tableName, string organization, string partner)
        {
            List<Activity0> activityList = new List<Activity0>();
            try
            {
                command.CommandText = "SELECT * FROM "+tableName+" WHERE [sync_status]=1";
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Activity0 activity = new Activity0();
                    activity.Aid = Convert.ToInt32(reader["ID"].ToString());
                    activity.Region = reader["region"].ToString();
                    activity.Zone = reader["zone"].ToString();
                    activity.Woreda = reader["woreda"].ToString();
                    activity.Kebele = reader["kebele"].ToString();
                    activity.ActivityDate = reader.GetDateTime(5);
                    activity.UserId = Convert.ToInt32(reader["user_id"].ToString());
                    activity.RemoteId = Convert.ToInt32(reader["remoteid"].ToString());

                    UserHelper userHelper = new UserHelper();
                    if(userHelper.IsUserOfOrganization(activity.UserId, organization, partner))
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


        public List<int> GetMaleFemaleParticipantsCount(string tableName, int remoteActivityId)
        {
            int maleCount = 0;
            int femaleCount = 0;

            List<int> genderCount = new List<int>();

            try
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT COUNT(*) FROM " + tableName + " WHERE [sex]='Male' AND [remoteactivityid]=@RemoteActivityId";
                command.Parameters.AddWithValue("@RemoteActivityId", remoteActivityId);
                maleCount = (int)command.ExecuteScalar();

                command.CommandText = "SELECT COUNT(*) FROM " + tableName + " WHERE [sex]='Female' AND [remoteactivityid]=@RemoteActivityId";
                command.Parameters.AddWithValue("@RemoteActivityId", remoteActivityId);
                femaleCount = (int)command.ExecuteScalar();

                genderCount.Add(maleCount);
                genderCount.Add(femaleCount);

                return genderCount;

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



    }
}
