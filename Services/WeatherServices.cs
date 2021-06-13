using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WeatherApp_cc.Models;
using RestSharp;
using Newtonsoft.Json;

namespace WeatherApp_cc.Services
{
    public class WeatherServices
    {
        private string baseUrl;
        private string query;
        private string key;

        public WeatherServices(string baseUrl, WeatherModel model, string key)
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

        public string BuildApiRequest()
        {
            string requestStr = "";
            string qStr = "weather?q=";
            StringBuilder reqStr = new StringBuilder();
            reqStr.AppendFormat(requestStr +"{0}{1}{2}", qStr, query, key);
            return reqStr.ToString();
        }

        public Rootobject getWeatherApi(string requestString)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(requestString);
            var response = client.Execute(request);

            Rootobject result = null;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                result = JsonConvert.DeserializeObject<Rootobject>(rawResponse);                
            }

            return result;
        }
        
    }
}
