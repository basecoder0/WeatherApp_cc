using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp_cc.Services;
using WeatherApp_cc.Models;

namespace WeatherApp_cc.Controllers
{
    public class HomeController : Controller
    {
        private const string _url = "https://api.openweathermap.org/data/2.5/";
        private const string _apiKey = "&appid=72c5e00d2fd4a038784dcac1583135aa";
        private readonly ILogger<HomeController> _logger;
        private string message ="";
        WeatherServices service = null;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Weather(IndexModel userInfo)        
        {
            if(userInfo.UserName == null)
            {
               return Redirect("Index");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult GetWeather(Rootobject model)
        {
            var apiString = GetAPIInfo(model);
            apiString.weather[0].State = model.weather[0].State;
            apiString.main.temp = service.KelvinToFahrenheit(apiString.main.temp);
            ViewData["WeatherAtt"] = apiString;
            //Work on elegant error handling
            return View("~/Views/Home/Weather.cshtml");
        }
              
        private Rootobject GetAPIInfo(Rootobject weatherAtt)
        {
            service = new WeatherServices(_url, weatherAtt, _apiKey);
            var requestStr = service.BuildApiRequest();
            var json = service.GetWeatherApi(requestStr);
            return json;
        }

        [HttpPost]
        public ActionResult PostUserInfo(SignUpModel userInfo)
        {           
            service = new WeatherServices();
            message = service.InsertUserInfo(userInfo);
            if(message != "Success")
            {
                ViewData["Message"] = message;
                return View("~/Views/Home/SignUp.cshtml");
            }
            else
            {
                ViewData["Message"] = message;
                return View("~/Views/Home/Weather.cshtml");
            }            
        }

        [HttpGet]
        public ActionResult GetUserCredentials(IndexModel userInfo)
        {
            service = new WeatherServices();
            message = service.GetUserCredentials(userInfo);
            if (!message.Contains("Welcome"))
            {
                ViewData["Message"] = message;
                return View("Index");
            }
            else
            {
                ViewData["Login"] = "Success";
                ViewData["Message"] = message;
                return View("~/Views/Home/Weather.cshtml");
            }
        }

    }
}
