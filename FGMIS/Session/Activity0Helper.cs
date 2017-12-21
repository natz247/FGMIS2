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
    public class Activity0Helper
    {
        OleDbConnection connection;
        OleDbCommand command;

        int activityRemoteId = -1;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public Activity0Helper()
        {
            ConnectTo();
        }

        public int Insert(int activityType, Activity0 activity, List<Participant> participantsList)
        {
            int activityId = -1;
            string firstStageCommand = "";
            string secondStageCommand = "";
            string tableName = "Activity" + activityType;
            try
            {
                firstStageCommand = "INSERT INTO "+tableName+" ([region], [zone], [woreda], [kebele], [activity_date], [user_id], [facilitator_name], [position], [issues_raised], [agreed_action_points], [localtimestamp], [mac]";
                secondStageCommand = " VALUES(@Region, @Zone, @Woreda, @Kebele, '" + activity.ActivityDate + "', @UserId, @FacilitatorName, @Position, @IssuesRaised, @AgreedActionPoints, '" + activity.LocalTimeStamp + "', @Mac";
                //command.CommandText = "INSERT INTO Activity1 ([region], [zone], [woreda], [kebele], [activity_date], [user_id], [facilitator_name], [position], [issues_raised], [agreed_action_points], [localtimestamp], [mac], [place], [duration]) VALUES(@Region, @Zone, @Woreda, @Kebele, '" + activity.ActivityDate+"', @UserId, @FacilitatorName, @Position, @IssuesRaised, @AgreedActionPoints, '"+activity.LocalTimeStamp+"', @Mac, @Place, @Duration);";

                //////////////////////////////////////////////////////////////////////////////
                if (activityType == 1)//if Activity1
                {
                    firstStageCommand += ", [place], [duration])";
                    secondStageCommand += ", @Place, @Duration);";
                }
                else if(activityType==2)//if Activiiy2
                {
                    firstStageCommand += ", [schoolname], [clubname], [clubleadername])";
                    secondStageCommand += ", @schoolname, @clubname, @clubleadername);";
                }
                else if (activityType == 3)//if Activiiy3
                {
                    firstStageCommand += ", [trainingtitle], [trainingfacilitator])";
                    secondStageCommand += ", @trainingtitle, @trainingfacilitator);";
                }
                else if (activityType == 4)//if Activiiy4
                {
                    firstStageCommand += ", [stage], [ccfacilitatorname], [duration])";
                    secondStageCommand += ", @stage, @ccfacilitatorname, @duration);";
                }
                else if (activityType == 5)//if Activiiy5
                {
                    firstStageCommand += ", [ryear])";
                    secondStageCommand += ", @ryear);";
                }
                else if (activityType == 6)//if Activiiy6
                {
                    firstStageCommand += ", [place])";
                    secondStageCommand += ", @place);";
                }
                else if (activityType == 7)//if Activiiy7
                {
                    firstStageCommand += ", [totalnumberofmembers])";
                    secondStageCommand += ", @totalnumberofmembers);";
                }
                else if (activityType == 8)//if Activiiy8
                {
                    firstStageCommand += ", [ayear], [schoolname])";
                    secondStageCommand += ", @ayear, @schoolname);";
                }
                else if (activityType == 9)//if Activiiy9
                {
                    firstStageCommand += ")";
                    secondStageCommand += ");";
                }
                else
                {
                    firstStageCommand += ")";
                    secondStageCommand += ");";
                }
                //////////////////////////////////////////////////////////////////////////////
                command.CommandText = firstStageCommand + secondStageCommand;

                if (activity.Region != null)
                    command.Parameters.AddWithValue("@Region", activity.Region);
                else
                    command.Parameters.AddWithValue("@Region", "");

                if (activity.Zone != null)
                    command.Parameters.AddWithValue("@Zone", activity.Zone);
                else
                    command.Parameters.AddWithValue("@Zone", "");

                if (activity.Woreda != null)
                    command.Parameters.AddWithValue("@Woreda", activity.Woreda);
                else
                    command.Parameters.AddWithValue("@Woreda", "");

                if (activity.Kebele != null)
                    command.Parameters.AddWithValue("@Kebele", activity.Kebele);
                else
                    command.Parameters.AddWithValue("@Kebele", "");

                command.Parameters.AddWithValue("@UserId", activity.UserId);
                command.Parameters.AddWithValue("@FacilitatorName", activity.FacilitatorName);
                command.Parameters.AddWithValue("@Position", activity.Position);
                command.Parameters.AddWithValue("@IssuesRaised", activity.IssuesRaised);
                command.Parameters.AddWithValue("@AgreedActionPoints", activity.AgreedActionPoints);
                command.Parameters.AddWithValue("@Mac", activity.Mac);
                //////////////////////////////////////////////////////////////////////////////
                if(activityType==1)
                {
                    command.Parameters.AddWithValue("@Place", ((Activity1)activity).Place);
                    command.Parameters.AddWithValue("@Duration", ((Activity1)activity).Duration);
                }
                else if(activityType==2)
                {
                    command.Parameters.AddWithValue("@schoolname", ((Activity2)activity).Schoolname);
                    command.Parameters.AddWithValue("@clubname", ((Activity2)activity).Clubname);
                    command.Parameters.AddWithValue("@clubleadername", ((Activity2)activity).Clubleadername);
                }
                else if (activityType == 3)
                {
                    command.Parameters.AddWithValue("@trainingtitle", ((Activity3)activity).Trainingtitle);
                    command.Parameters.AddWithValue("@trainingfacilitator", ((Activity3)activity).Trainingfacilitator);
                }
                else if (activityType == 4)
                {
                    command.Parameters.AddWithValue("@stage", ((Activity4)activity).Stage);
                    command.Parameters.AddWithValue("@ccfacilitatorname", ((Activity4)activity).Ccfacilitatorname);
                    command.Parameters.AddWithValue("@duration", ((Activity4)activity).Duration);
                }
                else if (activityType == 5)
                {
                    command.Parameters.AddWithValue("@ryear", ((Activity5)activity).Year);
                }
                else if (activityType == 6)
                {
                    command.Parameters.AddWithValue("@place", ((Activity6)activity).Place);
                }
                else if (activityType == 7)
                {
                    command.Parameters.AddWithValue("@totalnumberofmembers", ((Activity7)activity).Totalnumberofmembers);
                }
                else if (activityType == 8)
                {
                    command.Parameters.AddWithValue("@ayear", ((Activity8)activity).Year);
                    command.Parameters.AddWithValue("@schoolname", ((Activity8)activity).Schoolname);
                }
                else if (activityType == 9)
                {
                    
                }
                //////////////////////////////////////////////////////////////////////////////

                //Console.WriteLine(firstStageCommand + secondStageCommand);
                connection.Open();
                command.ExecuteNonQuery();

                command.CommandText = "SELECT @@IDENTITY";
                OleDbDataReader dr = command.ExecuteReader();
                while(dr.Read())
                {
                    activityId = Convert.ToInt32(dr[0].ToString());
                }
                dr.Close();
                
                
                if(activityId!=-1)
                {
                    tableName = "Participants" + activityType;
                    firstStageCommand = "INSERT INTO " + tableName + " ([pname], [kebele], [woreda], [sex], [age], [disabled], [activityid]";
                    secondStageCommand = " VALUES(@Name, @Kebele, @Woreda, @Sex, @Age, @Disabled, @ActivityId";


                    //////////////////////////////////////////////////////////////////////////////
                    if (activityType == 1)//if Activity1
                    {
                        firstStageCommand += ")";
                        secondStageCommand += ");";
                    }
                    else if (activityType == 2)//if Activiiy2
                    {
                        firstStageCommand += ", [gradelevel])";
                        secondStageCommand += ", @gradelevel);";
                    }
                    else if (activityType == 3)//if Activiiy3
                    {
                        firstStageCommand += ", [organization], [position])";
                        secondStageCommand += ", @organization, @position);";
                    }
                    else if (activityType == 4)//if Activiiy4
                    {
                        firstStageCommand += ", [role])";
                        secondStageCommand += ", @role);";
                    }
                    else if (activityType == 5)//if Activiiy5
                    {
                        firstStageCommand += ", [pdate], [code], [town], [dateofvisit], [identifiedcase], [referralhealth], [referralpolice], [referrallegal], [referralpsycho], [others])";
                        secondStageCommand += ", @pdate, @code, @town, @dateofvisit, @identifiedcase, @referralhealth, @referralpolice, @referrallegal, @referralpsycho, @others);";
                    }

                    else if (activityType == 6)//if Activiiy6
                    {
                        firstStageCommand += ")";
                        secondStageCommand += ");";
                    }
                    else if (activityType == 7)//if Activiiy7
                    {
                        firstStageCommand += ", [groupname], [noofadolescent], [noofwomen], [typeoftraining], [capital], [typeofiga], [statusofiga], [amountofsaving], [remark])";
                        secondStageCommand += ", @groupname, @noofadolescent, @noofwomen, @typeoftraining, @capital, @typeofiga, @statusofiga, @amountofsaving, @remark);";
                    }
                    else if (activityType == 8)//if Activiiy8
                    {
                        firstStageCommand += ", [gradelevel], [statusofretention], [remark])";
                        secondStageCommand += ", @gradelevel, @statusofretention, @remark);";
                    }
                    else if (activityType == 9)//if Activiiy9
                    {
                        firstStageCommand += ", [noofadolescent], [noofwomen], [typeofmaterial], [unit], [quantity])";
                        secondStageCommand += ", @noofadolescent, @noofwomen, @typeofmaterial, @unit, @quantity);";
                    }
                    else
                    {
                        firstStageCommand += ")";
                        secondStageCommand += ");";
                    }
                    //////////////////////////////////////////////////////////////////////////////
                    
                    
                    for (int i = 0; i < participantsList.Count; i++)
                    {
                        Participant participant = (Participant)participantsList[i];
                        OleDbCommand command2 = connection.CreateCommand();

                        command2.CommandText = firstStageCommand + secondStageCommand;

                        if (participant.Name!=null)
                            command2.Parameters.AddWithValue("@Name", participant.Name);
                        else
                            command2.Parameters.AddWithValue("@Name", "");

                        if (participant.Kebele!=null)
                            command2.Parameters.AddWithValue("@Kebele", participant.Kebele);
                        else
                            command2.Parameters.AddWithValue("@Kebele", "");

                        if (participant.Woreda!=null)
                            command2.Parameters.AddWithValue("@Woreda", participant.Woreda);
                        else
                            command2.Parameters.AddWithValue("@Woreda", "");

                        if (participant.Sex!=null)
                            command2.Parameters.AddWithValue("@Sex", participant.Sex);
                        else
                            command2.Parameters.AddWithValue("@Sex", "");

                        if (participant.Age!=null)
                            command2.Parameters.AddWithValue("@Age", participant.Age);
                        else
                            command2.Parameters.AddWithValue("@Age", -1);

                        if (participant.Disabled!=null)
                            command2.Parameters.AddWithValue("@Dsiabled", participant.Disabled);
                        else
                            command2.Parameters.AddWithValue("@Dsiabled", -1);


                        command2.Parameters.AddWithValue("@ActivityId", activityId);

                        //////////////////////////////////////////////////////////////////////////////
                        if (activityType == 2)//if Activiiy2
                        {
                            command2.Parameters.AddWithValue("@gradelevel", ((Participant2)participant).Gradelevel);
                        }
                        else if (activityType == 3)//if Activiiy3
                        {
                            command2.Parameters.AddWithValue("@organization", ((Participant3)participant).Organization);
                            command2.Parameters.AddWithValue("@position", ((Participant3)participant).Position);
                        }
                        else if (activityType == 4)//if Activiiy4
                        {
                            command2.Parameters.AddWithValue("@role", ((Participant4)participant).Role);
                        }
                        else if (activityType == 5)//if Activiiy5
                        {
                            command2.Parameters.AddWithValue("@pdate", ((Participant5)participant).Pdate);
                            command2.Parameters.AddWithValue("@code", ((Participant5)participant).Code);
                            command2.Parameters.AddWithValue("@town", ((Participant5)participant).Town);
                            command2.Parameters.AddWithValue("@dateofvisit", ((Participant5)participant).Dateofvisit);
                            command2.Parameters.AddWithValue("@identifiedcase", ((Participant5)participant).Identifiedcase);
                            command2.Parameters.AddWithValue("@referralhealth", ((Participant5)participant).Referralhealth);
                            command2.Parameters.AddWithValue("@referralpolice", ((Participant5)participant).Referralpolice);
                            command2.Parameters.AddWithValue("@referrallegal", ((Participant5)participant).Referrallegal);
                            command2.Parameters.AddWithValue("@referralpsycho", ((Participant5)participant).Referralpsycho);
                            command2.Parameters.AddWithValue("@others", ((Participant5)participant).Others);
                        }
                        else if (activityType == 6)//if Activiiy6
                        {
                            
                        }

                        else if (activityType == 7)//if Activiiy7
                        {
                            command2.Parameters.AddWithValue("@groupname", ((Participant7)participant).Groupname);
                            command2.Parameters.AddWithValue("@noofadolescent", ((Participant7)participant).Noofadolescent);
                            command2.Parameters.AddWithValue("@noofwomen", ((Participant7)participant).Noofwomen);
                            command2.Parameters.AddWithValue("@typeoftraining", ((Participant7)participant).Typeoftraining);
                            command2.Parameters.AddWithValue("@capital", ((Participant7)participant).Capital);
                            command2.Parameters.AddWithValue("@typeofiga", ((Participant7)participant).Typeofiga);
                            command2.Parameters.AddWithValue("@statusofiga", ((Participant7)participant).Statusofiga);
                            command2.Parameters.AddWithValue("@amountofsaving", ((Participant7)participant).Amountofsaving);
                            command2.Parameters.AddWithValue("@remark", ((Participant7)participant).Remark);
                        }
                        else if (activityType == 8)//if Activiiy8
                        {
                            command2.Parameters.AddWithValue("@gradelevel", ((Participant8)participant).Gradelevel);
                            command2.Parameters.AddWithValue("@statusofretention", ((Participant8)participant).Statusofretention);
                            command2.Parameters.AddWithValue("@remark", ((Participant8)participant).Remark);
                        }
                        else if (activityType == 9)//if Activiiy9
                        {
                            command2.Parameters.AddWithValue("@noofadolescent", ((Participant9)participant).Noofadolescent);
                            command2.Parameters.AddWithValue("@noofwomen", ((Participant9)participant).Noofwomen);
                            command2.Parameters.AddWithValue("@typeofmaterial", ((Participant9)participant).Typeofmaterial);
                            command2.Parameters.AddWithValue("@unit", ((Participant9)participant).Unit);
                            command2.Parameters.AddWithValue("@quantity", ((Participant9)participant).Quantity);
                        }
//////////////////////////////////////////////////////////////////////////////

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
            return activityId;
        }

        public Activity0 GetActivityData(int activityType, int activityId)
        {
            Activity0 activity = null;
            string tableName = "Activity"+activityType;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + tableName + " WHERE ID="+activityId;
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if(activityType==1)
                    activity = new Activity1();
                else if(activityType==2)
                    activity = new Activity2();
                else if (activityType == 3)
                    activity = new Activity3();
                else if (activityType == 4)
                    activity = new Activity4();
                else if (activityType == 5)
                    activity = new Activity5();
                else if (activityType == 6)
                    activity = new Activity6();
                else if (activityType == 7)
                    activity = new Activity7();
                else if (activityType == 8)
                    activity = new Activity8();
                else if (activityType == 9)
                    activity = new Activity9();

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
                activity.IssuesRaised = dr[16].ToString();
                activity.AgreedActionPoints = dr[17].ToString();

                if(activityType==1)//Activity1
                {
                    ((Activity1)activity).Place = dr[18].ToString();
                    ((Activity1)activity).Duration = dr[19].ToString();
                }
                else if (activityType == 2)//Activity2
                {
                    ((Activity2)activity).Schoolname = dr[18].ToString();
                    ((Activity2)activity).Clubname = dr[19].ToString();
                    ((Activity2)activity).Clubleadername= dr[20].ToString();
                }
                else if (activityType == 3)//Activity3
                {
                    ((Activity3)activity).Trainingtitle = dr[18].ToString();
                    ((Activity3)activity).Trainingfacilitator = dr[19].ToString();
                }
                else if (activityType == 4)//Activity4
                {
                    ((Activity4)activity).Stage = dr[18].ToString();
                    ((Activity4)activity).Ccfacilitatorname = dr[19].ToString();
                    ((Activity4)activity).Duration = dr[20].ToString();
                }
                else if (activityType == 5)//Activity5
                {
                    ((Activity5)activity).Year = dr[18].ToString();
                }
                else if (activityType == 6)//Activity6
                {
                    ((Activity6)activity).Place = dr[18].ToString();
                }
                else if (activityType == 7)//Activity7
                {
                    ((Activity7)activity).Totalnumberofmembers = Convert.ToInt32(dr[18].ToString());
                }
                else if (activityType == 8)//Activity8
                {
                    ((Activity8)activity).Year = dr[18].ToString();
                    ((Activity8)activity).Schoolname = dr[19].ToString();
                }
                else if (activityType == 9)//Activity9
                {
                    
                }
            }
            connection.Close();
            dr.Close();
            return activity;
        }

        public List<Activity0> GetUnsyncedActivityList(int activityType)
        {
            Activity0 activity=null;
            List<Activity0> activityList = new List<Activity0>();
            string tableName = "Activity"+activityType;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM " + tableName + " WHERE [sync_status]=0";
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (activityType == 1)
                    activity = new Activity1();
                else if (activityType == 2)
                    activity = new Activity2();
                else if (activityType == 3)
                    activity = new Activity3();
                else if (activityType == 4)
                    activity = new Activity4();
                else if (activityType == 5)
                    activity = new Activity5();
                else if (activityType == 6)
                    activity = new Activity6();
                else if (activityType == 7)
                    activity = new Activity7();
                else if (activityType == 8)
                    activity = new Activity8();
                else if (activityType == 9)
                    activity = new Activity9();

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
                activity.IssuesRaised = dr[16].ToString();
                activity.AgreedActionPoints = dr[17].ToString();

                if (activityType == 1)//Activity1
                {
                    ((Activity1)activity).Place = dr[18].ToString();
                    ((Activity1)activity).Duration = dr[19].ToString();
                }
                else if (activityType == 2)//Activity2
                {
                    ((Activity2)activity).Schoolname = dr[18].ToString();
                    ((Activity2)activity).Clubname = dr[19].ToString();
                    ((Activity2)activity).Clubleadername = dr[20].ToString();
                }
                else if (activityType == 3)//Activity3
                {
                    ((Activity3)activity).Trainingtitle = dr[18].ToString();
                    ((Activity3)activity).Trainingfacilitator= dr[19].ToString();
                }
                else if (activityType == 4)//Activity4
                {
                    ((Activity4)activity).Stage = dr[18].ToString();
                    ((Activity4)activity).Ccfacilitatorname = dr[19].ToString();
                    ((Activity4)activity).Duration = dr[20].ToString();
                }
                else if (activityType == 5)//Activity5
                {
                    ((Activity5)activity).Year = dr[18].ToString();
                }
                else if (activityType == 6)//Activity6
                {
                    ((Activity6)activity).Place = dr[18].ToString();
                }
                else if (activityType == 7)//Activity7
                {
                    ((Activity7)activity).Totalnumberofmembers = Convert.ToInt32(dr[18].ToString());
                }
                else if (activityType == 8)//Activity8
                {
                    ((Activity8)activity).Year = dr[18].ToString();
                    ((Activity8)activity).Schoolname = dr[19].ToString();
                }
                else if (activityType == 9)//Activity9
                {
                    
                }

                activityList.Add(activity);
            }
            connection.Close();
            dr.Close();
            return activityList;
        }

        public List<Participant> GetUnsyncedParticpansList(int activityType)
        {
            Participant participant = null;
            List<Participant> participantList = new List<Participant>();
            string tableName = "Participants"+activityType;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM " + tableName + " WHERE [sync_status]=0";
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (activityType == 1)
                    participant = new Participant1();
                else if (activityType == 2)
                    participant = new Participant2();
                else if (activityType == 3)
                    participant = new Participant3();
                else if (activityType == 4)
                    participant = new Participant4();
                else if (activityType == 5)
                    participant = new Participant5();
                else if (activityType == 6)
                    participant = new Participant6();
                else if (activityType == 7)
                    participant = new Participant7();
                else if (activityType == 8)
                    participant = new Participant8();
                else if (activityType == 9)
                    participant = new Participant9();

                participant.Pid = Convert.ToInt32(dr[0].ToString());
                participant.Name = dr[1].ToString();
                participant.Kebele = dr[2].ToString();
                participant.Woreda = dr[3].ToString();
                participant.Sex = dr[4].ToString();
                participant.Age = Convert.ToInt32(dr[5].ToString());
                participant.Disabled = Convert.ToInt32(dr[6].ToString());
                participant.ActivityId= Convert.ToInt32(dr[7].ToString());

                if(activityType==2)//Activity2
                {
                    ((Participant2)participant).Gradelevel = dr[11].ToString();
                }
                else if (activityType == 3)//Activity3
                {
                    ((Participant3)participant).Organization = dr[11].ToString();
                    ((Participant3)participant).Position = dr[12].ToString();
                }

                else if (activityType == 4)//Activity4
                {
                    ((Participant4)participant).Role = dr[11].ToString();
                }

                else if (activityType == 5)//Activity5
                {
                    ((Participant5)participant).Pdate = dr.GetDateTime(11);
                    ((Participant5)participant).Code = dr[12].ToString();
                    ((Participant5)participant).Town= dr[13].ToString();
                    ((Participant5)participant).Dateofvisit = dr.GetDateTime(14);
                    ((Participant5)participant).Identifiedcase = dr[15].ToString();
                    ((Participant5)participant).Referralhealth = Convert.ToInt32(dr[16].ToString());
                    ((Participant5)participant).Referralpolice = Convert.ToInt32(dr[17].ToString());
                    ((Participant5)participant).Referrallegal = Convert.ToInt32(dr[18].ToString());
                    ((Participant5)participant).Referralpsycho = Convert.ToInt32(dr[19].ToString());
                    ((Participant5)participant).Others = dr[20].ToString();
                }
                else if (activityType == 6)//Activity6
                {
                   
                }
                else if (activityType == 7)//Activity7
                {
                    ((Participant7)participant).Groupname= dr[11].ToString();
                    ((Participant7)participant).Noofadolescent = Convert.ToInt32(dr[12].ToString());
                    ((Participant7)participant).Noofwomen = Convert.ToInt32(dr[13].ToString());
                    ((Participant7)participant).Typeoftraining = dr[14].ToString();
                    ((Participant7)participant).Capital = dr[15].ToString();
                    ((Participant7)participant).Typeofiga = dr[16].ToString();
                    ((Participant7)participant).Statusofiga = dr[17].ToString();
                    ((Participant7)participant).Amountofsaving = dr[18].ToString();
                    ((Participant7)participant).Remark = dr[19].ToString();
                }
                else if (activityType == 8)//Activity8
                {
                    ((Participant8)participant).Gradelevel = dr[11].ToString();
                    ((Participant8)participant).Statusofretention = dr[12].ToString();
                    ((Participant8)participant).Remark = dr[13].ToString();
                }
                else if (activityType == 9)//Activity9
                {
                    ((Participant9)participant).Noofadolescent = Convert.ToInt32(dr[11].ToString());
                    ((Participant9)participant).Noofwomen = Convert.ToInt32(dr[12].ToString());
                    ((Participant9)participant).Typeofmaterial = dr[13].ToString();
                    ((Participant9)participant).Unit = dr[14].ToString();
                    ((Participant9)participant).Quantity= Convert.ToInt32(dr[15].ToString()); 
                }

                participantList.Add(participant);
            }
            connection.Close();
            dr.Close();
            return participantList;
        }

        public List<Participant> GetParticpantData(int activityType, int activityId)
        {
            Participant participant = null;
            List<Participant> participantList = new List<Participant>() ;
            string tableName = "Participants"+activityType;
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM " + tableName + " WHERE [activityid]=" + activityId;
            connection.Open();

            OleDbDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                if (activityType == 1)
                    participant = new Participant1();
                else if (activityType == 2)
                    participant = new Participant2();
                else if (activityType == 3)
                    participant = new Participant3();
                else if (activityType == 4)
                    participant = new Participant4();
                else if (activityType == 5)
                    participant = new Participant5();
                else if (activityType == 6)
                    participant = new Participant6();
                else if (activityType == 7)
                    participant = new Participant7();
                else if (activityType == 8)
                    participant = new Participant8();
                else if (activityType == 9)
                    participant = new Participant9();

                participant.Pid = Convert.ToInt32(dr[0].ToString());
                participant.Name = dr[1].ToString();
                participant.Kebele = dr[2].ToString();
                participant.Woreda = dr[3].ToString();
                participant.Sex = dr[4].ToString();
                participant.Age = Convert.ToInt32(dr[5].ToString());
                participant.Disabled = Convert.ToInt32(dr[6].ToString());
                participant.ActivityId = Convert.ToInt32(dr[7].ToString());

                if (activityType == 2)//Activity2
                {
                    ((Participant2)participant).Gradelevel = dr[11].ToString();
                }
                else if (activityType == 3)//Activity3
                {
                    ((Participant3)participant).Organization = dr[11].ToString();
                    ((Participant3)participant).Position = dr[12].ToString();
                }
                else if (activityType == 4)//Activity4
                {
                    ((Participant4)participant).Role = dr[11].ToString();
                }
                else if (activityType == 5)//Activity5
                {
                    ((Participant5)participant).Pdate = dr.GetDateTime(11);
                    ((Participant5)participant).Code = dr[12].ToString();
                    ((Participant5)participant).Town = dr[13].ToString();
                    ((Participant5)participant).Dateofvisit = dr.GetDateTime(14);
                    ((Participant5)participant).Identifiedcase = dr[15].ToString();
                    ((Participant5)participant).Referralhealth = Convert.ToInt32(dr[16].ToString());
                    ((Participant5)participant).Referralpolice = Convert.ToInt32(dr[17].ToString());
                    ((Participant5)participant).Referrallegal = Convert.ToInt32(dr[18].ToString());
                    ((Participant5)participant).Referralpsycho = Convert.ToInt32(dr[19].ToString());
                    ((Participant5)participant).Others = dr[20].ToString();
                }
                else if (activityType == 6)//Activity6
                {

                }
                else if (activityType == 7)//Activity7
                {
                    ((Participant7)participant).Groupname = dr[11].ToString();
                    ((Participant7)participant).Noofadolescent = Convert.ToInt32(dr[12].ToString());
                    ((Participant7)participant).Noofwomen = Convert.ToInt32(dr[13].ToString());
                    ((Participant7)participant).Typeoftraining = dr[14].ToString();
                    ((Participant7)participant).Capital = dr[15].ToString();
                    ((Participant7)participant).Typeofiga = dr[16].ToString();
                    ((Participant7)participant).Statusofiga = dr[17].ToString();
                    ((Participant7)participant).Amountofsaving = dr[18].ToString();
                    ((Participant7)participant).Remark = dr[19].ToString();
                }
                else if (activityType == 8)//Activity7
                {
                    ((Participant8)participant).Gradelevel = dr[11].ToString();
                    ((Participant8)participant).Statusofretention = dr[12].ToString();
                    ((Participant8)participant).Remark = dr[13].ToString();
                }
                else if (activityType == 9)//Activity9
                {
                    ((Participant9)participant).Noofadolescent = Convert.ToInt32(dr[11].ToString());
                    ((Participant9)participant).Noofwomen = Convert.ToInt32(dr[12].ToString());
                    ((Participant9)participant).Typeofmaterial = dr[13].ToString();
                    ((Participant9)participant).Unit = dr[14].ToString();
                    ((Participant9)participant).Quantity = Convert.ToInt32(dr[15].ToString());
                }

                participantList.Add(participant);
            }
            connection.Close();
            dr.Close();
            return participantList;
        }

        public int DeleteActivity(int activityType, int activityId)
        {
            int resultValue = -1;
            try {
                    string tableName = "Activity"+activityType;
                    connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
                    command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM " + tableName + " WHERE ID=" + activityId;
                    connection.Open();
                    resultValue = command.ExecuteNonQuery();
                    connection.Close();

                    if (resultValue > 0)//Row affected
                    {
                        tableName = "Participants"+activityType;
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

        public int SendParticipantsListToServer(int activityType, List<Participant> participantsList)
        {
            int syncSuccessful = -1;

            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;
                
                for (int i = 0; i < participantsList.Count; i++)
                {
                    Participant participant = participantsList[i];
                    syncSuccessful = InsertParticipant(activityType, participant, myConnection);
                }
                mySqlHelper.CloseConnection();
            }

            return syncSuccessful;
        }

        public int SendActivityListToServer(int activityType, List<Activity0> activityList)
        {
            int syncSuccessful = -1;

            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;
               
                for (int i = 0; i < activityList.Count; i++)
                {
                    Activity0 activity = activityList[i];
                    syncSuccessful=InsertActivity(activityType, activity, myConnection);
                }
                mySqlHelper.CloseConnection();
            }

            return syncSuccessful;
        }


        public int InsertActivity(int activityType, Activity0 activity, MySqlConnection myConnection)
        {
            string firstStageCommand = "";
            string secondStageCommand = "";
            string tableName = "Activity" + activityType;
            try
            {
               MySqlCommand myCommand = myConnection.CreateCommand();
               //myCommand.CommandText = "INSERT INTO "+tableName+" (`region`, `zone`, `woreda`, `kebele`, `activity_date`, `submission_date`, `user_id`, `facilitator_name`, `position`, `issues_raised`, `agreed_action_points`,`localtimestamp`, `mac`, `place`, `duration`) VALUES(@Region, @Zone, @Woreda, @Kebele, '" + activity.ActivityDate.ToString("yyyy-MM-dd H:mm:ss") + "', '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") +"', @UserId, @FacilitatorName, @Position, @IssuesRaised, @AgreedActionPoints, '" + activity.LocalTimeStamp.ToString("yyyy-MM-dd H:mm:ss") + "', @Mac, @Place, @Duration);";

               firstStageCommand = "INSERT INTO "+tableName+" (`region`, `zone`, `woreda`, `kebele`, `activity_date`, `submission_date`, `user_id`, `facilitator_name`, `position`, `issues_raised`, `agreed_action_points`,`localtimestamp`, `mac`";
               secondStageCommand = " VALUES(@Region, @Zone, @Woreda, @Kebele, '" + activity.ActivityDate.ToString("yyyy-MM-dd H:mm:ss") + "', '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") +"', @UserId, @FacilitatorName, @Position, @IssuesRaised, @AgreedActionPoints, '" + activity.LocalTimeStamp.ToString("yyyy-MM-dd H:mm:ss") + "', @Mac";


               //////////////////////////////////////////////////////////////////////////////
               if (activityType == 1)//if Activity1
               {
                   firstStageCommand += ", `place`, `duration`)";
                   secondStageCommand += ", @Place, @Duration);";
               }
               else if (activityType == 2)//if Activiiy2
               {
                   firstStageCommand += ", `schoolname`, `clubname`, `clubleadername`)";
                   secondStageCommand += ", @schoolname, @clubname, @clubleadername);";
               }
               else if (activityType == 3)//if Activiiy3
               {
                   firstStageCommand += ", `trainingtitle`, `trainingfacilitator`)";
                   secondStageCommand += ", @trainingtitle, @trainingfacilitator);";
               }
               else if (activityType == 4)//if Activiiy4
               {
                   firstStageCommand += ", `stage`, `ccfacilitatorname`, `duration`)";
                   secondStageCommand += ", @stage, @ccfacilitatorname, @duration);";
               }
               else if (activityType == 5)//if Activiiy5
               {
                   firstStageCommand += ", `ryear`)";
                   secondStageCommand += ", @ryear);";
               }
               else if (activityType == 6)//if Activiiy6
               {
                   firstStageCommand += ", `place`)";
                   secondStageCommand += ", @place);";
               }
               else if (activityType == 7)//if Activiiy7
               {
                   firstStageCommand += ", `totalnumberofmembers`)";
                   secondStageCommand += ", @totalnumberofmembers);";
               }
               else if (activityType == 8)//if Activiiy8
               {
                   firstStageCommand += ", `ayear`, `schoolname`)";
                   secondStageCommand += ", @ayear, @schoolname);";
               }
               else if (activityType == 9)//if Activiiy9
               {
                   firstStageCommand += ")";
                   secondStageCommand += ");";
               }
               else
               {
                   firstStageCommand += ")";
                   secondStageCommand += ");";
               }
               //////////////////////////////////////////////////////////////////////////////

               myCommand.CommandText = firstStageCommand + secondStageCommand;

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


                //////////////////////////////////////////////////////////////////////////////
                if (activityType == 1)
                {
                    myCommand.Parameters.AddWithValue("@Place", ((Activity1)activity).Place);
                    myCommand.Parameters.AddWithValue("@Duration", ((Activity1)activity).Duration);
                }
                else if (activityType == 2)
                {
                    myCommand.Parameters.AddWithValue("@schoolname", ((Activity2)activity).Schoolname);
                    myCommand.Parameters.AddWithValue("@clubname", ((Activity2)activity).Clubname);
                    myCommand.Parameters.AddWithValue("@clubleadername", ((Activity2)activity).Clubleadername);
                }
                else if (activityType == 3)
                {
                    myCommand.Parameters.AddWithValue("@trainingtitle", ((Activity3)activity).Trainingtitle);
                    myCommand.Parameters.AddWithValue("@trainingfacilitator", ((Activity3)activity).Trainingfacilitator);
                }
                else if (activityType == 4)
                {
                    myCommand.Parameters.AddWithValue("@stage", ((Activity4)activity).Stage);
                    myCommand.Parameters.AddWithValue("@ccfacilitatorname", ((Activity4)activity).Ccfacilitatorname);
                    myCommand.Parameters.AddWithValue("@duration", ((Activity4)activity).Duration);
                }
                else if (activityType == 5)
                {
                    myCommand.Parameters.AddWithValue("@ryear", ((Activity5)activity).Year);
                }
                else if (activityType == 6)
                {
                    myCommand.Parameters.AddWithValue("@place", ((Activity6)activity).Place);
                }

                else if (activityType == 7)
                {
                    myCommand.Parameters.AddWithValue("@totalnumberofmembers", ((Activity7)activity).Totalnumberofmembers);
                }
                else if (activityType == 8)
                {
                    myCommand.Parameters.AddWithValue("@ayear", ((Activity8)activity).Year);
                    myCommand.Parameters.AddWithValue("@schoolname", ((Activity8)activity).Schoolname);
                }
                else if (activityType == 9)
                {

                }
                //////////////////////////////////////////////////////////////////////////////
               
                
                //ID=Convert.ToInt32(command.ExecuteScalar());
                myCommand.ExecuteNonQuery();

                myCommand.CommandText = "SELECT @@IDENTITY";
                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    activityRemoteId = Convert.ToInt32(dr[0].ToString());
                }
                //update local database
                UpdateLocalActivity(activityType, activity, activityRemoteId);
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


        public int InsertParticipant(int activityType, Participant participant, MySqlConnection myConnection)
        {
            int participantRemoteId = -1;
            string firstStageCommand = "";
            string secondStageCommand = "";
            string tableName = "Participants" + activityType;
            try
            {
                MySqlCommand myCommand = myConnection.CreateCommand();
                firstStageCommand = "INSERT INTO "+tableName+" (`pname`, `kebele`, `woreda`, `sex`, `age`, `disabled`, `activityid`, `remoteactivityid`";
                secondStageCommand = " VALUES(@PName, @Kebele, @Woreda, @Sex, @Age, @Disabled, @ActivityId, @RemoteActivityId";


                //////////////////////////////////////////////////////////////////////////////
                if (activityType == 1)//if Activity1
                {
                    firstStageCommand += ")";
                    secondStageCommand += ");";
                }
                else if (activityType == 2)//if Activiiy2
                {
                    firstStageCommand += ", `gradelevel`)";
                    secondStageCommand += ", @gradelevel);";
                }
                else if (activityType == 3)//if Activiiy3
                {
                    firstStageCommand += ", `organization`, `position`)";
                    secondStageCommand += ", @organization, @position);";
                }
                else if (activityType == 4)//if Activiiy4
                {
                    firstStageCommand += ", `role`)";
                    secondStageCommand += ", @role);";
                }
                else if (activityType == 5)//if Activiiy5
                {
                    firstStageCommand += ", `pdate`, `code`, `town`, `dateofvisit`, `identifiedcase`, `referralhealth`, `referralpolice`, `referrallegal`, `referralpsycho`, `others`)";
                    secondStageCommand += ", @pdate, @code, @town, @dateofvisit, @identifiedcase, @referralhealth, @referralpolice, @referrallegal, @referralpsycho, @others);";
                }

                else if (activityType == 6)//if Activiiy6
                {
                    firstStageCommand += ")";
                    secondStageCommand += ");";
                }

                else if (activityType == 7)//if Activiiy7
                {
                    firstStageCommand += ", `groupname`, `noofadolescent`, `noofwomen`, `typeoftraining`, `capital`, `typeofiga`, `statusofiga`, `amountofsaving`, `remark`)";
                    secondStageCommand += ", @groupname, @noofadolescent, @noofwomen, @typeoftraining, @capital, @typeofiga, @statusofiga, @amountofsaving, @remark);";
                }
                else if (activityType == 8)//if Activiiy8
                {
                    firstStageCommand += ", `gradelevel`, `statusofretention`, `remark`)";
                    secondStageCommand += ", @gradelevel, @statusofretention, @remark);";
                }
                else if (activityType == 9)//if Activiiy9
                {
                    firstStageCommand += ", `noofadolescent`, `noofwomen`, `typeofmaterial`, `unit`, `quantity`)";
                    secondStageCommand += ", @noofadolescent, @noofwomen, @typeofmaterial, @unit, @quantity);";
                }
                else
                {
                    firstStageCommand += ")";
                    secondStageCommand += ");";
                }
                //////////////////////////////////////////////////////////////////////////////

                myCommand.CommandText = firstStageCommand + secondStageCommand;

                if (participant.Name!=null)
                    myCommand.Parameters.AddWithValue("@PName", participant.Name);
                else
                    myCommand.Parameters.AddWithValue("@PName", "");

                if (participant.Kebele != null)
                    myCommand.Parameters.AddWithValue("@Kebele", participant.Kebele);
                else
                    myCommand.Parameters.AddWithValue("@Kebele", "");

                if (participant.Woreda != null)
                    myCommand.Parameters.AddWithValue("@Woreda", participant.Woreda);
                else
                    myCommand.Parameters.AddWithValue("@Woreda", "");


                myCommand.Parameters.AddWithValue("@Sex", participant.Sex);
                myCommand.Parameters.AddWithValue("@Age", participant.Age);
                myCommand.Parameters.AddWithValue("@Disabled", participant.Disabled);
                myCommand.Parameters.AddWithValue("@ActivityId", participant.ActivityId);
                myCommand.Parameters.AddWithValue("@RemoteActivityId", activityRemoteId);

                //////////////////////////////////////////////////////////////////////////////
                if (activityType == 2)//if Activiiy2
                {
                    myCommand.Parameters.AddWithValue("@gradelevel", ((Participant2)participant).Gradelevel);
                }
                else if (activityType == 3)//if Activiiy3
                {
                    myCommand.Parameters.AddWithValue("@organization", ((Participant3)participant).Organization);
                    myCommand.Parameters.AddWithValue("@position", ((Participant3)participant).Position);
                }
                else if (activityType == 4)//if Activiiy4
                {
                    myCommand.Parameters.AddWithValue("@role", ((Participant4)participant).Role);
                }
                else if (activityType == 5)//if Activiiy5
                {
                    myCommand.Parameters.AddWithValue("@pdate", ((Participant5)participant).Pdate);
                    myCommand.Parameters.AddWithValue("@code", ((Participant5)participant).Code);
                    myCommand.Parameters.AddWithValue("@town", ((Participant5)participant).Town);
                    myCommand.Parameters.AddWithValue("@dateofvisit", ((Participant5)participant).Dateofvisit);
                    myCommand.Parameters.AddWithValue("@identifiedcase", ((Participant5)participant).Identifiedcase);
                    myCommand.Parameters.AddWithValue("@referralhealth", ((Participant5)participant).Referralhealth);
                    myCommand.Parameters.AddWithValue("@referralpolice", ((Participant5)participant).Referralpolice);
                    myCommand.Parameters.AddWithValue("@referrallegal", ((Participant5)participant).Referrallegal);
                    myCommand.Parameters.AddWithValue("@referralpsycho", ((Participant5)participant).Referralpsycho);
                    myCommand.Parameters.AddWithValue("@others", ((Participant5)participant).Others);
                }

                else if (activityType == 6)//if Activiiy6
                {
                    
                }

                else if (activityType == 7)//if Activiiy7
                {
                    myCommand.Parameters.AddWithValue("@groupname", ((Participant7)participant).Groupname);
                    myCommand.Parameters.AddWithValue("@noofadolescent", ((Participant7)participant).Noofadolescent);
                    myCommand.Parameters.AddWithValue("@noofwomen", ((Participant7)participant).Noofwomen);
                    myCommand.Parameters.AddWithValue("@typeoftraining", ((Participant7)participant).Typeoftraining);
                    myCommand.Parameters.AddWithValue("@capital", ((Participant7)participant).Capital);
                    myCommand.Parameters.AddWithValue("@typeofiga", ((Participant7)participant).Typeofiga);
                    myCommand.Parameters.AddWithValue("@statusofiga", ((Participant7)participant).Statusofiga);
                    myCommand.Parameters.AddWithValue("@amountofsaving", ((Participant7)participant).Amountofsaving);
                    myCommand.Parameters.AddWithValue("@remark", ((Participant7)participant).Remark);
                }
                else if (activityType == 8)//if Activiiy8
                {
                    myCommand.Parameters.AddWithValue("@gradelevel", ((Participant8)participant).Gradelevel);
                    myCommand.Parameters.AddWithValue("@statusofretention", ((Participant8)participant).Statusofretention);
                    myCommand.Parameters.AddWithValue("@remark", ((Participant8)participant).Remark);
                }
                else if (activityType == 9)//if Activiiy9
                {
                    myCommand.Parameters.AddWithValue("@noofadolescent", ((Participant9)participant).Noofadolescent);
                    myCommand.Parameters.AddWithValue("@noofwomen", ((Participant9)participant).Noofwomen);
                    myCommand.Parameters.AddWithValue("@typeofmaterial", ((Participant9)participant).Typeofmaterial);
                    myCommand.Parameters.AddWithValue("@unit", ((Participant9)participant).Unit);
                    myCommand.Parameters.AddWithValue("@quantity", ((Participant9)participant).Quantity);
                }
                //////////////////////////////////////////////////////////////////////////////


                myCommand.ExecuteNonQuery();

                myCommand.CommandText = "SELECT @@IDENTITY";
                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                   participantRemoteId = Convert.ToInt32(dr[0].ToString());
                }
                //update local database
                UpdateLocalParticipant(activityType, participant, participantRemoteId);
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

        private void UpdateLocalParticipant(int activityType, Participant participant, int participantRemoteId)
        {
            //make sync_status to 1
            //change remote id to new remote id
            string tableName = "Participants"+activityType;
            OleDbConnection connection2 = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            OleDbCommand command2 = connection2.CreateCommand();
            command2.CommandText = "UPDATE " + tableName + " SET [sync_status]=1, [remoteactivityid]=@RemoteActivityId, [remoteparticipantid]=@RemoteParticipantId WHERE ID=" + participant.Pid;
            command2.Parameters.AddWithValue("@RemoteActivityId", activityRemoteId);
            command2.Parameters.AddWithValue("@RemoteParticipantId", participantRemoteId);
            
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();

        }

        private void UpdateLocalActivity(int activityType, Activity0 activity, int activityRemoteId)
        {
            //make sync_status to 1
            //change remote id to new remote id
            string tableName = "Activity"+activityType;
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
