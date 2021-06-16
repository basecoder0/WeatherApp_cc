using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp_cc.Models;


namespace WeatherApp_cc.Repository
{
    public class WeatherAppRepo
    {
        public WeatherAppRepo() { }
        private const string myConnectionString = "server=aa1ge0iuetvkf6c.cpuwmmcmrvhq.us-east-2.rds.amazonaws.com; port=3306; database=ebdb; uid=Roah7791; pwd=gK8bqd!eSw7NheA; database=ebdb";
        public string  InsertUserInfo(SignUpModel userInfo)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;          

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {              
                conn.Open();
                command.Connection = conn;

                command.CommandText = "InsertUserInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@u_Name", userInfo.UserName);
                command.Parameters.AddWithValue("@f_Name", userInfo.FirstName);
                command.Parameters.AddWithValue("@l_Name", userInfo.LastName);
                command.Parameters.AddWithValue("@city", userInfo.City);
                command.Parameters.AddWithValue("@state", userInfo.State);
                command.Parameters.AddWithValue("@zip", userInfo.Zip);

                command.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {                
                return ex.Message.ToString();
            }
            conn.Close();
            string success = "Success";
            return success;
        }

        public string GetUserCredentials(IndexModel userInfo)
        {
            bool exists = false;
            string message = "";

            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;

                command.CommandText = "GetUserCredentials";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@u_Name", userInfo.UserName);
               
                var result = command.ExecuteScalar();
                if(Convert.ToInt16(result) > 0)
                {
                    exists = true;
                }
            }
            catch (MySqlException ex)
            {
                return ex.Message.ToString();
            }
            conn.Close();
            
            if(exists == true)
            {
                return message = String.Format("Welcome {0}", userInfo.UserName);                
            }
            else
            {                
                return message = "Please enter a valid user name";
            }           

        }
    }
}
