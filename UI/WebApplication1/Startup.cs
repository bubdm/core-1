using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.Domain.Identity;
using WebApplication1.Hubs;
using WebApplication1.Infrastructure.Conventions;
using WebApplication1.Infrastructure.Middleware;
using WebApplication1.Interfaces.Services;
using WebApplication1.Interfaces.WebAPI;
using WebApplication1.Logger;
using WebApplication1.Services.Services;
using WebApplication1.WebAPI.Clients.Identity;
using WebApplication1.WebAPI.Clients.Orders;
using WebApplication1.WebAPI.Clients.Persons;
using WebApplication1.WebAPI.Clients.Products;
using WebApplication1.WebAPI.Clients.Values;

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
            services.AddIdentity<User, Role>()
                .AddIdentityApplication1WebAPIClients()
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
            
            //services.AddScoped<ICartService, InCookiesCartService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartStore, InCookiesCartStore>();

            services.AddHttpClient("Application1API", c => c.BaseAddress = new Uri(Configuration["WebAPI"]))
                .AddTypedClient<IValuesService, ValuesClient>()
                .AddTypedClient<IPersonsData, PersonsClient>()
                .AddTypedClient<IProductData, ProductsClient>()
                .AddTypedClient<IOrderService, OrdersClient>();

            services.AddControllersWithViews(opt => opt.Conventions.Add(new TestControllerConvention()))
                .AddRazorRuntimeCompilation();

            services.AddSignalR();

            services.AddRazorPages();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log/*, IServiceProvider service*/)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }

            app.UseBlazorFrameworkFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
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
                endpoints.MapHub<ChatHub>("/chat");

                endpoints.MapGet("/hello", async context =>
                {
                    await context.Response.WriteAsync($"Hello World, {Configuration["Hello"]}!");
                });

                endpoints.MapControllerRoute(
                    name : "areas",
                    pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();

                endpoints.MapFallbackToFile("blazor.html");
                //endpoints.MapFallbackToFile("/blazor/{*path:nonfile}", "blazor.html");
            });


        }
    }
}
