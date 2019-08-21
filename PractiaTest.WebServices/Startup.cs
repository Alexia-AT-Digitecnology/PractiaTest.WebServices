using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PractiaTest.Database.Entities;
using PractiaTest.WebServices.Helpers;
using PractiaTest.WebServices.Services.Implementations;
using PractiaTest.WebServices.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace PractiaTest.WebServices
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="configuration">The configuration to use</param>
        /// /// <param name="loggerFactory">The logger factory to use</param>
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }
        
        /// <summary>
        /// The configuration used
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// The logger factory used
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }
        

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">A service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Practia Test API",
                    Description = "Practia Test Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Alexia Rodriguez",
                        Email = "digitecnology.development@gmail.com",
                        Url = "http://alexiaslab.wordpress.com/"
                    }
                });

                c.IncludeXmlComments("Assets/PractiaTest.WebServices.xml");

                c.CustomSchemaIds(x => x.FullName);
                
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseLazyLoadingProxies().UseSqlServer(String.Format(
                    "Server={0};Database={1};User Id={2};Password={3};",
                    Configuration[PreferencesManagerOptionsHelper.DatabaseHost], 
                    Configuration[PreferencesManagerOptionsHelper.DatabaseName], 
                    Configuration[PreferencesManagerOptionsHelper.DatabaseUser], 
                    Configuration[PreferencesManagerOptionsHelper.DatabasePassword])));
            
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IDatabaseService>(new DatabaseService(LoggerFactory));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder to use</param>
        /// <param name="env">Environment to use</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practia Test API");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseMvc();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}