using DigitalWellbeing.API.Services;
using DigitalWellbeing.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalWellbeing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _gemini;

        public UsageController(AppDbContext context, GeminiService gemini)
        {
            _context = context;
            _gemini = gemini;
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
        [HttpGet("ai-insights")]
        public async Task<IActionResult> GetAIInsights()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var data = await _context.AppUsages
                .Where(x => x.StartTime >= today && x.StartTime < tomorrow)
                .ToListAsync();

            if (!data.Any())
                return Ok(new { message = "No insights generated." });

            var summary = data
                .GroupBy(x => x.AppName)
                .Select(g => new
                {
                    App = g.Key,
                    Time = g.Sum(x => x.DurationSeconds) / 60
                });

            // 👉 ADD THIS HERE
            var prompt = $@"
You are an AI assistant analyzing digital wellbeing.

App usage data (minutes):
{string.Join("\n", summary.Select(x => $"{x.App}: {x.Time:F2}"))}

Give 3 short insights:
- Most used app
- Behavior pattern
- Suggestion

Keep it simple.
";

            var aiResponse = await _gemini.GenerateInsights(prompt);

            return Ok(new { message = aiResponse });
        }


    }
}
