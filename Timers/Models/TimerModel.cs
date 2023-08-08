using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Timers.Models
{
    public class TimerModel
    {
        [Key]
        public string Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string WebUrl { get; set; }
        public DateTime ExpiryTime { get; set; }
    }



}