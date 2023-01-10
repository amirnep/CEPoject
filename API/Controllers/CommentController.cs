using Domain.Models.Entities.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Persistence;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ShopDbContext _context;
        public CommentController(ShopDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetComments(int id)
        {
            var comment = from c in _context.Comments
                          where c.ProductID == id
                          where c.Confirm == true
                          where c.IsRemoved == false

                          select new { Name = c.Name, Message = c.Message };
            return Ok(comment);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return Ok("Your Comment Deleted Successfully.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult EditComment(int id, [FromForm] Messages commentobj)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound("No record found against this Id");
            }
            if (comment.IsRemoved == true || comment.Confirm == false)
            {
                return NotFound("Cart Removed by Admin Or not Confirmed.");
            }

            else
            {
                comment.Message = commentobj.Message;
                comment.UpdateTime = DateTime.Now;
                comment.Confirm = false;

                _context.SaveChanges();
                return Ok("Comment Updated Successfully.");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostComment([FromForm] Comment comment)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var commentobj = new Comment
            {
                ProductID = comment.ProductID,
                InsertTime = DateTime.Now,
                Name = comment.Name,
                Email = comment.Email,
                Message = comment.Message,
            };

            _context.Comments.Add(commentobj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
