using management_api.Data;
using management_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_api.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly MSSQLContext _context;

        public RatesController(MSSQLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rate>>> GetAll()
        {
            return await _context.Rates
                .Include(r => r.Position)
                .ToListAsync();
        }

        [HttpGet("between")]
        public async Task<ActionResult<IEnumerable<Rate>>> GetBetween(DateTime startDate, DateTime endDate)
        {
            return await _context.Rates
                .Where(r => (r.StartDate >= startDate)
                    && (r.StartDate <= endDate))
                .Include(r => r.Position)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rate>> GetById(int id)
        {
            var rate = await _context.Rates
                .Include(r => r.Position)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rate == null) return NotFound();

            return rate;
        }

        [HttpPost]
        public async Task<ActionResult<Rate>> Create(Rate rate)
        {
            await _context.Rates.AddAsync(rate);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rate.Id }, GetById(rate.Id).Result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Rate rate)
        {
            if (id != rate.Id) return BadRequest();

            _context.Entry(rate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rates.Any(r => r.Id == id))
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
            var rate = await _context.Rates.FindAsync(id);
            if (rate == null) return NotFound();

            _context.Rates.Remove(rate);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
