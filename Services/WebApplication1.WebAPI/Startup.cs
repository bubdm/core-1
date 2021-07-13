using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Services;
using WebApplication1.Logger;
using WebApplication1.Services.Data;
using WebApplication1.Services.Services;

namespace WebApplication1.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseName = Configuration["Database"];
            switch (databaseName)
            {
                case "MSSQL": 
                    services.AddDbContext<Application1Context>(opt => 
                        opt.UseSqlServer(Configuration.GetConnectionString("MSSQL"),
                            o => o.MigrationsAssembly("WebApplication1.Dal")));
                    break;
                case "SQLite":
                    services.AddDbContext<Application1Context>(opt =>
                        opt.UseSqlite(Configuration.GetConnectionString("SQLite"),
                            o => o.MigrationsAssembly("WebApplication1.Dal.Sqlite")));
                    break;
            }

            services.AddTransient<WebStoreDBInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<Application1Context>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
#if DEBUG
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 3;       
#endif
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "WebStoreDB";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(10);

                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.SlidingExpiration = true;
            });

            services.AddScoped<ICartService, InCookiesCartService>();
            
            services.AddScoped<IPersonsData, SqlPersonsData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<IOrderService, SqlOrderService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1.WebAPI", Version = "v1" });

                const string app1ApiDomainXml = "WebApplication1.Domain.xml";
                const string app1WebApiXml = "WebApplication1.WebAPI.xml";
                const string debugPath = "bin/debug/net5.0";

                #if DEBUG
                if(File.Exists(app1ApiDomainXml))
                    c.IncludeXmlComments(app1ApiDomainXml);
                else if(File.Exists(Path.Combine(debugPath, app1ApiDomainXml)))
                    c.IncludeXmlComments(Path.Combine(debugPath, app1ApiDomainXml));

                if (File.Exists(app1WebApiXml))
                    c.IncludeXmlComments(app1WebApiXml);
                else if (File.Exists(Path.Combine(debugPath, app1WebApiXml)))
                    c.IncludeXmlComments(Path.Combine(debugPath, app1WebApiXml));
                #else
                if(File.Exists(app1ApiDomainXml))
                    c.IncludeXmlComments(app1ApiDomainXml);
                if (File.Exists(app1WebApiXml))
                    c.IncludeXmlComments(app1WebApiXml);
                #endif
                
                c.IncludeXmlComments("WebApplication1.WebAPI.xml");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service, ILoggerFactory log)
        {
            log.AddLog4Net();

            using (var scope = service.CreateScope())
                scope.ServiceProvider.GetRequiredService<WebStoreDBInitializer>().Init();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1.WebAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
