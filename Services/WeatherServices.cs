using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WeatherApp_cc.Models;

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
            string qStr = "?q=";
            StringBuilder reqStr = new StringBuilder();
            reqStr.AppendFormat(baseUrl, qStr, query, key);
            return reqStr.ToString();
        }

        // Create method to return JSON object 
    }
}
