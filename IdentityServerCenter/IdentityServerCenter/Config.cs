using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerCenter
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };
        }

        public static IEnumerable<ApiResource> GetResources()
        {
            //    return new List<ApiResource> {
            //    new ApiResource {
            //        Name = "customAPI",
            //        DisplayName = "Custom API",
            //        Description = "Custom API Access",
            //        UserClaims = new List<string> {"role"},
            //        ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
            //        //Scopes = new List<Scope> {
            //        //    new Scope("customAPI.read"),
            //        //    new Scope("customAPI.write")
            //        //}
            //            Scopes = new List<string> {
            //            "customAPI.read",
            //            "customAPI.write"
            //        }
            //    }
            //};

            return new List<ApiResource> { new ApiResource("api", "My Api") };
        }

        public static IEnumerable<Client> GetClients()
        {
            //return new List<Client> {
            //new  Client (){
            //    ClientId ="oauthClient",
            //    ClientName = "Example Client Credentials Client Application",
            //    AllowedGrantTypes =GrantTypes.ClientCredentials,
            //    ClientSecrets = new List<Secret> {
            //        new  Secret("superSecretPassword".Sha256())
            //    },
            //    AllowedScopes =new List<string> {"customAPI.read"}
            //    }
            //};

            return new List<Client> {
            new  Client (){
                ClientId ="oauthClient",
                ClientName = "Example Client Credentials Client Application",
                AllowedGrantTypes =GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> {
                    new  Secret("superSecretPassword".Sha256())
                },
                AllowedScopes = {"api"}
                }
            };
        }

        public static List<TestUser> Get()
        {
            return new List<TestUser> {
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "scott",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            }
        };
        }
    }
}
