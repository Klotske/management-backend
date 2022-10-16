using management_api.Data;
using management_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private readonly MSSQLContext _context;

        public DepartmentsController(MSSQLContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        {
            return await _context.Departments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null) return NotFound();

            return department;
        }

        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department department)
        {
            await _context.Departments.AddAsync(department);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Department department)
        {
            if (id != department.Id) return BadRequest();

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(d => d.Id == id))
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
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
