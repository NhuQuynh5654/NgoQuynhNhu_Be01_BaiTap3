using BaitapBackend3.Data;
using BaitapBackend3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaitapBackend3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllowAccessController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AllowAccessController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.AllowAccesses.Include(a => a.Role).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var access = await _context.AllowAccesses.FindAsync(id);
            if (access == null) return NotFound();
            return Ok(access);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AllowAccess access)
        {
            _context.AllowAccesses.Add(access);
            await _context.SaveChangesAsync();
            return Ok(access);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AllowAccess updated)
        {
            var access = await _context.AllowAccesses.FindAsync(id);
            if (access == null) return NotFound();

            _context.Entry(access).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var access = await _context.AllowAccesses.FindAsync(id);
            if (access == null) return NotFound();

            _context.AllowAccesses.Remove(access);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
