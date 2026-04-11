using Microsoft.AspNetCore.Mvc;
using DigitalWellbeing.API.Data;
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

        // GET: api/usage/daily
        [HttpGet("daily")]
        public async Task<IActionResult> GetToday()
        {
            var today = DateTime.Today;

            var data = await _context.AppUsages
                .Where(x => x.StartTime.Date == today)
                .ToListAsync();

            return Ok(data);
        }
    }
}
