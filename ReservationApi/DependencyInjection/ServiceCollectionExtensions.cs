﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReservationApi.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string[] corsDomains = configuration.GetSection("CorsDomains")
                .Get<string>()
                .Split(',');

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(corsDomains)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build();
                });
            });
        }

        public static void AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 // base-address of your identityserver Endpoint
                 options.Authority = "https://localhost:5001/";

                 // name of the API resource
                 options.Audience = "reservationapi";
             });
        }

        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {

                //Swagger Documentation option
                options.SwaggerDoc("v1", new Info
                {
                    Title = "MicrosoftDemo API",
                    Version = "v1",
                    Description = "Amadeus Api for Training",
                    Contact = new Contact
                    {
                        Email = "timothy.oleson@microsoft.com",
                        Name = "Tim Oleson",
                        Url = "https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2"
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://opensource.org/licenses/MIT"
                    }
                });

                //Include XML comments in you Api Documentation 
                // Open Project Properties under Build Tab in Output section check xml documentation file change value to ReservationApi
                //Use Reflection to file name 
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                //use full path
                options.IncludeXmlComments(xmlCommentsFullPath);
                options.DescribeAllEnumsAsStrings();
                //options.SchemaFilter<FluentValidationRuleSchemaFilter>();
                //  options.SchemaFilter<BrowsablePropertySchemaFilter>();

                options.AddSecurityDefinition("Bearer",
                  new ApiKeyScheme
                  {
                      In = "header",
                      Description = "Please enter into field the word 'Bearer' following by space and JWT",
                      Name = "Authorization",
                      Type = "apiKey"
                  });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() },
             });
            });
        }
    }
}
