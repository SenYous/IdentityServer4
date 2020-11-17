using MicroService.ClientWebDemo.Models;
using MicroService.Interface;
using MicroService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace MicroService.ClientWebDemo.Controllers
{
    // dotnet MicroService.ClientWebDemo.dll --urls="http://*:6467" --ip="127.0.0.1" --port=6467
    public class HomeController : Controller
    {
        #region private
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _iUserService = null;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this._iUserService = userService;
        }

        #endregion

        public IActionResult Index()
        {
            //base.ViewBag.Users = this._iUserService.UserAll();
            string url = "http://localhost:5726/api/users/all";
            string content = InvokeApi(url);

            base.ViewBag.Users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
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


        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

    }
}
