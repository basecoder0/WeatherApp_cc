using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string _apiKey = "";
        private readonly ILogger<HomeController> _logger;
        private string _message = "";
        private WeatherServices _service = new WeatherServices();

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

        /**
         * Gets weather by calling GetApi to build the apistring
         *  - Gets the weather info based on query in apistring
         *  - Get users previous weather info records and updates them to the latest temp
         *  - Takes recente weather inquiry and insterts into DB
         */
        [HttpGet]
        public JsonResult GetWeather(Rootobject model)
        {
            string temp = "";
            var apiString = GetAPIInfo(model);
            if(apiString.ErrorMessage != "NotFound")
            {
                apiString.Weather[0].City = model.Weather[0].City;
                apiString.Weather[0].State = model.Weather[0].State;

                temp = _service.KelvinToFahrenheit(apiString.Main.Temp);
                apiString.Main.Temp = Convert.ToDouble(temp);
                apiString.User_id = GetUserId(model.UserName);

                var weatherInfo = _service.GetWeatherInfo(Convert.ToInt16(apiString.User_id));
                model.WeatherInfo = weatherInfo;
                if (weatherInfo != null)
                {
                    model.WeatherInfo = UpdateWeatherInfo(weatherInfo);
                }

                InsertWeatherInfo(apiString);
                apiString.WeatherInfo = weatherInfo;
                return Json(apiString);
            }
            ModelState.AddModelError("State", "Please Enter a Valid State Name");
            return Json(apiString);
        }

        /**
         * Gets user credentials upon initial sign in
         *  - upon loading page, all current weather records for logged in user are updated
         **/

        [HttpGet]
        public ActionResult GetUserCredentials(IndexModel userInfo)
        {
            if (ModelState.IsValid)
            {
                var userExist = _service.UserExist(userInfo.UserName);
                if (!userExist)
                {
                    ModelState.AddModelError("UserName", "Please Enter a Valid User Name");
                    return View("Index", userInfo);
                }
                _message = _service.GetUserCredentials(userInfo);
                string userId = _service.GetUserId(userInfo.UserName);
                var weatherInfo = _service.GetWeatherInfo(Convert.ToInt16(userId));
                Rootobject model = new Rootobject();
                model.WeatherInfo = weatherInfo;
                if (weatherInfo != null)
                {
                    model.WeatherInfo = UpdateWeatherInfo(weatherInfo);
                }
                ViewData["Login"] = "Success";
                ViewData["UserName"] = _message;
                return View("Weather", model);
            }
            return Redirect("Index");
        }

        /**
         * Gets user's loctaion based on record from DB and inserts that as a weather info record
         **/
        [HttpGet]
        public JsonResult GetUserSignUpLoc(IndexModel userInfo)
        {
            Rootobject model = new Rootobject();
            string temp = "";

            string userId = _service.GetUserId(userInfo.UserName);
            var weatherInfo = _service.GetUserSignUpLoc(Convert.ToInt16(userId));
            model.WeatherInfo = weatherInfo;
            var apiString = GetAPIInfo(model);
            temp = _service.KelvinToFahrenheit(apiString.Main.Temp);
            apiString.Main.Temp = Convert.ToDouble(temp);
            apiString.User_id = Convert.ToInt16(userId);

            InsertWeatherInfo(apiString);
            return Json(apiString);
        }

        /**
         * Posts user's sign up info to DB
         **/

        [HttpPost]
        public ActionResult PostUserInfo(SignUpModel userInfo)
        {
            var userExist = _service.UserExist(userInfo.UserName);
            if (userExist)
            {
                ModelState.AddModelError("UserName", "User Name already exists");
                return View("SignUp", userInfo);
            }
            else
            {
                _message = _service.InsertUserInfo(userInfo);

                if (_message == "Success" && ModelState.IsValid)
                {
                    ViewData["Login"] = "Success";
                    ViewData["Message"] = _message;
                    ViewData["UserName"] = userInfo.UserName;

                    Rootobject userObj = new Rootobject();
                    userObj.SignUpModel = userInfo;

                    return View("Weather", userObj);
                }
            }
            return View("SignUp", userInfo);
        }

        /**
         * Deletes weather records from DB
         * 
         **/
        [HttpPost]
        public void DeleteWeatherInfo(string id)
        {
            string[] key = _service.GetKey(id);
            _service.DeleteWeatherInfo(key);
        }

        /**
         * Inserts weather record 
         **/
        private void InsertWeatherInfo(Rootobject model)
        {
            _service.InsertWeatherInfo(model);
        }

        /**
        * Setups api string to be used to query api
        **/
        private Rootobject GetAPIInfo(Rootobject weatherAtt)
        {
            string requestStr;
            Rootobject json;
            WeatherInfoModel weather = new WeatherInfoModel();
            if (weatherAtt.Weather != null)
            {
                _service = new WeatherServices(_url, weatherAtt, _apiKey);
            }
            else
            {
                foreach (var item in weatherAtt.WeatherInfo)
                {
                    weather.City = item.City;
                    weather.State = item.State;
                }
                _service = new WeatherServices(_url, weather, _apiKey);
                requestStr = _service.BuildApiRequest();
                json = _service.GetWeatherApi(requestStr, weather.City, weather.State);
                return json;
            }
            requestStr = _service.BuildApiRequest();
            json = _service.GetWeatherApi(requestStr);
            if (json.ErrorMessage != null)
            {
                return json;
            }
            return json;
        }

        /**
        * Updates weather record 
        **/
        private List<WeatherInfoModel> UpdateWeatherInfo(List<WeatherInfoModel> model)
        {
            List<WeatherInfoModel> updatedWeather = new List<WeatherInfoModel>();
            foreach (var item in model)
            {
                _service = new WeatherServices(_url, item, _apiKey);
                var requestStr = _service.BuildApiRequest();
                var json = _service.GetWeatherApi(requestStr);
                json.Weather[0].City = item.City;
                json.Weather[0].State = item.State;
                json.WeatherInfo_Obj = _service.CreateNewJSONObj(json);
                json.WeatherInfo_Obj.Id = item.Id;
                _service.UpdateWeatherInfo(json.WeatherInfo_Obj);

                updatedWeather.Add(json.WeatherInfo_Obj);
            }
            return updatedWeather;
        }

        /**
         * Gets user id
         **/
        private int GetUserId(string userName)
        {
            int u_id = 0;
            return u_id = Convert.ToInt16(_service.GetUserId(userName));
        }

    }
}
