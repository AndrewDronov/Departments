using Departments.HostedServices;
using Departments.Models;
using Departments.Services.Implementations;
using Departments.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System;

namespace Departments
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
            services.AddDbContext<DepartmentsContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("Departments")));

            services.AddHostedService<StatusChangerHostedService>();

            services.AddHttpClient("RandomStatus", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["RandomStatusUrl"]);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Configuration["RandomStatusToken"]);
                httpClient.Timeout = TimeSpan.FromSeconds(3);
            });
            
            services.AddScoped<IRepository<Department>, DbDepartmentRepository>();
            services.AddScoped<IRepository<Department>, FileDepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IRandomStatusService, RandomStatusService>();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}