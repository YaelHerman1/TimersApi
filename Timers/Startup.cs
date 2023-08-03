using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Timers.Models;
using System.Configuration;
using Timers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Timers.Controllers;
using Timers.Persistence;

namespace Timers
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(TimerController).Assembly);
            services.AddControllers();
            services.AddDbContext<TimerDbContext>(options => options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<TimerRepository>();
            services.AddHttpClient();
            services.AddScoped<TimerBackgroundService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timers Manager", Version = "v1" });


            });
        }
        public void Configure(IApplicationBuilder app)
        {

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }
            );
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timers Manager");
            });

        }
    }
}
