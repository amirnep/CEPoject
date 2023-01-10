using Domain.Models.Entities.Products;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Store.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ShopDbContext _context;

        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }

        public void DeleteProduct(int IDProduct)
        {
            var product = _context.Products.Find(IDProduct);
            _context.Products.Remove(product);
            Save();
        }

        public void DeleteProductColors(int IDProductColor)
        {
            var productcolor = _context.OtherColors.Find(IDProductColor);
            _context.OtherColors.Remove(productcolor);
            Save();
        }

        public void DeleteProductGallery(int IDProductGallery)
        {
            var productgallery = _context.ProductsGalleries.Find(IDProductGallery);
            _context.ProductsGalleries.Remove(productgallery);
            Save();
        }

        public Product GetProduct(int IDProduct)
        {
            var product = _context.Products.Find(IDProduct);
            if (product.IsRemoved == false)
            {
                return product;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            var product = _context.Products.Where(p => p.IsRemoved == false).ToList();
            return product;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
