using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp_cc.Models
{
    public class WeatherModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
