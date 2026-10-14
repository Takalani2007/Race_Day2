using Ice_Task2.Data;
using Ice_Task2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ice_Task2.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ResultsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/results
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Results
                .Include(r => r.Event)
                .AsNoTracking()
                .ToListAsync());

        // GET: api/results/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _db.Results
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.ResultId == id);

            if (result == null) return NotFound(new { message = "Result not found." });
            return Ok(result);
        }

        // GET: api/results/event/1
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var list = await _db.Results
                .Where(r => r.EventId == eventId)
                .AsNoTracking()
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/results
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Result result)
        {
            if (result == null)
                return BadRequest(new { message = "Invalid request." });

            if (!await _db.Events.AnyAsync(e => e.EventId == result.EventId))
                return NotFound(new { message = "Event not found." });

            if (!await _db.Participants.AnyAsync(p => p.ParticipantId == result.ParticipantId))
                return NotFound(new { message = "Participant not found." });

            _db.Results.Add(result);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = result.ResultId }, result);
        }

        // PUT: api/results/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Result updated)
        {
            var result = await _db.Results.FindAsync(id);
            if (result == null) return NotFound(new { message = "Result not found." });

            result.Outcome = updated.Outcome;
            result.Score = updated.Score;

            await _db.SaveChangesAsync();
            return Ok(result);
        }

        // DELETE: api/results/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _db.Results.FindAsync(id);
            if (result == null) return NotFound(new { message = "Result not found." });

            _db.Results.Remove(result);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}