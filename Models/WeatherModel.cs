using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp_cc.Models
{
    public class WeatherModel
    {
        public string WeatherDescription { get; set; }
        public string Temperature { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}
