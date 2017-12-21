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
    public class PreviousYearDataHelper
    {
        public static int DATABASE_TARGET = 0;
        public static int DATABASE_DATA = 1;
        private int databaseIndex = DATABASE_DATA;
        
        public PreviousYearDataHelper(int databaseIndex)
        {
            this.databaseIndex = databaseIndex;
        }
        

        public int UpdateYearsData(PreviousYear previousYear)
        {
            string tableName = "years";
            if (databaseIndex == DATABASE_DATA)
                tableName += "data";
            else
                tableName += "target";

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
                    myCommand.CommandText = "UPDATE " + tableName + " SET `output11`=@output11, `output12`=@output12,`output13`=@output13,`output21`=@output21,`output22`=@output22,`output23`=@output23,`output24`=@output24,`output25`=@output25,`output31`=@output31,`output32`=@output32, `uid`=@uid WHERE `oyear`=@year";
                    myCommand.Parameters.AddWithValue("@output11", previousYear.Output11);
                    myCommand.Parameters.AddWithValue("@output12", previousYear.Output12);
                    myCommand.Parameters.AddWithValue("@output13", previousYear.Output13);
                    myCommand.Parameters.AddWithValue("@output21", previousYear.Output21);
                    myCommand.Parameters.AddWithValue("@output22", previousYear.Output22);
                    myCommand.Parameters.AddWithValue("@output23", previousYear.Output23);
                    myCommand.Parameters.AddWithValue("@output24", previousYear.Output24);
                    myCommand.Parameters.AddWithValue("@output25", previousYear.Output25);
                    myCommand.Parameters.AddWithValue("@output31", previousYear.Output31);
                    myCommand.Parameters.AddWithValue("@output32", previousYear.Output32);
                    myCommand.Parameters.AddWithValue("@uid", previousYear.Uid);
                    myCommand.Parameters.AddWithValue("@year", previousYear.Year);

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

        public PreviousYear GetPreviousYearData(int year)
        {
            string tableName = "years";
            if (databaseIndex == DATABASE_DATA)
                tableName += "data";
            else
                tableName += "target";

            MySqlCommand myCommand;
            //connect to remote database
            MySqlHelper mySqlHelper = new MySqlHelper();
            PreviousYear previousYear = new PreviousYear();
                    
            if (mySqlHelper.OpenConnection())
            {
                MySqlConnection myConnection = mySqlHelper.Connection;

                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT * FROM " + tableName + " WHERE `oyear`="+year;
                Organization organization = null;

                MySqlDataReader dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    previousYear.Pid = Convert.ToInt32(dr[0].ToString());
                    previousYear.Year = Convert.ToInt32(dr[1].ToString());
                    previousYear.Output11 = Convert.ToInt32(dr[2].ToString());
                    previousYear.Output12 = Convert.ToInt32(dr[3].ToString());
                    previousYear.Output13 = Convert.ToInt32(dr[4].ToString());
                    previousYear.Output21 = Convert.ToInt32(dr[5].ToString());
                    previousYear.Output22 = Convert.ToInt32(dr[6].ToString());
                    previousYear.Output23 = Convert.ToInt32(dr[7].ToString());
                    previousYear.Output24 = Convert.ToInt32(dr[8].ToString());
                    previousYear.Output25 = Convert.ToInt32(dr[9].ToString());
                    previousYear.Output31 = Convert.ToInt32(dr[10].ToString());
                    previousYear.Output32 = Convert.ToInt32(dr[11].ToString());
                    previousYear.Uid= Convert.ToInt32(dr[12].ToString());

                }
                dr.Close();
                mySqlHelper.CloseConnection();
            }
            return previousYear;
        }
    }
}
