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
    public class Report1Helper
    {
        OleDbConnection connection;
        OleDbCommand command;

        int activityRemoteId = -1;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public Report1Helper()
        {
            ConnectTo();
        }

        public int Insert(ReportClass1 report, List<Output> outputsList)
        {
            int reportId = -1;
            try
            {
                command.CommandText = "INSERT INTO Report1 ([partner],[title],[period],[pyear],[plocation],[apopulation],[tpopulation],[budget],[description],[goal],[objective],[strategies],[tgroups],[participation],[coordination],[challenges],[stories],[engagement],[issues],[sustainability],[risks],[accountability],[minitoring],[uid],[localtimestamp],[remoteid],[remotetimestamp],[mac],[sync_status],[submit_status],[submissiondate]) VALUES(@partner,@title,@period,@pyear,@plocation,@apopulation,@tpopulation,@budget,@description,@goal,@objective,@strategies,@tgroups,@participation,@coordination,@challenges,@stories,@engagement,@issues,@sustainability,@risks,@accountability,@minitoring,@uid,'"+report.Localtimestamp+"',@remoteid,'"+report.Remotetimestamp+"',@mac,0,0,'"+report.Submissiondate+"');";

                command.Parameters.AddWithValue("@Partner",report.Partner);
                command.Parameters.AddWithValue("@title",report.Title);
                command.Parameters.AddWithValue("@period",report.Period);
                command.Parameters.AddWithValue("@pyear",report.Pyear);
                command.Parameters.AddWithValue("@plocation",report.Plocation);
                command.Parameters.AddWithValue("@apopulation",report.Apopulation);
                command.Parameters.AddWithValue("@tpopulation",report.Tpopulation);
                command.Parameters.AddWithValue("@budget",report.Budget);
                command.Parameters.AddWithValue("@description",report.Description);
                command.Parameters.AddWithValue("@goal",report.Goal);
                command.Parameters.AddWithValue("@objective",report.Objective);
                command.Parameters.AddWithValue("@strategies",report.Strategies);
                command.Parameters.AddWithValue("@tgroups",report.Tgroups);
                command.Parameters.AddWithValue("@participation",report.Participation);
                command.Parameters.AddWithValue("@coordination",report.Coordination);
                command.Parameters.AddWithValue("@challenges",report.Challenges);
                command.Parameters.AddWithValue("@stories",report.Stories);
                command.Parameters.AddWithValue("@engagement",report.Engagement);
                command.Parameters.AddWithValue("@issues",report.Issues);
                command.Parameters.AddWithValue("@sustainability",report.Sustainability);
                command.Parameters.AddWithValue("@risks",report.Risks);
                command.Parameters.AddWithValue("@accountability",report.Accountability);
                command.Parameters.AddWithValue("@minitoring",report.Monitoring);
                command.Parameters.AddWithValue("@uid",report.Uid);
                //command.Parameters.AddWithValue("@localtimestamp",report.Localtimestamp);
                command.Parameters.AddWithValue("@remoteid",report.Remoteid);
                //command.Parameters.AddWithValue("@remotetimestamp",report.Remotetimestamp);
                command.Parameters.AddWithValue("@mac",report.Mac);
                //command.Parameters.AddWithValue("@sync_status",report.Syncstatus);
                //command.Parameters.AddWithValue("@submit_status",report.Submitstatus);
                //command.Parameters.AddWithValue("@submissiondate", report.Submissiondate);
                connection.Open();
                //ID=Convert.ToInt32(command.ExecuteScalar());
                command.ExecuteNonQuery();

                command.CommandText = "SELECT @@IDENTITY";
                OleDbDataReader dr = command.ExecuteReader();
                while(dr.Read())
                {
                    reportId = Convert.ToInt32(dr[0].ToString());
                }
                dr.Close();
                //command.CommandType = CommandType.Text;
                //int ID = command.ExecuteNonQuery();
                if(reportId!=-1)
                {
                    for (int i = 0; i < outputsList.Count; i++)
                    {
                        Output output = (Output)outputsList[i];
                        OleDbCommand command2 = connection.CreateCommand();
                        command2.CommandText = "INSERT INTO [Output] ([title],[type],[activityid],[reportid],[plannedm],[plannedf],[actualm],[actualf],[results],[deviation]) VALUES(@title,@type,@activityid,@reportid,@plannedm,@plannedf,@actualm,@actualf,@results,@deviation);";
                        
                        command2.Parameters.AddWithValue("@title", output.Title);
                        command2.Parameters.AddWithValue("@type", output.Type);
                        command2.Parameters.AddWithValue("@activityid", output.Activityid);
                        command2.Parameters.AddWithValue("@reportid", reportId);
                        command2.Parameters.AddWithValue("@plannedm", output.Plannedm);
                        command2.Parameters.AddWithValue("@plannedf", output.Plannedf);
                        command2.Parameters.AddWithValue("@actualm", output.Actualm);
                        command2.Parameters.AddWithValue("@actualf", output.Actualf);
                        command2.Parameters.AddWithValue("@results", output.Results);
                        command2.Parameters.AddWithValue("@deviation", output.Deviation);
                        
                        command2.ExecuteNonQuery();
                    }
                }
                
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
            return reportId;
        }

        public Activity1 GetActivityData(int activityId)
        {
            Activity1 activity = null;
            string tableName = "Activity1";
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + tableName + " WHERE ID="+activityId;
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                activity = new Activity1();
                activity.Aid = Convert.ToInt32(dr[0].ToString());
                activity.Region = dr[1].ToString();
                activity.Zone = dr[2].ToString();
                activity.Woreda = dr[3].ToString();
                activity.Kebele = dr[4].ToString();
                activity.ActivityDate = dr.GetDateTime(5);
                //activity.SubmissionDate = dr.GetDateTime(6);
                activity.UserId = Convert.ToInt32(dr[7].ToString());
                activity.FacilitatorName = dr[8].ToString();
                activity.Position = dr[9].ToString();
                activity.SubmitStatus = Convert.ToInt32(dr[10].ToString());
                activity.LocalTimeStamp = dr.GetDateTime(11);
                activity.Mac = dr[12].ToString();
                activity.RemoteId = Convert.ToInt32(dr[13].ToString());
                //activity.RemoteTimeStamp = dr.GetDateTime(14);
                activity.SyncStatus = Convert.ToInt32(dr[15].ToString());
                activity.Place = dr[16].ToString();
                activity.Duration = dr[17].ToString();
                activity.IssuesRaised = dr[18].ToString();
                activity.AgreedActionPoints = dr[19].ToString();
            }
            connection.Close();
            dr.Close();
            return activity;
        }

        public List<Activity1> GetUnsyncedActivityList()
        {
            List<Activity1> activityList = new List<Activity1>();
            string tableName = "Activity1";
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + tableName + " WHERE [sync_status]=0";
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Activity1 activity = new Activity1();
                activity.Aid = Convert.ToInt32(dr[0].ToString());
                activity.Region = dr[1].ToString();
                activity.Zone = dr[2].ToString();
                activity.Woreda = dr[3].ToString();
                activity.Kebele = dr[4].ToString();
                activity.ActivityDate = dr.GetDateTime(5);
                //activity.SubmissionDate = dr.GetDateTime(6);
                activity.UserId = Convert.ToInt32(dr[7].ToString());
                activity.FacilitatorName = dr[8].ToString();
                activity.Position = dr[9].ToString();
                activity.SubmitStatus = Convert.ToInt32(dr[10].ToString());
                activity.LocalTimeStamp = dr.GetDateTime(11);
                activity.Mac = dr[12].ToString();
                //activity.RemoteId = Convert.ToInt32(dr[13].ToString());
                //activity.RemoteTimeStamp = dr.GetDateTime(14);
                activity.SyncStatus = Convert.ToInt32(dr[15].ToString());
                activity.Place = dr[16].ToString();
                activity.Duration = dr[17].ToString();
                activity.IssuesRaised = dr[18].ToString();
                activity.AgreedActionPoints = dr[19].ToString();

                activityList.Add(activity);
            }
            connection.Close();
            dr.Close();
            return activityList;
        }

        public List<Participant1> GetUnsyncedParticpansList()
        {
            List<Participant1> participantList = new List<Participant1>();
            string tableName = "Participants1";
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM " + tableName + " WHERE [sync_status]=0";
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Participant1 participant = new Participant1();
                participant.Pid = Convert.ToInt32(dr[0].ToString());
                participant.Name = dr[1].ToString();
                participant.Kebele = dr[2].ToString();
                participant.Woreda = dr[3].ToString();
                participant.Sex = dr[4].ToString();
                participant.Age = Convert.ToInt32(dr[5].ToString());
                participant.Disabled = Convert.ToInt32(dr[6].ToString());
                participant.ActivityId= Convert.ToInt32(dr[7].ToString());

                participantList.Add(participant);
            }
            connection.Close();
            dr.Close();
            return participantList;
        }

        public List<Participant1> GetParticpantData(int activityId)
        {
            List<Participant1> participantList = new List<Participant1>() ;
            string tableName = "Participants1";
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM " + tableName + " WHERE [activityid]=" + activityId;
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Participant1 participant = new Participant1();
                participant.Pid = Convert.ToInt32(dr[0].ToString());
                participant.Name = dr[1].ToString();
                participant.Kebele = dr[2].ToString();
                participant.Sex = dr[4].ToString();
                participant.Age = Convert.ToInt32(dr[5].ToString());

                participantList.Add(participant);
            }
            connection.Close();
            dr.Close();
            return participantList;
        }

        public int DeleteActivity(int activityId)
        {
            int resultValue = -1;
            try {
                Activity1 activity = null;
                string tableName = "Activity1";
                connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM " + tableName + " WHERE ID=" + activityId;
                connection.Open();
                resultValue = command.ExecuteNonQuery();
                connection.Close();

                if (resultValue > 0)//Row affected
                {
                    tableName = "Participants1";
                    command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM " + tableName + " WHERE [activityid]=" + activityId;
                    connection.Open();
                    resultValue = command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch
            {
                
            }

            return resultValue;
        }

        public int SendParticipantsListToServer(List<Participant1> participantsList)
        {
            int syncSuccessful = -1;

            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;

                for (int i = 0; i < participantsList.Count; i++)
                {
                    Participant1 participant = (Participant1)participantsList[i];
                    syncSuccessful = InsertParticipant(participant, myConnection);
                }
                mySqlHelper.CloseConnection();
            }

            return syncSuccessful;
        }

        public int SendActivityListToServer(List<Activity1> activityList)
        {
            int syncSuccessful = -1;

            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;
               
                for (int i = 0; i < activityList.Count; i++)
                {
                    Activity1 activity = (Activity1)activityList[i];
                    syncSuccessful=InsertActivity(activity, myConnection);
                }
                mySqlHelper.CloseConnection();
            }

            return syncSuccessful;
        }


        public int InsertActivity(Activity1 activity, MySqlConnection myConnection)
        {
            
            try
            {
               MySqlCommand myCommand = myConnection.CreateCommand();
               myCommand.CommandText = "INSERT INTO Activity1 (`region`, `zone`, `woreda`, `kebele`, `activity_date`, `submission_date`, `user_id`, `facilitator_name`, `position`, `issues_raised`, `agreed_action_points`,`localtimestamp`, `mac`, `place`, `duration`) VALUES(@Region, @Zone, @Woreda, @Kebele, '" + activity.ActivityDate.ToString("yyyy-MM-dd H:mm:ss") + "', '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") +"', @UserId, @FacilitatorName, @Position, @IssuesRaised, @AgreedActionPoints, '" + activity.LocalTimeStamp.ToString("yyyy-MM-dd H:mm:ss") + "', @Mac, @Place, @Duration);";

                myCommand.Parameters.AddWithValue("@Region", activity.Region);
                myCommand.Parameters.AddWithValue("@Zone", activity.Zone);
                myCommand.Parameters.AddWithValue("@Woreda", activity.Woreda);
                myCommand.Parameters.AddWithValue("@Kebele", activity.Kebele);
                //command.Parameters.AddWithValue("@ActivityDate", activity.ActivityDate);
                //command.Parameters.AddWithValue("@SubmissionDate", DateTime.Now);
                myCommand.Parameters.AddWithValue("@UserId", activity.UserId);
                myCommand.Parameters.AddWithValue("@FacilitatorName", activity.FacilitatorName);
                myCommand.Parameters.AddWithValue("@Position", activity.Position);
                myCommand.Parameters.AddWithValue("@IssuesRaised", activity.IssuesRaised);
                myCommand.Parameters.AddWithValue("@AgreedActionPoints", activity.AgreedActionPoints);
                //command.Parameters.AddWithValue("@LocalTimeStamp", activity.LocalTimeStamp);
                myCommand.Parameters.AddWithValue("@Mac", activity.Mac);
                myCommand.Parameters.AddWithValue("@Place", activity.Place);
                myCommand.Parameters.AddWithValue("@Duration", activity.Duration);
                
                //ID=Convert.ToInt32(command.ExecuteScalar());
                myCommand.ExecuteNonQuery();

                myCommand.CommandText = "SELECT @@IDENTITY";
                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    activityRemoteId = Convert.ToInt32(dr[0].ToString());
                }
                //update local database
                UpdateLocalActivity(activity, activityRemoteId);
                dr.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (myConnection != null)
                {
                    //myConnection.Close();
                }
            }
            return activityRemoteId;
         }


        public int InsertParticipant(Participant1 participant, MySqlConnection myConnection)
        {
            int participantRemoteId = -1;
            try
            {
                MySqlCommand myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "INSERT INTO Participants1 (`pname`, `kebele`, `woreda`, `sex`, `age`, `disabled`, `activityid`, `remoteactivityid`) VALUES(@PName, @Kebele, @Woreda, @Sex, @Age, @Disabled, @ActivityId, @RemoteActivityId);";

                myCommand.Parameters.AddWithValue("@PName", participant.Name);
                myCommand.Parameters.AddWithValue("@Kebele", participant.Kebele);
                myCommand.Parameters.AddWithValue("@Woreda", participant.Woreda);
                myCommand.Parameters.AddWithValue("@Sex", participant.Sex);
                myCommand.Parameters.AddWithValue("@Age", participant.Age);
                myCommand.Parameters.AddWithValue("@Disabled", participant.Disabled);
                myCommand.Parameters.AddWithValue("@ActivityId", participant.ActivityId);
                myCommand.Parameters.AddWithValue("@RemoteActivityId", activityRemoteId);

                //ID=Convert.ToInt32(command.ExecuteScalar());
                myCommand.ExecuteNonQuery();

                myCommand.CommandText = "SELECT @@IDENTITY";
                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                   participantRemoteId = Convert.ToInt32(dr[0].ToString());
                }
                //update local database
                UpdateLocalParticipant(participant, participantRemoteId);
                dr.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (myConnection != null)
                {
                    //myConnection.Close();
                }
            }
            return participantRemoteId;
        }

        private void UpdateLocalParticipant(Participant1 participant, int participantRemoteId)
        {
            //make sync_status to 1
            //change remote id to new remote id
            string tableName = "Participants1";
            OleDbConnection connection2 = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            OleDbCommand command2 = connection2.CreateCommand();
            command2.CommandText = "UPDATE " + tableName + " SET [sync_status]=1, [remoteactivityid]=@RemoteActivityId, [remoteparticipantid]=@RemoteParticipantId WHERE ID=" + participant.Pid;
            command2.Parameters.AddWithValue("@RemoteActivityId", activityRemoteId);
            command2.Parameters.AddWithValue("@RemoteParticipantId", participantRemoteId);
            
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();

        }

        private void UpdateLocalActivity(Activity1 activity, int activityRemoteId)
        {
            //make sync_status to 1
            //change remote id to new remote id
            string tableName = "Activity1";
            OleDbConnection connection2 = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            OleDbCommand command2 = connection2.CreateCommand();
            command2.CommandText = "UPDATE " + tableName + " SET [sync_status]=1, [remoteid]=@RemoteActivityId WHERE ID=" + activity.Aid;
            command2.Parameters.AddWithValue("@RemoteActivityId", activityRemoteId);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();

        }

       

    }
}
