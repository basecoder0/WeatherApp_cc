﻿using Microsoft.AspNetCore.Mvc;
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
            return Json(apiString);
        }

        [HttpGet]
        public ActionResult GetUserCredentials(IndexModel userInfo)
        {
            if (ModelState.IsValid)
            {
                var userExist = service.UserExist(userInfo.UserName);
                if (!userExist)
                {
                    ModelState.AddModelError("UserName", "Please Enter a Valid User Name");
                    return View("Index", userInfo);
                }
                message = service.GetUserCredentials(userInfo);
                string userId = service.GetUserId(userInfo.UserName);
                var weatherInfo = service.GetWeatherInfo(Convert.ToInt16(userId));
                Rootobject model = new Rootobject();
                model.weatherInfo = weatherInfo;
                //uncomment this when ready to test update funcationality or it is ready to turn in
                //model.weatherInfo = UpdateWeatherInfo(weatherInfo);

                ViewData["Login"] = "Success";
                ViewData["UserName"] = message;
                return View("Weather", model);
            }
            return Redirect("Index");
        }

        [HttpGet]
        public JsonResult GetUserSignUpLoc(IndexModel userInfo)
        {
            Rootobject model = new Rootobject();
            string temp = "";

            string userId = service.GetUserId(userInfo.UserName);
            var weatherInfo= service.GetUserSignUpLoc(Convert.ToInt16(userId));
            model.weatherInfo = weatherInfo;
            var apiString = GetAPIInfo(model);
            temp = service.KelvinToFahrenheit(apiString.main.temp);
            apiString.main.temp = Convert.ToDouble(temp);
            apiString.user_id = Convert.ToInt16(userId);

            InsertWeatherInfo(apiString);
            return Json(apiString);            
        }

        [HttpPost]
        public ActionResult PostUserInfo(SignUpModel userInfo)
        {
            var userExist = service.UserExist(userInfo.UserName);
            if (userExist)
            {
                ModelState.AddModelError("UserName", "User Name already exists");
                return View("SignUp", userInfo);
            }
            else
            {
                message = service.InsertUserInfo(userInfo);
                //if ( ModelState.IsValid)
                if (message == "Success" && ModelState.IsValid)
                {
                    ViewData["Login"] = "Success";
                    ViewData["Message"] = message;
                    ViewData["UserName"] = userInfo.UserName;

                    Rootobject userObj = new Rootobject();
                    userObj.signUpModel = userInfo;

                    return View("Weather", userObj);
                }
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
            WeatherInfoModel weather = new WeatherInfoModel();
            if (weatherAtt.weather != null)
            {
                service = new WeatherServices(_url, weatherAtt, _apiKey);
            }
            else
            {
               
                foreach(var item in weatherAtt.weatherInfo)
                {
                    weather.City = item.City;
                    weather.State = item.State;
                }               
                service = new WeatherServices(_url, weather,_apiKey);
            }           
            var requestStr = service.BuildApiRequest();
            var json = service.GetWeatherApi(requestStr,weather.City, weather.State);
            return json;
        }

        private List<WeatherInfoModel> UpdateWeatherInfo(List<WeatherInfoModel> model)
        {
            List<WeatherInfoModel> updatedWeather = new List<WeatherInfoModel>();
            foreach (var item in model)
            {
                service = new WeatherServices(_url, item, _apiKey);
                var requestStr = service.BuildApiRequest();
                var json = service.GetWeatherApi(requestStr);
                json.weather[0].City = item.City;
                json.weather[0].State = item.State;
                json.weatherInfo_Obj = service.createNewJSONObj(json);
                json.weatherInfo_Obj.id = item.id;
                service.UpdateWeatherInfo(json.weatherInfo_Obj);

                updatedWeather.Add(json.weatherInfo_Obj);
            }
            return updatedWeather;
        }

        private int GetUserId(string userName)
        {
            int u_id = 0;
            return u_id = Convert.ToInt16(service.GetUserId(userName));
        }

    }
}
