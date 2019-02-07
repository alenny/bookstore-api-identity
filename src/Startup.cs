using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BookStore.Api.Identity.Services;
using Swashbuckle.AspNetCore.Swagger;
using BookStore.Api.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Identity
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddScoped<IUserService, UserService>();

            // Register AppSettings as a service
            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("AppDbContext")));

            // Add Swagger service
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Info { Title = "BookStore.Api.Identity", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(policyBuilder => policyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Insert Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore.Api.Identity v1"));

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
