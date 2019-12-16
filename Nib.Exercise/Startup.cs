using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nib.Exercise.Configurations;
using Nib.Exercise.Helpers;
using Nib.Exercise.Extensions;
using Nib.Exercise.Interfaces;
using Nib.Exercise.Models;

namespace Nib.Exercise
{
    public class Startup
    {
        #region PROPERTIES

        private readonly ConfigurationExercise _configurationExercise;

        #endregion

        public Startup()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            (ConfigurationExercise config, IConfigurationRoot rootConfig) = ConfigurationLoader.LoadConfigurations<ConfigurationExercise>(env);
            _configurationExercise = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configurationExercise);
            services.AddSession();
            services.AddRazorPages(); 
            services.AddAntiforgery(o => o.HeaderName = "CSRF-TOKEN");
            services.AddControllersWithViews();

            services.AddSingleton<IPropertiesFormatter<Vacancy>, VacancyPropertiesFormatter>();
            services.RegisterLocationsClient(_configurationExercise.LocationsApiUrl);
            services.RegisterVacanciesSource();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Careers}/{action=Careers}");
            });
        }
    }
}
