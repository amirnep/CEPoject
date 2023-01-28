using Domain.Models.Entities.Products;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Store.Models.Entities;
using System.Transactions;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;
        private ShopDbContext _context;

        public ProductController(IProductRepository repository, ShopDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        //Get Products Details
        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = _repository.GetProduct(id);
            return new OkObjectResult(product);
        }

        //Search Products
        [HttpGet]
        public IActionResult Search(string productName)
        {
            var prod = _context.Products.Where(p => p.ProductName == productName).Select(s => s.IsRemoved).FirstOrDefault();
            if (prod == true)
            {
                return NotFound("Product Not Found.");
            }
            var products = from p in _context.Products
                           where p.ProductName.StartsWith(productName)
                           select new
                           {
                               Id = p.ID,
                               Name = p.ProductName,
                               ImageUrl = p.ImageUrl
                           };
            return Ok(products);
        }

        //AllProducts
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _repository.GetProducts();
            return Ok(products);
        }

        //Category
        [HttpGet]
        public IActionResult GetCategory(string category)
        {
            var cat = _context.Products.Where(p => p.Category == category).ToList();
            return Ok(cat);
        }

        //ProductColors
        [HttpGet]
        public IActionResult GetProductColors(string ProductName)
        {
            var product = _context.Products.Where(p => p.ProductName == ProductName).Select(c => c.ID).ToList();
            var color = _context.OtherColors.Where(c => c.ProductID == Convert.ToInt32(product[0])).Select(c => c.Color).ToList();
            return Ok(color);
        }

        //Images
        [HttpGet]
        public IActionResult GetProductGallery(string ProductName)
        {
            var product = _context.Products.Where(p => p.ProductName == ProductName).Select(c => c.ID).ToList();
            var gallery = _context.ProductsGalleries.Where(c => c.ProductID == Convert.ToInt32(product[0])).Select(c => c.ImageUrl).ToList();
            return Ok(gallery);
        }
    }
}
