using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocelot.Cache;
using Ocelot.Provider.Polly;

namespace MicroService.GateWayDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region ids4
            var authenticationProviderKey = "UserGatewayKey";
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                {
                    options.Authority = "http://localhost:7200";
                    options.ApiName = "api";
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                });
            #endregion

            services
                .AddOcelot()
                .AddConsul()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();//Ĭ���ֵ�洢
                })
                .AddPolly();
            //.AddSingletonDefinedAggregator<UserAggregator>() //�Զ���ۺ���
            //.AddPolly();//��δ���

            services.AddSingleton<IOcelotCache<CachedResponse>, CustomCache>();
            //�����IOcelotCache<CachedResponse>��Ĭ�ϻ����Լ��--׼���滻���Զ����

            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseOcelot().Wait();//�������̵Ĺܵ�����Ocelot

            //Ocelot��չ
            var configuration = new OcelotPipelineConfiguration
            {
                PreQueryStringBuilderMiddleware = async (context, next) =>
                {
                    await Task.Run(() => Console.WriteLine($"This is custom middleware,PreQueryStringBuilderMiddleware {context.Request.Path}"));
                    await next.Invoke();
                }
            };

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
