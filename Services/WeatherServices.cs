using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WeatherApp_cc.Models;
using WeatherApp_cc.Repository;
using RestSharp;
using Newtonsoft.Json;
using System.Data;

namespace WeatherApp_cc.Services
{
    /**Service Class that contains all pipelined / transformation logic  
     * between the Controller and the repository
    **/
    public class WeatherServices
    {
        private string baseUrl;
        private string query;
        private string key;
        WeatherAppRepo repo = new WeatherAppRepo();   

        public WeatherServices() { }

        public WeatherServices(string baseUrl, WeatherInfoModel model, string key)
        {
            if (model.City != "" && model.State != "")
                this.query = model.City + "," + model.State;
            else if (model.City != "")
                this.query = model.City;
            else if (model.State != "")
                this.query = model.State;

            this.baseUrl = baseUrl;
            this.key = key;
        }

        public WeatherServices(string baseUrl, Rootobject model, string key)
        {
            if (model.weather[0].City != "" && model.weather[0].State != "")
                this.query = model.weather[0].City + "," + model.weather[0].State;
            else if (model.weather[0].City != "")
                this.query = model.weather[0].City;
            else if (model.weather[0].State != "")
                this.query = model.weather[0].State;

            this.baseUrl = baseUrl;
            this.key = key;
        }

        public string BuildApiRequest()
        {
            string requestStr = "";
            string qStr = "weather?q=";
            StringBuilder reqStr = new StringBuilder();
            reqStr.AppendFormat(requestStr + "{0}{1}{2}", qStr, query, key);
            return reqStr.ToString();
        }

        public void DeleteWeatherInfo(string[] key)
        {
            repo.DeleteWeatherInfo(key);
        }

        public Rootobject GetWeatherApi(string requestString, string city="", string state="")
        {
            var client = new RestClient(baseUrl);
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
            result.weather[0].City = weatherRes.City;
            result.weather[0].State = weatherRes.State;
            return result;
        }

        public string GetUserCredentials(IndexModel userInfo)
        {

            return repo.GetUserCredentials(userInfo);
        }

        public string[] GetKey(string id)
        {
            string[] words;
            words = id.Split("'_'");
            return words;
        }

        public string GetUserId(string userName)
        {
            return repo.GetUserId(userName);
        }
     
        public List<WeatherInfoModel> GetUserSignUpLoc(int userId)
        {
            return repo.GetUserSignUpLoc(userId);
        }
        public List<WeatherInfoModel> GetWeatherInfo(int userId)
        {
            return repo.GetWeatherInfo(userId);
        }

        public string InsertUserInfo(SignUpModel userInfo)
        {
            return repo.InsertUserInfo(userInfo);
        }

        public void InsertWeatherInfo(Rootobject model)
        {
            repo.InsertWeatherInfo(model);
        }

        public string KelvinToFahrenheit(double temp)
        {
            double fahrenheit = ((temp - 273.15) * 9 / 5) + 32;
            return fahrenheit.ToString("00");
        }

        public void UpdateWeatherInfo(WeatherInfoModel model)
        {
            repo.UpdateWeatherInfo(model);
        }

        public bool UserExist(string userName)
        {
            return repo.UserExist(userName);
        }

        public WeatherInfoModel createNewJSONObj(Rootobject json)
        {
            WeatherInfoModel info = new WeatherInfoModel()
            {

                City = json.weather[0].City,
                State = json.weather[0].State,
                Latitude = json.coord.lat,
                Longitude = json.coord.lon,                
                Description = json.weather[0].description,

            };
            var temp = KelvinToFahrenheit(json.main.temp);
            info.Temperature = Convert.ToDouble(temp);
            return info;
        }

    }
}
