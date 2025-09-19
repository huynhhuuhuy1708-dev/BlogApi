using BlogAI.Data;
using BlogAI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context) { _context = context; }

        // Get all with pagination + search + sorting
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery]int page = 1, [FromQuery]int pageSize = 10, [FromQuery]string? search = null)
        {
            var query = _context.BlogPosts.Include(p => p.User).AsQueryable();

            if(!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));

            query = query.OrderByDescending(p => p.CreatedAt);

            var total = await query.CountAsync();
            var posts = await query.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            return Ok(new { total, page, pageSize, posts });
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogPost post)
        {
            post.UserId = int.Parse(User.FindFirst("id").Value);
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return Ok(post);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogPost post)
        {
            var dbPost = await _context.BlogPosts.FindAsync(id);
            if(dbPost == null) return NotFound();

            var userId = int.Parse(User.FindFirst("id").Value);
            var role = User.FindFirst("role")?.Value;

            if(dbPost.UserId != userId && role != "Admin")
                return Unauthorized("You cannot edit this post");

            dbPost.Title = post.Title;
            dbPost.Content = post.Content;
            await _context.SaveChangesAsync();
            return Ok(dbPost);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbPost = await _context.BlogPosts.FindAsync(id);
            if(dbPost == null) return NotFound();

            var userId = int.Parse(User.FindFirst("id").Value);
            var role = User.FindFirst("role")?.Value;

            if(dbPost.UserId != userId && role != "Admin")
                return Unauthorized("You cannot delete this post");

            _context.BlogPosts.Remove(dbPost);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }
    }
}
