using MicroService.Interface;
using MicroService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroService.ClientDemo.Controllers
{
    //dotnet MicroService.ClientDemo.dll --urls="http://*:5726" --ip="127.0.0.1" --port=5726
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        #region private
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _iUserService = null;
        private IConfiguration _iConfiguration;
        public UsersController(ILogger<UsersController> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            this._iUserService = userService;
            this._iConfiguration = configuration;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]}|Invoke GetId");
            var model = this._iUserService.FindUser(id);
            model.Role = $"{this._iConfiguration["ip"]}{this._iConfiguration["port"]}";
            return model;
        }


        [HttpGet]
        [Route("All")]
        public IEnumerable<User> Get()
        {
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]}|Invoke Get");
            return this._iUserService.UserAll().Select(u => new Model.User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{this._iConfiguration["ip"]}{this._iConfiguration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

        [HttpGet]
        [Route("Timeout")]
        public IEnumerable<User> Timeout()
        {
            Thread.Sleep(5000);
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]}|Invoke Timeout");
            return this._iUserService.UserAll().Select(u => new Model.User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{this._iConfiguration["ip"]}{this._iConfiguration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

        [HttpGet]
        [Route("Exception")]
        public void Exception()
        {
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]}|Invoke Exception");
        }
    }
}
