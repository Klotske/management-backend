using management_api.Data;
using management_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_api.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly MSSQLContext _context;

        public SchedulesController(MSSQLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetAll()
        {
            return await _context.Schedules.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetById(int id)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null) return NotFound();

            return schedule;
        }

        [HttpPost]
        public async Task<ActionResult<Schedule>> Create(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, GetById(schedule.Id).Result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Schedule schedule)
        {
            if (id != schedule.Id) return BadRequest();

            _context.Entry(schedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Schedules.Any(s => s.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null) return NotFound();

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
