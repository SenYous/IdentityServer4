using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.ServoceInstanceAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        #region private
        private readonly ILogger<TestController> _logger;
        private IConfiguration _iConfiguration;
        public TestController(ILogger<TestController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._iConfiguration = configuration;
        }

        #endregion


        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is TestController {this._iConfiguration["port"]}|Invoke Index");
            return new JsonResult(new
            {
                message = "This is TestController Index",
                Port = this._iConfiguration["port"],
                Time = DateTime.Now.ToString("G")
            });
        }

        [Authorize]
        [HttpGet]
        [Route("Exception")]
        public IActionResult IndexA()
        {
            Console.WriteLine($"This is TestController {this._iConfiguration["port"]}|Invoke IndexA");
            return new JsonResult(new
            {
                message = "This is TestController IndexA",
                Port = this._iConfiguration["port"],
                Time = DateTime.Now.ToString("G")
            });
        }
    }
}
