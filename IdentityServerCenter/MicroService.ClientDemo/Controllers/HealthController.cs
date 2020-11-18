using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.ClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : Controller
    {
        #region private
        private IConfiguration _iConfiguration;
        public HealthController(IConfiguration configuration)
        {
            this._iConfiguration = configuration;
        }
        #endregion

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is HealthController {this._iConfiguration["port"]} Invoke");
            return Ok();//只是个200
        }
    }
}
