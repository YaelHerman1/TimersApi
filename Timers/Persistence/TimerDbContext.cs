using Microsoft.EntityFrameworkCore;
using Timers.Models;

namespace Timers.Persistence
{
    public class TimerDbContext : DbContext
    {
        public TimerDbContext(DbContextOptions<TimerDbContext> options) : base(options)
        {
        }

        public DbSet<TimerModel> Timers { get; set; }
    }
}
