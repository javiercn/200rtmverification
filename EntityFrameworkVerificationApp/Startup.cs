using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Authentication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkVerificationApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Catalog")));

            services.AddDbContext<ReviewsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Reviews")));

            services.AddDbContext<ShowsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Shows")));

            services.AddDbContext<BillingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Billing")));

            if (Environment.IsDevelopment())
            {
                services.AddTransient<IStartupFilter, DatabaseInitializer>();
            }

            services.AddAzureAdB2CAuthentication();

            services.AddMvc()
                .AddRazorPagesOptions(rpo =>
                    rpo.Conventions
                       .AuthorizeFolder("/Billing"))
                .AddCookieTempDataProvider()
                .AddRazorOptions(r => r.PageViewLocationFormats.Add("/Pages/Shared/{0}.cshtml"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}