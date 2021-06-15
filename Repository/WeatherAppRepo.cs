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
        public string  InsertUserInfo(SignUpModel userInfo)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand command;

            string myConnectionString = ConfigurationManager.ConnectionStrings["awsDB"].ConnectionString;

            conn = new MySql.Data.MySqlClient.MySqlConnection();
            command = new MySql.Data.MySqlClient.MySqlCommand();

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
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                return ex.ToString();
            }
            conn.Close();
            string success = "Success";
            return success;
        }
    }
}
