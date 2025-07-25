using BaitapBackend3.Data;
using BaitapBackend3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaitapBackend3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly AppDbContext _context;
    public RoleController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _context.Roles.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return Ok(role);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Role updated)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();
        _context.Entry(role).CurrentValues.SetValues(updated);
        await _context.SaveChangesAsync();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
