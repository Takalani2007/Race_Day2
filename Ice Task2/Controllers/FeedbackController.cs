using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _db;

        public FeedbackController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/feedback
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Event)
                .AsNoTracking()
                .ToListAsync());

        // GET: api/feedback/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _db.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Event)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback == null) return NotFound(new { message = "Feedback not found." });
            return Ok(feedback);
        }

        // GET: api/feedback/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var list = await _db.Feedbacks
                .Include(f => f.User)
                .Where(f => f.EventId == eventId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/feedback/event/1
        [HttpPost("event/{eventId}")]
        public async Task<IActionResult> Submit(int eventId, [FromBody] FeedbackRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request." });

            if (!await _db.Events.AnyAsync(e => e.EventId == eventId))
                return NotFound(new { message = "Event not found." });

            if (!await _db.Users.AnyAsync(u => u.UserId == request.UserId))
                return NotFound(new { message = "User not found." });

            if (request.Rating < 1 || request.Rating > 5)
                return UnprocessableEntity(new { message = "Rating must be between 1 and 5." });

            var feedback = new Feedback
            {
                EventId = eventId,
                UserId = request.UserId,
                Comment = request.Comment,
                Rating = request.Rating,
                CreatedAt = DateTime.Now
            };

            _db.Feedbacks.Add(feedback);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = feedback.FeedbackId }, feedback);
        }

        // DELETE: api/feedback/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _db.Feedbacks.FindAsync(id);
            if (feedback == null) return NotFound(new { message = "Feedback not found." });

            _db.Feedbacks.Remove(feedback);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}