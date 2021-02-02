using RoleBasedAccessControl.Queries.OAuth.Implementation;
using Lib.Databse.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoleBasedAccessControl.Interface;
using RoleBasedAccessControl.POCO;
using RoleBasedAccessControl.Queries.OAuth.Interfaces;
using RoleBasedAccessControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAccessControl.Queries.User.Interfaces;
using RoleBasedAccessControl.Queries.User.Implementation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RoleBasedAccessControl.CustomHandler;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RoleBasedAccessControl
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
            services.AddAuthentication("CookieAuthentication")
                 .AddCookie("CookieAuthentication", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie"; // Name of cookie     
                     config.LoginPath = "/Login/UserLogin"; // Path for the redirect to user login page    
                     config.AccessDeniedPath = "/Login/UserAccessDenied";
                 });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            var APIsettingsSection = Configuration.GetSection("ConnectionString");
            APIsettingsSection.Get<POCO.AppSettings>();
            services.Configure<POCO.AppSettings>(APIsettingsSection);

            services.AddDbContext<rbrbacdbContext>(options => {
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionString"));
            }, ServiceLifetime.Transient);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            appSettingsSection.Get<POCO.AppSettings>();
            services.Configure<POCO.AppSettings>(appSettingsSection);



            services.AddCors(o => o.AddPolicy("LoginPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = POCO.AppSettings.Audience,
                    ValidIssuer = POCO.AppSettings.Issuer,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(POCO.AppSettings.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            services.AddControllersWithViews();
            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>(e => Configuration.GetSection("AppSettings").Get<ApplicationConfiguration>());

            services.AddDbContext<rbrbacdbContext>();
            rbrbacdbContext rbDBContext = new rbrbacdbContext(POCO.AppSettings.GetMasterConnectionString());
            IOAuthDataAccess oauthDataAccess = new OAuthDataAccess(rbDBContext);

            services.AddScoped<IOAuth, ROAuth>(sp =>
            {
                return new ROAuth(oauthDataAccess);
            });

            IUserDataAccess userDataAccess = new UserDataAccess(rbDBContext);
            services.AddScoped<IUsers, RUsers>(sp =>
            {
                return new RUsers(userDataAccess);
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("LoginPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=OAuth}/{action=Index}");
            });
        }
    }
}
