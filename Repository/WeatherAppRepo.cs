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
        private const string _myConnectionString = "";
       
        //Deletes weather record based on user id, longitude and latitude of City
        public void DeleteWeatherInfo(string[] key)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();
            conn.ConnectionString = _myConnectionString;
            try
            {               
                conn.Open();
                command.Connection = conn;
                command.CommandText = "DeleteWeatherInfo";
                command.CommandType = CommandType.StoredProcedure;
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

        //Checks if username exists, returns username
        public string GetUserCredentials(IndexModel userInfo)
        {
            bool exists = false;
            string message = "";

            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserCredentials";
                command.CommandType = CommandType.StoredProcedure;
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

        //Retrieves userid 
        public string GetUserId(string userName)
        {
            DataTable dt = new DataTable();
            string userId = "";
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserId";
                command.CommandType = CommandType.StoredProcedure;
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

        // Returns bool value if user exist
        public bool UserExist(string userName)
        {
            bool userExist = false;

            DataTable dt = new DataTable();            
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserId";
                command.CommandType = CommandType.StoredProcedure;
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
        //Gets all weather records for a user based on userid
        public List<WeatherInfoModel> GetWeatherInfo(int userId)
        {
            List<WeatherInfoModel> weatherInfo = new List<WeatherInfoModel>();
            DataTable dt = new DataTable();            
            MySqlConnection conn = null;
            MySqlCommand command = null;           
            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetWeatherInfo";
                command.CommandType = CommandType.StoredProcedure;
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

        //Gets the user location from the initial sign-up
        public List<WeatherInfoModel> GetUserSignUpLoc(int userId)
        {
            List<WeatherInfoModel> weatherInfo = new List<WeatherInfoModel>();
            DataTable dt = new DataTable();
            MySqlConnection conn = null;
            MySqlCommand command = null;
            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "GetUserSignUpLoc";
                command.CommandType = CommandType.StoredProcedure;
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

        //Inserts user information from signup
        public string  InsertUserInfo(SignUpModel userInfo)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;          

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {              
                conn.Open();
                command.Connection = conn;

                command.CommandText = "InsertUserInfo";
                command.CommandType = CommandType.StoredProcedure;
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

        //Inserts Weather information using user id, longitude and latitude as a "key"
        public void InsertWeatherInfo(Rootobject model)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "InsertWeatherInfo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_ID", model.User_id);
                command.Parameters.AddWithValue("@lon", model.Coord.Lon);
                command.Parameters.AddWithValue("@lat", model.Coord.Lat);
                command.Parameters.AddWithValue("@city", model.Weather[0].City);
                command.Parameters.AddWithValue("@state", model.Weather[0].State);               
                command.Parameters.AddWithValue("@temp", model.Main.Temp);
                command.Parameters.AddWithValue("@descri", model.Weather[0].Description);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                ex.Message.ToString();
            }
            conn.Close();
        }

        //Updates weather record baseed on user id, longitude and latitude
        public void UpdateWeatherInfo(WeatherInfoModel model)
        {
            MySqlConnection conn = null;
            MySqlCommand command = null;

            conn = new MySqlConnection();
            command = new MySqlCommand();

            conn.ConnectionString = _myConnectionString;
            try
            {
                conn.Open();
                command.Connection = conn;
                command.CommandText = "UpdateWeatherInfo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@u_ID",model.Id);
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
