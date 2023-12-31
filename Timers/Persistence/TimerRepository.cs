﻿using Timers.Models;

namespace Timers.Persistence
{
    public class TimerRepository
    {
        private readonly TimerDbContext _dbContext;

        public TimerRepository(TimerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddTimerAsync(TimerModel timer)
        {
            _dbContext.Timers.Add(timer);
            await _dbContext.SaveChangesAsync();
            return timer.Id;
        }

        public async Task<TimerModel> GetTimerByIdAsync(string id)
        {
            return await _dbContext.Timers.FindAsync(id);
        }
    }

}
