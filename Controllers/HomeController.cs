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
    /** Controller Class that handles all HTTP: POST, GET logic as well
     * as INSERT, UPDATE, DELETE logic that will be passed into the service layer
    **/
    public class HomeController : Controller
    {
        private const string _url = "https://api.openweathermap.org/data/2.5/";
        private const string _apiKey = "&appid=72c5e00d2fd4a038784dcac1583135aa";
        private readonly ILogger<HomeController> _logger;
        private string message = "";
        WeatherServices service = new WeatherServices();

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
            if (userInfo.UserName == null)
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
        public JsonResult GetWeather(Rootobject model)
        {
            string temp = "";
            var apiString = GetAPIInfo(model);
            apiString.weather[0].City = model.weather[0].City;
            apiString.weather[0].State = model.weather[0].State;

            temp = service.KelvinToFahrenheit(apiString.main.temp);
            apiString.main.temp = Convert.ToDouble(temp);
            apiString.user_id = GetUserId(model.userName);

            InsertWeatherInfo(apiString);
            //Work on elegant error handling
            return Json(apiString);
        }

        [HttpGet]
        public ActionResult GetUserCredentials(IndexModel userInfo)
        {
            if (userInfo.UserName != null && ModelState.IsValid)
            {
                message = service.GetUserCredentials(userInfo);
                string userId = service.GetUserId(userInfo.UserName);
                var weatherInfo = service.GetWeatherInfo(Convert.ToInt16(userId));
                Rootobject model = new Rootobject();
                model.weatherInfo = weatherInfo;

                ViewData["Login"] = "Success";
                ViewData["UserName"] = message;
                return View("~/Views/Home/Weather.cshtml", model);
            }
            return View("Index", userInfo);
        }

        [HttpPost]
        public ActionResult PostUserInfo(SignUpModel userInfo)
        {
            //var userExist = service.GetUserId(userInfo.UserName);
            //if(Convert.ToInt16(userExist) > 0)

            message = service.InsertUserInfo(userInfo);
            if (message == "Success" && ModelState.IsValid)
            {
                ViewData["Message"] = message;
                return View("~/Views/Home/Weather.cshtml", userInfo);
            }
            return View("SignUp", userInfo);
        }

        [HttpPost]
        public void DeleteWeatherInfo(string id)
        {
            string[] key = service.GetKey(id);
            service.DeleteWeatherInfo(key);
        }

        private void InsertWeatherInfo(Rootobject model)
        {
            service.InsertWeatherInfo(model);
        }

        private Rootobject GetAPIInfo(Rootobject weatherAtt)
        {
            service = new WeatherServices(_url, weatherAtt, _apiKey);
            var requestStr = service.BuildApiRequest();
            var json = service.GetWeatherApi(requestStr);
            return json;
        }

        public int GetUserId(string userName)
        {
            int u_id = 0;
            return u_id = Convert.ToInt16(service.GetUserId(userName));
        }

    }
}
