using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Infrastructure.Conventions;
using WebApplication1.Infrastructure.Middleware;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPersonsData, InMemoryEmployesData>(); // хранение на время работы приложения
            //services.AddScoped<IPersonsData, InMemoryEmployesData>(); // хранение только на время запроса
            //services.AddTransient<IPersonsData, InMemoryEmployesData>(); // нет ничего для хранения

            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware(typeof(TestMiddleware));

            //отдельный специальный маршрут
            app.Map("/Hello", context => context.Run(async req => await req.Response.WriteAsync("Hello!")));

            app.Use(async (app, next) =>
            {
                var proc = next();
                await proc;
            });

            app.UseWelcomePage("/WelcomePage");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/hello", async context =>
                {
                    await context.Response.WriteAsync($"Hello World, {Configuration["Hello"]}!");
                });

                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
