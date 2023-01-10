using Domain.Models.Entities.Cart;
using Domain.Models.Entities.Comments;
using Domain.Models.Entities.Products;
using Domain.Models.Entities.User;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Store.Models.Entities;
using System.Security.Claims;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private IUserRepository _Urepository;
        private IProductRepository _Prepository;
        private ICartRepository _Crepository;
        private ShopDbContext _context;

        public DashboardController(IUserRepository repository, IProductRepository _repository, ICartRepository __repository, ShopDbContext context)
        {
            _Crepository = __repository;
            _Urepository = repository;
            _Prepository = _repository;
            _context = context;

        }

        //---------------------------------------------------User Actions----------------------------------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _Urepository.GetUsers();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetUser(int id)
        {
            var user = _Urepository.GetUser(id);
            return new OkObjectResult(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _Urepository.DeleteUser(id);
            return new OkResult();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditRole(int id, [FromForm] User userobj)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("No record found against this Id");
            }

            user.Role = userobj.Role;
            user.UpdateTime = DateTime.Now;
            _context.SaveChanges();
            return Ok("Role Updated Successfully.");
        }

        //--------------------------------------------------------Product Actions------------------------------------------------

        //Post Products
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult PostProduct([FromForm] Product product)
        {
            //Get Last ID In Table Records
            var id = _context.Products
                             .OrderByDescending(x => x.ID)
                             .Take(1)
                             .Select(x => x.ID)
                             .ToList()
                             .FirstOrDefault();

            string File = Convert.ToString(id + 1);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsImages/", File + ".jpg");

            product.Images.CopyTo(new FileStream(imagePath, FileMode.Create));

            product.ImageUrl = imagePath.Remove(0, 7);
            product.InsertTime = DateTime.Now;
            _context.Products.Add(product);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Post Product Gallery
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ProductsGallery([FromForm] ProductsGallery products)
        {
            //Get Last ID In Table Records
            var id = _context.ProductsGalleries
                             .OrderByDescending(x => x.ID)
                             .Take(1)
                             .Select(x => x.ID)
                             .ToList()
                             .FirstOrDefault();

            string File = Convert.ToString(id + 1);

            int Folder = products.ProductID;

            var DC = Directory.CreateDirectory("wwwroot/ProductsGallery/" + Folder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsGallery/" + Folder, File + ".jpg");

            var galleryobj = new ProductsGallery
            {
                ProductID = products.ProductID,
                ImageUrl = products.ImageUrl
            };

            products.Images.CopyTo(new FileStream(imagePath, FileMode.Create));

            galleryobj.ImageUrl = imagePath.Remove(0, 7);
            galleryobj.InsertTime = DateTime.Now;
            _context.ProductsGalleries.Add(galleryobj);
            _context.SaveChanges();
            return Ok("Image Added.");
        }

        //Post Product Colors
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ProductColor([FromBody] OtherColors color)
        {
            var colorobj = new OtherColors
            {
                ProductID = color.ProductID,
                Color = color.Color
            };
            colorobj.InsertTime = DateTime.Now;
            _context.OtherColors.Add(colorobj);
            _context.SaveChanges();
            return Ok("Color Added.");
        }

        //Delete Products
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _Prepository.DeleteProduct(id);
            return new OkResult();
        }

        //Delete ProductsGallery
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProductGallery(int id)
        {
            _Prepository.DeleteProductGallery(id);
            return new OkResult();
        }

        //Delete ProductsColors
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProductColors(int id)
        {
            _Prepository.DeleteProductColors(id);
            return new OkResult();
        }

        //Update Products
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditProduct(int id, [FromForm] Product productobj)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                //Get Last ID In Table Records
                var Id = _context.Products.Where(e => e.ProductCode == product.ProductCode).Select(i => i.ID).FirstOrDefault();

                string File = Convert.ToString(Id);

                string FileName = File;

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsImages/", id + ".jpg");

                productobj.Images.CopyTo(new FileStream(imagePath, FileMode.Create));

                product.ImageUrl = imagePath.Remove(0, 7);

                product.ProductName = productobj.ProductName;
                product.ProductCode = productobj.ProductCode;
                product.Weight = productobj.Weight;
                product.Description = productobj.Description;
                product.Content = productobj.Content;
                product.Quantity = productobj.Quantity;
                product.Price = productobj.Price;
                product.ImageUrl = productobj.ImageUrl;
                product.HDD = productobj.HDD;
                product.SSD = productobj.SSD;
                product.RAM = productobj.RAM;
                product.CPU = productobj.CPU;
                product.Graphic = productobj.Graphic;
                product.ScreenSize = productobj.ScreenSize;
                product.Memory = productobj.Memory;
                product.Battery = productobj.Battery;
                product.Camera = productobj.Camera;
                product.SelfCamera = productobj.SelfCamera;
                product.Category = productobj.Category;
                product.UpdateTime = DateTime.Now;

                _context.SaveChanges();
                return Ok("Product updated successfully");
            }
        }

        //Update ProductsGallery
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditProductGallery(int id, [FromForm] ProductsGallery productobj)
        {
            var product = _context.ProductsGalleries.Find(id);
            if (product == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                //Get Last ID In Table Records
                var Id = _context.ProductsGalleries.Where(e => e.ID == product.ID).Select(i => i.ID).FirstOrDefault();

                string File = Convert.ToString(Id);

                string FileName = File;

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + File;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProductsGallery/" + product.ProductID, File + ".jpg");

                productobj.Images.CopyTo(new FileStream(imagePath, FileMode.Create));

                product.ImageUrl = imagePath.Remove(0, 7);

                product.UpdateTime = DateTime.Now;
                _context.SaveChanges();
                return Ok("Gallery updated successfully");
            }
        }

        //Update ProductsColors
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditProductColor(int id, [FromForm] OtherColors colorobj)
        {
            var color = _context.OtherColors.Find(id);
            if (color == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                color.Color = colorobj.Color;
                color.UpdateTime = DateTime.Now;
                _context.SaveChanges();
                return Ok("Colors updated successfully");
            }
        }
        //--------------------------------------------------------IsRemoved,RemoveTime --> Delete -----------------------------------

        //RemoveUser
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveUser(int id, User userobj)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                user.RemoveTime = DateTime.Now;
                user.UpdateTime = DateTime.Now;
                user.IsRemoved = true;
                _context.SaveChanges();
                return Ok("The Record Removed By Admin.");
            }
        }

        //RemoveColor
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveColor(int id, OtherColors colorobj)
        {
            var color = _context.OtherColors.Find(id);
            if (color == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                color.RemoveTime = DateTime.Now;
                color.UpdateTime = DateTime.Now;
                color.IsRemoved = true;
                _context.SaveChanges();
                return Ok("The Record Removed By Admin.");
            }
        }

        //RemoveGallery
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveImage(int id, ProductsGallery productobj)
        {
            var product = _context.ProductsGalleries.Find(id);
            if (product == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                product.RemoveTime = DateTime.Now;
                product.UpdateTime = DateTime.Now;
                product.IsRemoved = true;
                _context.SaveChanges();
                return Ok("The Record Removed By Admin.");
            }
        }

        //RemoveProduct
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveProduct(int id, Product productobj)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                product.RemoveTime = DateTime.Now;
                product.UpdateTime = DateTime.Now;
                product.IsRemoved = true;
                _context.SaveChanges();
                return Ok("The Record Removed By Admin.");
            }
        }

        //RemoveCarts
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveCarts(int id, Cart cartobj)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _context.Carts.Where(c => c.UserID == user.ID).ToList().ForEach(a => a.RemoveTime = DateTime.Now);
                _context.Carts.Where(c => c.UserID == user.ID).ToList().ForEach(a => a.UpdateTime = DateTime.Now);
                _context.Carts.Where(c => c.UserID == user.ID).ToList().ForEach(a => a.IsRemoved = true);

                _context.SaveChanges();
                return Ok("The Record Removed By Admin.");
            }
        }

        //RemoveCart
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveCart(int id, Cart cartobj)
        {
            var cart = _context.Carts.Find(id);
            cart.IsRemoved = true;
            cart.UpdateTime = DateTime.Now;
            cart.RemoveTime = DateTime.Now;

            _context.SaveChanges();
            return Ok("Cart Remove.");
        }

        //RemoveComment
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult RemoveComment(int id)
        {
            var comment = _context.Comments.Find(id);

            comment.IsRemoved = true;
            comment.UpdateTime = DateTime.Now;
            comment.RemoveTime = DateTime.Now;

            _context.SaveChanges();
            return Ok("Comment Remove.");
        }
        //------------------------------------------------Cart Actions---------------------------------------------------

        //Get Cart Details
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetCart(int id)
        {
            var cart = _context.Carts.Where(c => c.UserID == id).ToList();
            return Ok(cart);

        }

        //AllCarts
        [HttpGet]
        public IActionResult GetCarts()
        {
            var carts = _Crepository.GetCarts();
            return Ok(carts);
        }

        //DeleteCart
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cart = _context.Carts.Find(id);
            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return Ok("Cart Deleted.");
        }

        //--------------------------------------------------Discount actions------------------------------------------------------
        //PostDiscount
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult PostDiscount(int id, [FromForm] Product discountobj)
        {
            var discount = _context.Products.Find(id);

            discount.DiscountText = discountobj.DiscountText;
            discount.DiscountNum = discountobj.DiscountNum;
            discount.UpdateTime = DateTime.Now;

            _context.SaveChanges();
            return Ok("Discount Added.");
        }

        //UpdateDiscount
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditDiscount(int id, [FromForm] Product discountobj)
        {
            var discount = _context.Products.Find(id);
            discount.DiscountText = discountobj.DiscountText;
            discount.DiscountNum = discountobj.DiscountNum;
            discount.UpdateTime = DateTime.Now;

            _context.SaveChanges();
            return Ok("Discount Updated.");
        }

        //UpdateDiscount
        [Authorize(Roles = "Admin")]
        [HttpDelete("{discount}")]
        public IActionResult DeleteDiscount(string discount)
        {
            _context.Products.Where(p => p.DiscountText == discount).ToList().ForEach(p => p.DiscountText = null);
            _context.SaveChanges();
            return Ok("Discount Deleted.");
        }

        //GetDiscounts
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetDiscounts()
        {
            var discount = from d in _context.Products
                           where d.IsRemoved == false
                           where d.DiscountText != null

                           select new { DiscountText = d.DiscountText, DiscountNum = d.DiscountNum };
            return Ok(discount);
        }

        //GetDiscount
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetDiscount(int id)
        {
            var discount = from d in _context.Products
                           where d.ID == id
                           where d.IsRemoved == false

                           select new { DiscountText = d.DiscountText, DiscountNum = d.DiscountNum };
            //var discount = _context.Products.Where(d => d.ID == id).Where(d => d.IsRemoved == false).Select(d => d.DiscountText).FirstOrDefault();
            return Ok(discount);
        }

        //---------------------------------------------------Comment Actions-------------------------------------------------------------------------
        //DeleteComment
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return Ok("Comment Deleted Successfully.");
            }
        }

        //ConfirmComment
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult ConfirmComment(int id)
        {
            var comment = _context.Comments.Find(id);

            comment.UpdateTime = DateTime.Now;
            comment.Confirm = true;

            _context.SaveChanges();
            return Ok("Comment Confirmed.");
        }
    }
}
