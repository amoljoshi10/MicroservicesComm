using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShoppingApp.Controllers;
using OnlineShoppingApp.Services;

namespace OnlineShoppingApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var builder = services.AddControllersWithViews();
            services.AddHttpClient<IProductService, ProductService>(c =>
                c.BaseAddress = new Uri(Configuration["GatewayAPIURI"]));

            services.AddHttpClient<ICartService, CartService>(c =>
             c.BaseAddress = new Uri(Configuration["GatewayAPIURI"]));

            services.AddHttpClient<IOrderService, OrderService>(c =>
             c.BaseAddress = new Uri(Configuration["GatewayAPIURI"]));


            services.AddApplicationInsightsTelemetry();
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();
            
           
            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
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
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Products}/{action=List}/{id?}");
            });
        }
    }
}