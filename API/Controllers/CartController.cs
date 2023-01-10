using Domain.Models.Entities.Cart;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartRepository _repository;
        private ShopDbContext _context;

        public CartController(ICartRepository repository, ShopDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCart()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var Cart = from c in _context.Carts
                               join p in _context.Products
                               on c.ProductID equals p.ID

                               join u in _context.Users
                               on c.UserID equals u.ID

                               where c.UserID == user.ID && c.IsRemoved == false && c.Paid == false

                               select new
                               {
                                   ProductName = p.ProductName,
                                   ProductCode = p.ProductCode,
                                   Price = p.Price,
                                   FName = u.FName,
                                   LName = u.LName,
                                   Email = u.Email,
                                   Fee = c.Fee,
                                   Mount = c.Mount,
                                   TotalPrice = c.TotalPrice
                               };

                var TotalPrice = _context.Carts.Where(u => u.UserID == user.ID) .Where(u => u.IsRemoved == false).Where(u => u.Paid == false).
                    Select(p => p.TotalPrice).ToList().Sum();

                return Ok(new {Cart , TotalPrice });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostCart([FromForm] Cart cart)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.ID == cart.ProductID);
            int discount = _context.Products.Where(p => p.DiscountText == cart.DisCount).Select(c => c.DiscountNum).FirstOrDefault();

            if(discount == 0)
            {
                return NotFound("Discount Code Is Wrong.");
            }

            var cartobj = new Cart
            {
                ProductID = cart.ProductID,
                Mount = cart.Mount,
                DisCount = cart.DisCount,
                InsertTime = DateTime.Now
            };

            long dis = ((product.Price / 100) * discount);
            cartobj.UserID = user.ID;
            cartobj.Fee = product.Price - dis;

            _context.Carts.Add(cartobj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var cart = _context.Carts.Find(id);
            if (cart == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return Ok("Your Cart Deleted Successfully.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateCart(int id, [FromForm] Cart cartobj)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            var cart = _context.Carts.Find(id);
            if (cart == null)
            {
                return NotFound("No record found against this Id");
            }
            if (cart.IsRemoved == true || cart.Paid == true)
            {
                return NotFound("Cart Removed by Admin Or Paid.");
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.ID == cart.ProductID);
                int discount = _context.Products.Where(p => p.DiscountText == cart.DisCount).Select(c => c.DiscountNum).FirstOrDefault();

                if (discount == 0)
                {
                    return NotFound("Discount Code Is Wrong.");
                }

                long dis = ((product.Price / 100) * discount);

                cart.DisCount = cartobj.DisCount;
                cart.UpdateTime = DateTime.Now;
                cart.ProductID = cartobj.ProductID;
                cart.Fee = product.Price - dis;
                cart.Mount = cartobj.Mount;

                _context.SaveChanges();
                return Ok("Cart Updated Successfully.");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult PaidCart()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            _context.Carts.Where(c => c.UserID == user.ID).ToList().ForEach(c => c.Paid = true);
            _context.SaveChanges();
            return Ok("Carts Paid.");
        }

    }
}
