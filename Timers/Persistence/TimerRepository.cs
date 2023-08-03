using Timers.Models;

namespace Timers.Persistence
{
    public class TimerRepository
    {
        private readonly TimerDbContext _dbContext;

        public TimerRepository(TimerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddTimerAsync(TimerModel timer)
        {
            _dbContext.Timers.Add(timer);
            await _dbContext.SaveChangesAsync();
            return timer.Id;
        }

        public async Task<TimerModel> GetTimerByIdAsync(int id)
        {
            return await _dbContext.Timers.FindAsync(id);
        }
    }

}
