using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Timers.Models
{
    // Timer.cs
    public class TimerModel
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string WebUrl { get; set; }
        public DateTime ExpiryTime { get; set; }
    }



}