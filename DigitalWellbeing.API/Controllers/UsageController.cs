using Microsoft.AspNetCore.Mvc;

using DigitalWellbeing.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalWellbeing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usage/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.AppUsages.ToListAsync();
            return Ok(data);
        }

        // GET: api/usage/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _context.AppUsages
                .GroupBy(x => x.AppName)
                .Select(g => new
                {
                    AppName = g.Key,
                    TotalTime = g.Sum(x => x.DurationSeconds)
                })
                .ToListAsync();

            return Ok(summary);
        }

        //// GET: api/usage/daily
        //[HttpGet("daily")]
        //public async Task<IActionResult> GetToday()
        //{
        //    var today = DateTime.Today;

        //    var data = await _context.AppUsages
        //        .Where(x => x.StartTime.Date == today)
        //        .ToListAsync();

        //    return Ok(data);
        //}


        // GET: api/usage/today
        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var data = await _context.AppUsages
                .Where(x => x.StartTime >= today && x.StartTime < tomorrow)
                .ToListAsync();

            var hourly = data
                .GroupBy(x => x.StartTime.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalTime = g.Sum(x => x.DurationSeconds)
                })
                .OrderBy(x => x.Hour)
                .ToList();

            return Ok(hourly);
        }


        // GET: api/usage/insights
        [HttpGet("insights")]
        public async Task<IActionResult> GetInsights()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var data = await _context.AppUsages
                .Where(x => x.StartTime >= today && x.StartTime < tomorrow)
                .ToListAsync();

            if (!data.Any())
                return Ok(new List<string> { "No usage data available today." });

            var insights = new List<string>();

            // 🔥 1. Most used app
            var topApp = data
                .GroupBy(x => x.AppName)
                .OrderByDescending(g => g.Sum(x => x.DurationSeconds))
                .First();

            insights.Add($"You spent most time on {topApp.Key}");

            // 🔥 2. Total usage
            var totalTime = data.Sum(x => x.DurationSeconds);
            insights.Add($"Total usage today is {(totalTime / 60):0.0} minutes");

            // 🔥 3. Peak hour
            var peakHour = data
                .GroupBy(x => x.StartTime.Hour)
                .OrderByDescending(g => g.Sum(x => x.DurationSeconds))
                .First();

            insights.Add($"You were most active around {peakHour.Key}:00");

            return Ok(insights);
        }


    }
}
