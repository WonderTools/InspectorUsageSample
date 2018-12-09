using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WonderTools.Inspector;

namespace InspectorUsageSample
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
            services.AddInspector();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfiguringInspectorExample1(app);
            //ConfiguringInspectorExample2(app);
            //ConfiguringInspectorExample3(app);
            //ConfiguringInspectorExample4(app);
            //ConfiguringInspectorExample5(app);
            //ConfiguringInspectorExample6(app);
            //ConfiguringInspectorExample7(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfiguringInspectorExample1(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.AddName("Inspector Usage Sample");
                x.AddVersion("1.0.0");
                x.AddKeyValue("Some Key", "Some Value");
            });

            /*
             * Version endpoint : https://localhost:44397/version
             *
             * Version data
             *{
             *  "Environment": "development",
             *  "Name": "Inspector Usage Sample",
             *  "Version": "1.0.0",
             *  "Some Key": "Some Value"
             *}
             *
             */
        }

        private void ConfiguringInspectorExample2(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.AddName("Inspector Usage Sample");
                x.AddVersion("1.0.0");
                x.AddKeyValue("Some Key", "Some Value");
                x.AddConfigurationSection(Configuration, "ConfigurationData1");
            });

            /*
             * Version endpoint : https://localhost:44397/version
             *
             * Version data
                {
                  "Environment": "development",
                  "Name": "Inspector Usage Sample",
                  "Version": "1.0.0",
                  "Some Key": "Some Value",
                  "Property1": "Value1"
                }
             *
             */
        }

        private void ConfiguringInspectorExample3(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.AddName("Inspector Usage Sample");
                x.AddVersion("1.0.0");
                x.AddKeyValue("Some Key", "Some Value");
                x.AddConfigurationSection(Configuration, "Node1:Node2:1");
            });

            /*
             * Version endpoint : https://localhost:44397/version
             *
             * Version data
                {
                  "Environment": "development",
                  "Name": "Inspector Usage Sample",
                  "Version": "1.0.0",
                  "Some Key": "Some Value",
                  "Prop1": "Value1",
                  "Prop2": "Value2"
                }
             *
             */
        }

        private void ConfiguringInspectorExample4(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.SetBaseEndpoint("/inspector/ins");
            });

            /*
             * Version endpoint : https://localhost:44397/inspector/ins/version
             *
             * Version data
                {
                  "Environment": "development",
                }
             *
             */
        }

        private void ConfiguringInspectorExample5(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.AuthenticateWith("one" , "two");
            });

            /*
             * Version endpoint (guarded) : https://localhost:44397/version
             * Expects in header (key:value)->(wondertools-authorization:one) or (wondertools-authorization:two)
             * Version Login endpoint exposed at : https://localhost:44397/version-login
             * This provides UI for typing in your password
             *
             * Version data
                {
                  "Environment": "development",
                }
             *
             */
        }

        private void ConfiguringInspectorExample6(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.SetBaseEndpoint("/inspector");
                x.AuthenticateWith((password) =>
                {
                    if ("Monday" == password) return true;
                    if ("Tuesday" == password) return true;
                    return false;
                });
            });

            /*
             * Version endpoint (guarded) : https://localhost:44397/inspector/version
             * Expects in header (key:value)->(wondertools-authorization:Monday) or (wondertools-authorization:Tuesday)
             * Version Login endpoint exposed at : https://localhost:44397/inspector/version-login
             * This provides UI for typing in your password
             *
             * Version data
                {
                  "Environment": "development",
                }
             *
             */
        }

        private void ConfiguringInspectorExample7(IApplicationBuilder app)
        {
            app.UseInspector(x =>
            {
                x.AddEnvironment("development");
                x.SetBaseEndpoint("/inspector");
                x.AuthenticateWith((password) =>
                {
                    if ("Monday" == password) return true;
                    if ("Tuesday" == password) return true;
                    return false;
                });
                x.EnableCors();
            });

            /*
             * Version endpoint (guarded) : https://localhost:44397/inspector/version
             * Expects in header (key:value)->(wondertools-authorization:Monday) or (wondertools-authorization:Tuesday)
             * ***Cross Origin request to the version endpoint are not allowed 
             * Version Login endpoint exposed at : https://localhost:44397/inspector/version-login
             * This provides UI for typing in your password
             *
             * Version data
                {
                  "Environment": "development",
                }
             *
             */
        }
    }
}
