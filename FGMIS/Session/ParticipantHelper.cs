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
    public class ParticipantHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command=connection.CreateCommand();
        }

        public ParticipantHelper()
        {
            ConnectTo();
        }

        public void Insert(Participant user)
        {
            try
            {
                command.CommandText = "INSERT INTO Participants (pname, kebele, woreda, sex, age, disabled, activityid, localtimestamp, mac) VALUES('"+user.Name+"', '"+user.Kebele+"', '"+user.Woreda+"', '"+user.Sex+"', '"+user.Age + "', '"+user.Disabled + "', '"+user.ActivityId +"', '" +DateTime.Now+"', '"+ GetMacAddress() + "')";
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
        

        public List<Participant> GetPaticipants(int activityId)
        {
            List<Participant> participantsList = new List<Participant>();
            try
            {
                command.CommandText = "SELECT * FROM Participants WHERE activityid="+activityId;
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Participant participant = new Participant();
                    participant.Pid = Convert.ToInt32(reader["ID"].ToString());
                    participant.Name = reader["pname"].ToString();
                    participant.Kebele = reader["kebele"].ToString();
                    participant.Woreda = reader["woreda"].ToString();
                    participant.Sex = reader["sex"].ToString();
                    participant.Age = Convert.ToInt32(reader["age"].ToString());
                    participant.Disabled = Convert.ToInt32(reader["disabled"].ToString());
                    participant.ActivityId = Convert.ToInt32(reader["activityid"].ToString());
                    //participant.LocalTimeStamp = reader.GetDateTime(8);
                    //participant.Mac = reader["mac"].ToString();
                    participant.Remoteid = Convert.ToInt32(reader["remoteid"].ToString());
                    //participant.RemoteTimeStamp = reader.GetDateTime(11);
                    //participant.SyncStatus = Convert.ToInt32(reader["sync_status"].ToString());

                    participantsList.Add(participant);
                }
                return participantsList;
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
    }
}
