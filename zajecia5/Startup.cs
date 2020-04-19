using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using zajecia5.MIddlewear;
using zajecia5.Services;

namespace zajecia5
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
            services.AddTransient<IStudentDbService, SqlServerStudentDbService>();
            services.AddControllers()
                    .AddXmlSerializerFormatters();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Students App API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService studentDb)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<LoggingMiddleware>();
            app.UseWhen(condition => condition.Request.ToString().Contains("info"), app =>
            {
                app.Use(async (contex, next) =>
                {
                    if (!contex.Request.Headers.ContainsKey("Index"))
                    {
                        contex.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await contex.Response.WriteAsync("Podaj index!");
                        return;
                    }
                    var stIndex = contex.Request.Headers["Index"].ToString();
                    if (!studentDb.IsThereStudent(stIndex))
                    {
                        contex.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await contex.Response.WriteAsync("Taki student nie istnieje!");
                        return;
                    }
                    await next();
                });
            });
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Students App API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
