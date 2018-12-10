using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using POC01.Model.DataContext;
using POC01.Model;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace POC01
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add response compression
            services.AddResponseCompression();

            //Add custom services
            services.AddOptions();

            //Add Azure SQL connection and enable retry if connection fails
            services.AddDbContext<AzureDataContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("LocalDB"),
                sqlServerOptionsAction: SqlServerDbContextOptionsExtensions =>
                {
                    SqlServerDbContextOptionsExtensions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });

            //Repositories
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IDentistRepository, DentistRepository>();


            //Enable CORS
            services.AddCors();

            //Force HTTPS
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});

            //Add framework services.
            services.AddMvc();

            //AD support
            services.AddAuthentication(SharedOptions => SharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //use response compression
            app.UseResponseCompression();

            //AD
            app.UseCookieAuthentication();

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder => builder.WithOrigins("http://localhost"));

            //AD support
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                ClientId = Configuration["Authentication:AzureAd:ClientId"],
                Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"],
                CallbackPath = Configuration["Authentication:AzureAd:CallbackPath"]
            });

            /* removed to support AD
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"],
                Audience = Configuration["Authentication:AzureAd:Audience"]
            });
            */

            //Force HTTPS
            //var options = new RewriteOptions().AddRedirectToHttps();
            //app.UseRewriter(options);

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}