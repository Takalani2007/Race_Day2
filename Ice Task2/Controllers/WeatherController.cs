using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly AppDbContext _db;

        public WeatherController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/weather
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Weathers.AsNoTracking().ToListAsync());

        // GET: api/weather/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var weather = await _db.Weathers.FindAsync(id);
            if (weather == null) return NotFound(new { message = "Weather not found." });
            return Ok(weather);
        }

        // GET: api/weather/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var list = await _db.Weathers
                .Where(w => w.EventId == eventId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/weather
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Weather weather)
        {
            if (weather == null)
                return BadRequest(new { message = "Invalid request." });

            // Validate the event exists
            if (!await _db.Events.AnyAsync(e => e.EventId == weather.EventId))
                return NotFound(new { message = "Event not found." });

            weather.RecordedAt = DateTime.Now;
            _db.Weathers.Add(weather);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = weather.WeatherId }, weather);
        }

        // PUT: api/weather/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Weather updated)
        {
            var weather = await _db.Weathers.FindAsync(id);
            if (weather == null) return NotFound(new { message = "Weather not found." });

            weather.Condition = updated.Condition;
            weather.Temperature = updated.Temperature;

            await _db.SaveChangesAsync();
            return Ok(weather);
        }

        // DELETE: api/weather/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var weather = await _db.Weathers.FindAsync(id);
            if (weather == null) return NotFound(new { message = "Weather not found." });

            _db.Weathers.Remove(weather);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}