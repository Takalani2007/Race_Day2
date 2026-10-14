using Ice_Task2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EventController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Events
                .AsNoTracking()
                .ToListAsync());

        // GET: api/events/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _db.Events
                .Include(e => e.Weathers)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (ev == null) return NotFound(new { message = "Event not found." });
            return Ok(ev);
        }

        // GET: api/events/upcoming
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var upcoming = await _db.Events
                .Where(e => e.Date >= DateTime.Now)
                .OrderBy(e => e.Date)
                .AsNoTracking()
                .ToListAsync();

            return Ok(upcoming);
        }
    }
}