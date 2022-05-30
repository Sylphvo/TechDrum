using Autofac;
using Invedia.DI;
using Invedia.Web.Middlewares.HttpContextMiddleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Repository.Data;
using TechDrum.Web.Modules;
using System;
using System.Reflection;

namespace TechDrum.Web
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://45.119.82.72:8044/hub/eth")
                        .AllowCredentials();
                });
            });
            //set time token
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddApplicationInsightsTelemetry();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddInvediaHttpContext();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/signin";
                        options.LogoutPath = "/signout";
                    });
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(
                        typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
                    sqlServerOptionsAction.MigrationsHistoryTable("Migration");
                }));
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddDefaultIdentity<UserEntity>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;  
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                //options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                //Default Sign setting
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/signin";
                //options.Cookie.Name = "YourAppCookieName";
                //options.Cookie.HttpOnly = true;
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/signin";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            services.AddControllersWithViews();

            services.AddDI();
            services.PrintServiceAddedToConsole();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Oops/{statusCode}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //System Setting
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //httpcontext
            app.UseInvediaHttpContext();
            app.UseRequestLocalization();
            app.UseRouting();
          
            app.UseCookiePolicy();

            //Add User session
            app.UseSession();
            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "Home",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //builder.Register(c => new AppDbContext()).InstancePerLifetimeScope();
            builder.RegisterModule(new AutofacModule());
        }
    }
}