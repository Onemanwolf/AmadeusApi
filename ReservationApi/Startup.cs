using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationApi.Data.EFCore.DependencyInjection;
using ReservationApi.Data.MongoDb.DependencyInjection;
using ReservationApi.DependencyInjection;
using ReservationApi.Services;

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
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                //XML Formatter
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.UseMemberCasing());

            services.AddScoped<IReservationService, ReservationService>();

            //services.AddSqlDatabaseSupport(Configuration);
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

            app.UseMvc();
        }
    }
}
