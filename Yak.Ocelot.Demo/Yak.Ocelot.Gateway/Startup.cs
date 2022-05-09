using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

namespace Yak.Ocelot.Gateway
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
            //IdentityService4
            //Action<IdentityServerAuthenticationOptions> isaOptClient = option =>
            //{
            //    option.Authority = Configuration["IdentityService4.Uri"];
            //    option.ApiName = "UserService";
            //    option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService4.UseHttps"]);
            //    option.SupportedTokens = SupportedTokens.Both;
            //    //option.ApiSecret = Configuration["IdentityService4:ApiSecrets:DesignerService"];
            //};

            //services.AddAuthentication()
            //    .AddIdentityServerAuthentication("userserverkey", isaOptClient);

            #region Identity4
            //var authenticationProviderKey = "Gatewaykey";
            //services.AddAuthentication("Bearer")
            //       .AddIdentityServerAuthentication(authenticationProviderKey, m =>
            //       {
            //           m.Authority = "http://localhost:8000";//Ids�ĵ�ַ����ȡ��Կ
            //           m.ApiName = "Yak.Ocelot.Api";
            //           m.RequireHttpsMetadata = false;
            //           m.SupportedTokens = SupportedTokens.Both;
            //           m.ValidateIssuer = false;
            //       });
            #endregion

            var identityBuilder = services.AddAuthentication();
            IdentityServerConfig identityServerConfig = new IdentityServerConfig();
            //Configuration.Bind("IdentityServerConfig", identityServerConfig);
            //identityBuilder.AddIdentityServerAuthentication("Gatewaykey", options =>
            //{
            //    options.Authority = "http://localhost:8000";//Ids�ĵ�ַ����ȡ��Կ
            //    options.RequireHttpsMetadata = false;
            //    options.ApiName = "Yak.Ocelot.Api";
            //    options.SupportedTokens = SupportedTokens.Both;
            //});


            // ע����֤������������defaultSchemeΪBearer
            //services.AddAuthentication("Bearer")
            //  .AddJwtBearer("Bearer", options =>
            //  {
            //      options.Authority = "http://localhost:8000";
            //      // ����֤tokenʱ������֤Audience
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          ValidateAudience = false,
            //          ValidateIssuer = false
            //      };
            //      options.RequireHttpsMetadata = false; // ������Https
            //  });

            IdentityModelEventSource.ShowPII = true;
            var authenticationProviderKey = "Gatewaykey";
            services.AddAuthentication("Bearer").AddJwtBearer(authenticationProviderKey, x =>
            {
                x.Authority = "http://localhost:8000";
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });


        //    services
        //.AddAuthentication("Bearer")
        //.AddJwtBearer("Bearer", config =>
        //{
        //    config.Authority = "http://localhost:8000";
        //    //ȷ��ʹ����Щ��Դ
        //    config.Audience = "ApiOne";
        //    config.RequireHttpsMetadata = false;
        //    //ȡ����֤�û��Լ���֤��ɫ
        //    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        //    {
        //        ValidateIssuer = false,
        //        ValidateAudience = false
        //    };
        //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("api1Scope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", "Yak.Ocelot.Api");
            //    });
            //});


            services.AddControllers();

            //services.AddOcelot().AddConsul()
            //    .AddCacheManager(m =>
            //    {
            //        m.WithDictionaryHandle();//Ĭ���ֵ�洢
            //    })
            //    .AddPolly();
            services.AddOcelot(Configuration).AddConsul().AddPolly();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            

            app.UseRouting();
            app.UseAuthentication();//�ȼ�Ȩ,û�м�Ȩ,��Ȩ��û�������

            app.UseAuthorization();//����Ȩ
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("����Ocelot����!");
                });
            });
            
            //app.UseOcelot().Wait();
            app.UseOcelot();
        }
    }

    public class IdentityServerConfig
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string IdentityScheme { get; set; }
        public List<APIResource> Resources { get; set; }
    }

    public class APIResource
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
