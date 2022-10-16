using management_api.Data;
using management_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_api.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionsController : Controller
    {
        private readonly MSSQLContext _context;

        public PositionsController(MSSQLContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetAll()
        {
            return await _context.Positions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetById(int id)
        {
            var position = await _context.Positions.FindAsync(id);

            if (position == null) return NotFound();

            return position;
        }

        [HttpPost]
        public async Task<ActionResult<Position>> Create(Position position)
        {
            await _context.Positions.AddAsync(position);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = position.Id }, position);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Position position)
        {
            if (id != position.Id) return BadRequest();

            _context.Entry(position).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Positions.Any(p => p.Id == id))
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
            var position = await _context.Positions.FindAsync(id);
            if (position == null) return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
