using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp_cc.Models;
using WeatherApp_cc.Repository;
using RestSharp;
using Newtonsoft.Json;

namespace WeatherApp_cc.Services
{
    /**Service Class that contains all pipelined / transformation logic  
     * between the controller and the repository layers
    **/
    public class WeatherServices
    {
        private  string _baseUrl;
        private string _query;
        private string _key;
        private WeatherAppRepo _repo = new WeatherAppRepo();

        public WeatherServices() { }

        public WeatherServices(string baseUrl, WeatherInfoModel model, string key)
        {
            if (model.City != "" && model.State != "")
                this._query = model.City + "," + model.State;
            else if (model.City != "")
                this._query = model.City;
            else if (model.State != "")
                this._query = model.State;

            this._baseUrl = baseUrl;
            this._key = key;
        }

        public WeatherServices(string baseUrl, Rootobject model, string key)
        {
            if (model.Weather[0].City != "" && model.Weather[0].State != "")
                this._query = model.Weather[0].City + "," + model.Weather[0].State;
            else if (model.Weather[0].City != "")
                this._query = model.Weather[0].City;
            else if (model.Weather[0].State != "")
                this._query = model.Weather[0].State;

            this._baseUrl = baseUrl;
            this._key = key;
        }
        /**
         * Builds api string
         **/
        public string BuildApiRequest()
        {
            string requestStr = "";
            string qStr = "weather?q=";
            StringBuilder reqStr = new StringBuilder();
            reqStr.AppendFormat(requestStr + "{0}{1}{2}", qStr, _query, _key);
            return reqStr.ToString();
        }

        public void DeleteWeatherInfo(string[] key)
        {
            _repo.DeleteWeatherInfo(key);
        }

        /**
         * Takes api string and calls api for weather info
         **/
        public Rootobject GetWeatherApi(string requestString, string city = "", string state = "")
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(requestString);
            var response = client.Execute(request);
            Rootobject result = null;
            var weatherRes = new Weather()
            {
                City = city,
                State = state
            };

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                result = JsonConvert.DeserializeObject<Rootobject>(rawResponse);
            }
            else
            {
                result = new Rootobject();
                result.ErrorMessage = response.StatusCode.ToString();
                return result;
            }

            result.Weather[0].City = weatherRes.City;
            result.Weather[0].State = weatherRes.State;
            return result;
        }

        public string GetUserCredentials(IndexModel userInfo)
        {
            return _repo.GetUserCredentials(userInfo);
        }

        /**
         * Returns makeshift "key" based on user_id, longitude and latitude
         **/
        public string[] GetKey(string id)
        {
            string[] words;
            words = id.Split("'_'");
            return words;
        }

        public string GetUserId(string userName)
        {
            return _repo.GetUserId(userName);
        }

        public List<WeatherInfoModel> GetUserSignUpLoc(int userId)
        {
            return _repo.GetUserSignUpLoc(userId);
        }
        public List<WeatherInfoModel> GetWeatherInfo(int userId)
        {
            return _repo.GetWeatherInfo(userId);
        }

        public string InsertUserInfo(SignUpModel userInfo)
        {
            return _repo.InsertUserInfo(userInfo);
        }

        public void InsertWeatherInfo(Rootobject model)
        {
            _repo.InsertWeatherInfo(model);
        }

        /***
         * Converts from kelvin to fahrenheit
         * **/
        public string KelvinToFahrenheit(double temp)
        {
            double fahrenheit = ((temp - 273.15) * 9 / 5) + 32;
            return fahrenheit.ToString("00");
        }

        public void UpdateWeatherInfo(WeatherInfoModel model)
        {
            _repo.UpdateWeatherInfo(model);
        }

        public bool UserExist(string userName)
        {
            return _repo.UserExist(userName);
        }

        /**
         * Helper method to create new JSON obj
         **/
        public WeatherInfoModel CreateNewJSONObj(Rootobject json)
        {
            WeatherInfoModel info = new WeatherInfoModel()
            {
                City = json.Weather[0].City,
                State = json.Weather[0].State,
                Latitude = json.Coord.Lat,
                Longitude = json.Coord.Lon,
                WeatherDescription = json.Weather[0].Description,

            };
            var temp = KelvinToFahrenheit(json.Main.Temp);
            info.Temperature = Convert.ToDouble(temp);
            return info;
        }
    }
}
