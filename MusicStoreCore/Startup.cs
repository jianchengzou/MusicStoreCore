using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using System.IO;
using MusicStoreCore.Services;
using MusicStoreCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace MusicStoreCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            #region Requiring HTTPS globally
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});
            #endregion

            #region enable session
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set 25 mins timeout so shopping cart content can persist for reasonably long time
                options.IdleTimeout = TimeSpan.FromSeconds(1500);
                options.CookieHttpOnly = true;
            });
            #endregion

            services.AddSingleton(Configuration);
            services.AddScoped<IAlbumData, SqlAlbumData>();
            services.AddScoped<IGenreData, SqlGenreData>();
            services.AddScoped<IArtistData, SqlArtistData>();
            services.AddDbContext<MusicStoreDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MusicStoreCore")));
            //Identity
            services.AddIdentity<User, IdentityRole>(options => {
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/Login";
                })
                .AddEntityFrameworkStores<MusicStoreDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //seed data
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                if (!serviceScope.ServiceProvider.GetService<MusicStoreDbContext>().AllMigrationsApplied())
                {
                    serviceScope.ServiceProvider.GetService<MusicStoreDbContext>().Database.Migrate();
                    serviceScope.ServiceProvider.GetService<MusicStoreDbContext>().EnsureSeeded();
                }
            }

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions()
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Oopps!")
                });
            }

            #region redirect all http to https
            //var options = new RewriteOptions().AddRedirectToHttps();
            //app.UseRewriter(options);

            #endregion


            app.UseFileServer();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "bower_components")),
                RequestPath = "/bower_components"                
            });

            app.UseIdentity();
            app.UseSession();
            app.UseMvc(ConfigureRoutes);

            //app.Run(ctx => ctx.Response.WriteAsync("Not found")); this will force not loaded css or js to return 404 error
            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            // /Home/Index
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
