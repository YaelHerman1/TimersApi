using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timers.Contract;
using Timers.Models;
using Timers.Persistence;

namespace Timers.Controllers
{
    /// <summary>
    /// Controller for managing timers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TimerController : ControllerBase
    {
        private readonly TimerRepository _timerRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public TimerController(TimerRepository timerRepository, IHttpClientFactory httpClientFactory)
        {
            _timerRepository = timerRepository;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Creates a new timer with the specified duration and web URL.
        /// </summary>
        /// <param name="timerInput">Input model containing timer details (hours, minutes, seconds, and web URL).</param>
        /// <returns>The unique ID of the created timer.</returns>
        [HttpPost("SetTimer")]
        public async Task<IActionResult> SetTimer([FromBody] TimerInputModel timerInput)
        {
            DateTime expiryTime = DateTime.Now.AddHours(timerInput.Hours).AddMinutes(timerInput.Minutes).AddSeconds(timerInput.Seconds);

            var timer = new TimerModel
            {
                Hours = timerInput.Hours,
                Minutes = timerInput.Minutes,
                Seconds = timerInput.Seconds,
                WebUrl = timerInput.WebUrl,
                ExpiryTime = expiryTime
            };

            int timerId = await _timerRepository.AddTimerAsync(timer);

            return Ok(new { id = timerId });
        }

        /// <summary>
        /// Retrieves the remaining seconds left until the timer with the specified ID expires.
        /// </summary>
        /// <param name="id">The ID of the timer to check.</param>
        /// <returns>The number of seconds left until the timer expires.</returns>
        [HttpGet("GetTimerStatus/{id}")]
        public async Task<IActionResult> GetTimerStatus(int id)
        {
            var timer = await _timerRepository.GetTimerByIdAsync(id);

            if (timer == null)
            {
                return NotFound();
            }

            // Calculate the seconds left based on the expiry time and current time
            int secondsLeft = (int)Math.Max(0, (timer.ExpiryTime - DateTime.Now).TotalSeconds);

            return Ok(new { secondsLeft });
        }
    }
}
