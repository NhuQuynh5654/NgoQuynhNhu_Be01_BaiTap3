using BaitapBackend3.Data;
using BaitapBackend3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Security.Claims;

namespace BaitapBackend3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InternController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InternController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetInterns()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                // Nếu người dùng chưa đăng nhập
                return Unauthorized(new { message = "Bạn cần đăng nhập để truy cập tài nguyên này." });
            }

            var userId = int.Parse(userIdClaim);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                // Nếu không tìm thấy người dùng (lỗi dữ liệu hoặc token không hợp lệ)
                return Unauthorized(new { message = "Tài khoản không hợp lệ." });
            }

            string[] props;

            // Kiểm tra quyền truy cập dựa trên RoleId
            if (user.RoleId == 2)
            {
                // Nếu là Admin, trả về tất cả các trường
                props = typeof(Intern).GetProperties().Select(p => p.Name).ToArray();
            }
            else
            {
                // Nếu là User, kiểm tra quyền trên bảng AllowAccess
                var access = await _context.AllowAccesses
                    .FirstOrDefaultAsync(a => a.RoleId == user.RoleId && a.TableName == "Intern");

                if (access == null)
                {
                    // Nếu không có quyền truy cập
                    return Unauthorized(new { message = "Bạn cần đăng nhập để truy cập tài nguyên này." });

                }

                props = access?.AccessProperties?.Split(",") ?? Array.Empty<string>();
            }

            var interns = await _context.Interns.ToListAsync();

            // Chuyển các kết quả thành một danh sách các đối tượng động (dynamic)
            var result = interns.Select(i =>
            {
                var obj = new ExpandoObject() as IDictionary<string, object>;
                foreach (var p in props)
                {
                    var value = i.GetType().GetProperty(p.Trim())?.GetValue(i);
                    obj[p.Trim()] = value;
                }
                return obj;
            });

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> AddIntern([FromBody] Intern intern)
        {
            _context.Interns.Add(intern);
            await _context.SaveChangesAsync();
            return Ok(intern);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIntern(int id, [FromBody] Intern updated)
        {
            var intern = await _context.Interns.FindAsync(id);
            if (intern == null) return NotFound();

            _context.Entry(intern).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntern(int id)
        {
            var intern = await _context.Interns.FindAsync(id);
            if (intern == null) return NotFound();

            _context.Interns.Remove(intern);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}