using Microsoft.EntityFrameworkCore;
using Timers.Models;
using Timers.Persistence;

namespace Timers.Services
{
    public class TimerBackgroundService : BackgroundService
    {
        private readonly TimerDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public TimerBackgroundService(TimerDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;

                var expiredTimers = await _dbContext.Timers.Where(t => t.ExpiryTime <= now).ToListAsync();

                foreach (var timer in expiredTimers)
                {
                    try
                    {
                        var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
                        var httpClient = _httpClientFactory.CreateClient();
                        await httpClient.PostAsync(timer.WebUrl, content);

                        // Remove the timer from the database since it has expired
                        _dbContext.Timers.Remove(timer);
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Check for expired timers every 10 seconds
            }
        }
    }

}
