using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatWithSignalR.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace ChatWithSignalR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddDbContext<AuthDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IMongoClient>
                (mc => new MongoClient(Configuration.GetConnectionString("MongoDbConnection")));
            // We injected IMongoClient interface as new MongoClient object as singelton object.

            services.AddSingleton<MongoDbContext>
                  (mdc => new MongoDbContext(mdc.GetRequiredService<IMongoClient>(), Configuration["MongoDbName"]));
            // We injected MongoDbContext as singelton MongoDbContext in application.

            services.AddTransient<ChatHistoryRepository>();
            /* We injected ChatHistoryRepository as transient, 
                it means this object will be generated whenever we use in ctor */
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseSignalR(route =>
            {
                route.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
