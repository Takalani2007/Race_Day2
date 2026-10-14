using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/participants")]
    public class ParticipantController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ParticipantController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var participants = await _db.Participants
                .Include(p => p.Event)
                .AsNoTracking()
                .ToListAsync();

            return Ok(participants);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var participant = await _db.Participants
                .Include(p => p.Event)
                .FirstOrDefaultAsync(p => p.ParticipantId == id);

            if (participant == null)
                return NotFound(new { message = "Participant not found." });

            return Ok(participant);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            // Check event exists
            if (!await _db.Events.AnyAsync(e => e.EventId == eventId))
                return NotFound(new { message = "Event not found." });

            var list = await _db.Participants
                .Where(p => p.EventId == eventId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // =====================================================
        // GET: api/participants/user/1
        // Returns all events a user is participating in
        // =====================================================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            if (!await _db.Users.AnyAsync(u => u.UserId == userId))
                return NotFound(new { message = "User not found." });

            var list = await _db.Participants
                .Include(p => p.Event)
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // =====================================================
        // POST: api/participants
        // Adds a participant to an event
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Participant participant)
        {
            if (participant == null)
                return BadRequest(new { message = "Invalid request." });

            // Validate event exists
            if (!await _db.Events.AnyAsync(e => e.EventId == participant.EventId))
                return NotFound(new { message = "Event not found." });

            // Validate user exists
            if (!await _db.Users.AnyAsync(u => u.UserId == participant.UserId))
                return NotFound(new { message = "User not found." });

            // Prevent duplicate participation
            var alreadyExists = await _db.Participants.AnyAsync(p =>
                p.EventId == participant.EventId &&
                p.UserId == participant.UserId);

            if (alreadyExists)
                return Conflict(new { message = "User is already a participant in this event." });

            if (string.IsNullOrWhiteSpace(participant.Status))
                participant.Status = "Confirmed";

            _db.Participants.Add(participant);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = participant.ParticipantId },
                participant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Participant updated)
        {
            if (updated == null)
                return BadRequest(new { message = "Invalid request." });

            var participant = await _db.Participants.FindAsync(id);
            if (participant == null)
                return NotFound(new { message = "Participant not found." });

            // Only update the status (other fields shouldn't change)
            if (!string.IsNullOrWhiteSpace(updated.Status))
                participant.Status = updated.Status;

            await _db.SaveChangesAsync();
            return Ok(participant);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var participant = await _db.Participants.FindAsync(id);
            if (participant == null)
                return NotFound(new { message = "Participant not found." });

            _db.Participants.Remove(participant);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}