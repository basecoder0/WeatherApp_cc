using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using WeatherApp_cc.Models;


namespace WeatherApp_cc.Repository
{
    /** Repository Class that contains all repository logic each
     * method corresponds to a stored procedure in the database      
     **/
    public class WeatherAppRepo
    {
        public WeatherAppRepo() { }
        private const string myConnectionString = "";
       

        public void DeleteWeatherInfo(string[] key)
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
                command.CommandText = "DeleteWeatherInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_id", Convert.ToInt16(key[0]));
                command.Parameters.AddWithValue("@lon", decimal.Parse(key[1].ToString()));
                command.Parameters.AddWithValue("@lat", decimal.Parse(key[2].ToString()));
                command.ExecuteScalar();               
            }
            catch (MySqlException ex)
            {
                 ex.Message.ToString();
            }
            conn.Close();
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
                if (Convert.ToInt16(result) > 0)
                {
                    exists = true;
                }
            }
            catch (MySqlException ex)
            {
                return ex.Message.ToString();
            }
            conn.Close();

            if (exists == true)
            {
                return message = userInfo.UserName;
            }
            else
            {
                return message = "Please enter a valid user name";
            }
        }

        public string GetUserId(string userName)
        {
            DataTable dt = new DataTable();
            string userId = "";
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserId";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_Name", userName);
              
                dt.Load(command.ExecuteReader());
                if(dt.Rows.Count > 0)
                {
                    userId = dt.Rows[0]["UserId"].ToString();
                }              
            }
            catch (MySqlException ex)
            {
                return ex.Message.ToString();
            }
            conn.Close();
            return userId;
        }

        public bool UserExist(string userName)
        {
            bool userExist = false;

            DataTable dt = new DataTable();            
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserId";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_Name", userName);

                dt.Load(command.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    return userExist = true;
                }
            }
            catch (MySqlException ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
            return userExist;
        }

        public List<WeatherInfoModel> GetWeatherInfo(int userId)
        {
            List<WeatherInfoModel> weatherInfo = new List<WeatherInfoModel>();
            DataTable dt = new DataTable();            
            MySqlConnection conn = null;
            MySqlCommand command = null;           
            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetWeatherInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_id", userId);               
                command.ExecuteNonQuery();
               
                dt.Load(command.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    weatherInfo = CommonMethod.ConvertToList<WeatherInfoModel>(dt);
                }
                else
                {
                    return null;
                }
            }
            catch (MySqlException ex)
            {
                 ex.Message.ToString();
            }
            conn.Close();
            return weatherInfo;
        }

        public List<WeatherInfoModel> GetUserSignUpLoc(int userId)
        {
            List<WeatherInfoModel> weatherInfo = new List<WeatherInfoModel>();
            DataTable dt = new DataTable();
            MySqlConnection conn = null;
            MySqlCommand command = null;
            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserSignUpLoc";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_id", userId);
                command.ExecuteNonQuery();

                dt.Load(command.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    weatherInfo = CommonMethod.ConvertToList<WeatherInfoModel>(dt);
                }
                else
                {
                    return null;
                }
            }
            catch (MySqlException ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
            return weatherInfo;
        }


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

        public void InsertWeatherInfo(Rootobject model)
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
                command.CommandText = "InsertWeatherInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_ID", model.user_id);
                command.Parameters.AddWithValue("@lon", model.coord.lon);
                command.Parameters.AddWithValue("@lat", model.coord.lat);
                command.Parameters.AddWithValue("@city", model.weather[0].City);
                command.Parameters.AddWithValue("@state", model.weather[0].State);               
                command.Parameters.AddWithValue("@temp", model.main.temp);
                command.Parameters.AddWithValue("@descri", model.weather[0].description);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
        }

        public void UpdateWeatherInfo(WeatherInfoModel model)
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
                command.CommandText = "UpdateWeatherInfo";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_ID",model.id);
                command.Parameters.AddWithValue("@lon", model.Longitude);
                command.Parameters.AddWithValue("@lat",model.Latitude);
                command.Parameters.AddWithValue("@temp",model.Temperature );
                command.Parameters.AddWithValue("@descri", model.WeatherDescription);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
        }        
    }
}
