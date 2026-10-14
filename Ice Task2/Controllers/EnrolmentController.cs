using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/enrolments")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EnrolmentsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/enrolments
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Enrolments
                .Include(e => e.User)
                .Include(e => e.Event)
                .AsNoTracking()
                .ToListAsync());

        // GET: api/enrolments/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrolment = await _db.Enrolments
                .Include(e => e.User)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentId == id);

            if (enrolment == null) return NotFound(new { message = "Enrolment not found." });
            return Ok(enrolment);
        }

        // GET: api/enrolments/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var list = await _db.Enrolments
                .Include(e => e.User)
                .Where(e => e.EventId == eventId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/enrolments/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _db.Enrolments
                .Include(e => e.Event)
                .Where(e => e.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/enrolments
        [HttpPost]
        public async Task<IActionResult> Enrol([FromBody] Enrolment enrolment)
        {
            if (enrolment == null)
                return BadRequest(new { message = "Invalid request." });

            // Validate event
            if (!await _db.Events.AnyAsync(e => e.EventId == enrolment.EventId))
                return NotFound(new { message = "Event not found." });

            // Validate user
            if (!await _db.Users.AnyAsync(u => u.UserId == enrolment.UserId))
                return NotFound(new { message = "User not found." });

            // Prevent duplicate enrolment
            var exists = await _db.Enrolments.AnyAsync(e =>
                e.UserId == enrolment.UserId && e.EventId == enrolment.EventId);

            if (exists)
                return Conflict(new { message = "User is already enrolled in this event." });

            enrolment.EnrolledAt = DateTime.Now;
            _db.Enrolments.Add(enrolment);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = enrolment.EnrolmentId }, enrolment);
        }

        // DELETE: api/enrolments/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var enrolment = await _db.Enrolments.FindAsync(id);
            if (enrolment == null) return NotFound(new { message = "Enrolment not found." });

            _db.Enrolments.Remove(enrolment);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}