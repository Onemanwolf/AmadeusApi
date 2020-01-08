using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationApi.DependencyInjection;
using ReservationApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft;
using Newtonsoft.Json;

namespace ReservationApi
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
            //Session 1
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                //XML Formatter
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());

            })

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.UseMemberCasing();
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    
                });
                


                



            ///Swagger
            services.AddSwaggerConfiguration(Configuration);


            //Session 3 Add Auth
            services.AddAuthConfiguration(Configuration);

            //Session 1
            //The configuration instance to which the appsettings.json file's BookstoreDatabaseSettings section binds is 
            //registered in the Dependency Injection (DI) container. For example, a BookstoreDatabaseSettings object's 
            //ConnectionString property is populated with the BookstoreDatabaseSettings:ConnectionString property in 
            //appsettings.json.
            //The IBookstoreDatabaseSettings interface is registered in DI with a singleton service lifetime.When 
            //injected, the interface instance resolves to a BookstoreDatabaseSettings object.

            //configure mongodb connection and register an instance of IMongoCollection for use in service

            services.AddScoped<ReservationService>();

            //services.AddEFCoreDatabaseSupport(Configuration);
            services.AddMongoDatabaseSupport(Configuration);


            services.AddCorsConfiguration(Configuration);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Session 3
            app.UseAuthentication();
            app.UseHttpsRedirection();

            //Session 2
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reservation Api");

                //c.DocExpansion(DocExpansion.None);

                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
