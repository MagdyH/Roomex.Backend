using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Roomex.Backend.Data.Infrastructure;
using Roomex.Backend.Services.Implementations;
using Roomex.Backend.Services.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace Roomex.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment envService)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(envService.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{envService.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            CurrentEnvironment = envService;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            Configuration.GetSection("AppSettings").Bind(appSettings);

            AddInjection(services);

            services.AddControllers();

            AddSwagger(services, appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // add swagger for all env but Production
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(setupAction =>
                {
                    setupAction.SwaggerEndpoint("/swagger/DistanceCalculationAPISpecification/swagger.json",
                        "Distance Calculation Api");
                    setupAction.RoutePrefix = string.Empty;
                });
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// to inject services, repositories if exists
        /// </summary>
        /// <param name="services"></param>
        private void AddInjection(IServiceCollection services)
        {
            InjectServices(services);
        }
        /// <summary>
        /// to inject services
        /// </summary>
        /// <param name="services"></param>
        private void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IDistanceCalculationService, DistanceCalculationService>();
        }

        /// <summary>
        /// add swagger to represent endpoints and its input and output
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings"></param>
        private void AddSwagger(IServiceCollection services, AppSettings appSettings)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(appSettings.Swagger.Name,
                    new OpenApiInfo()
                    {
                        Title = appSettings.Swagger.Title,
                        Description = appSettings.Swagger.Description
                    });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFilePath);
            });
        }
    }
}
