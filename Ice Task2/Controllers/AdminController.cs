using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/admin/events")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents() =>
            Ok(await _db.Events.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound(new { message = "Event not found." });
            return Ok(ev);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequest request)
        {
            if (request == null) return BadRequest(new { message = "Invalid request." });

            var newEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location
            };

            _db.Events.Add(newEvent);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound(new { message = "Event not found." });

            ev.Name = updatedEvent.Name;
            ev.Description = updatedEvent.Description;
            ev.Date = updatedEvent.Date;
            ev.Location = updatedEvent.Location;

            await _db.SaveChangesAsync();
            return Ok(ev);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _db.Events.FindAsync(id);
            if (ev == null) return NotFound(new { message = "Event not found." });

            _db.Events.Remove(ev);   // cascade deletes children automatically
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}