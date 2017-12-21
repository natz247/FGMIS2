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
    public class ReportSelectorHelper
    {
        OleDbConnection connection;
        OleDbCommand command;

        private void ConnectTo()
        {
            connection = new OleDbConnection(Properties.Settings.Default.DatabaseAddress);
            command =connection.CreateCommand();
        }

        public ReportSelectorHelper()
        {
            ConnectTo();
        }

        public void Insert(Report report)
        {
            try
            {
                command.CommandText = "INSERT INTO Activities (title) VALUES('"+report.Title+ "')";
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

        public List<Report> GetReportList()
        {
            List<Report> reportList = new List<Report>();
            try
            {
                command.CommandText = "SELECT * FROM Reports";
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Report report = new Report();
                    report.Aid = Convert.ToInt32(reader["ID"].ToString());
                    report.Title = reader["title"].ToString();
                    report.TableName = reader["tablename"].ToString();

                    reportList.Add(report);
                }
                return reportList;
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


        public List<ReportListItem> GetActivityList(string tableName)
        {
            List<ReportListItem> reportList = new List<ReportListItem>();
            try
            {
                command.CommandText = "SELECT * FROM "+tableName+" where sync_status=0";
                command.CommandType = CommandType.Text;
                connection.Open();

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ReportListItem report = new ReportListItem();
                    report.Aid = Convert.ToInt32(reader["ID"].ToString());
                    report.LocalTimeStamp = reader.GetDateTime(11);
                    report.Title = reader["region"].ToString() + ", " + reader["zone"].ToString() + ", " + reader["woreda"].ToString();
                    report.TableName = tableName;

                    reportList.Add(report);
                }
                return reportList;
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
