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
using Consul;

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
            string url = null;
            #region Nginx
            url = "http://localhost:8055/api/users/all";
            #endregion

            #region consul
            url = "http://MicroService/api/users/all";//客户端调用服务--Consul就像个DNS

            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500/");
                c.Datacenter = "dcl";
            });
            var response = client.Agent.Services().Result.Response;
            //foreach (var item in response)
            //{
            //    Console.WriteLine("********************");
            //    Console.WriteLine(item.Key);
            //    var service = item.Value;
            //    Console.WriteLine($"{service.Address}--{service.Port}--{service.Service}");
            //    Console.WriteLine("********************");
            //}


            Uri uri = new Uri(url);
            string groupName = uri.Host;
            AgentService agentService = null;
            var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();//找到的全部服务

            agentService = serviceDictionary[0].Value;
            //负载均衡策略
            {
                //轮询策略
                //agentService = serviceDictionary[iIndex++].Value;
                //if (iIndex >= serviceDictionary.Length)
                //    iIndex = 0;
            }
            {
                //平均策略--随机获取索引--相对就平均
                //if (iIndex >= int.MaxValue - 1)
                //    iIndex = 0;
                //agentService = serviceDictionary[new Random(iIndex++).Next(0, serviceDictionary.Length)].Value;
            }

            {
                //权重策略--给不同的实例分配不同的压力--注册时提供权重
                List<KeyValuePair<string, AgentService>> pairsList = new List<KeyValuePair<string, AgentService>>();
                foreach (var pair in serviceDictionary)
                {
                    int count = int.Parse(pair.Value.Tags?[0]);
                    for (int i = 0; i < count; i++)
                    {
                        pairsList.Add(pair);
                    }
                }
                agentService = pairsList.ToArray()[new Random(iIndex++).Next(0, pairsList.Count())].Value;

            }


            url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";


            #endregion

            string content = InvokeApi(url);

            base.ViewBag.Users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            Console.WriteLine($"This is {url} Invoke");
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

        private static int iIndex = 0;
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
