using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroService.AuthenticationCenter
{
    /// <summary>
    /// 自定义的管理信息
    /// </summary>
    public class InitConfig
    {
        /// <summary>
        /// 定义ApiResourse
        /// 这里的资源（Resources）指的就是管理的API
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>() {
                new ApiResource("api", "My api") { Scopes={"api1","api2"} //将具体的scopes 归为一组 统称为api
            } };
        }
        //创建具体的scope
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope> {
                new ApiScope("api1","My first api"),
                new ApiScope("api2","My second api")};
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client{
                ClientId = "client",//客户端唯一标识
                ClientSecrets = { new Secret("secret".Sha256()) },//客户端密码，进行了加密
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //授权方式，客户端认证，只要ClientId+ClientSecrets
                AllowedScopes = { "api1" },//允许访问的资源
                //AllowOfflineAccess = true
                //Claims=new List<Claim>(){ 
                //  new Claim(IdentityModel.JwtClaimTypes.,"Admin")  
                //},
                }
            };
        }
    }
}
