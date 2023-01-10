using Domain.Models.Entities.Cart;
using Infrastructure.Repositories.Interfaces;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CartRepository:ICartRepository
    {
        private ShopDbContext _context;

        public CartRepository(ShopDbContext context)
        {
            _context = context;
        }

        public void DeleteCart(int IDCart)
        {
            var cart = _context.Carts.Find(IDCart);
            _context.Carts.Remove(cart);
            Save();
        }

        public IEnumerable<Cart> GetCarts()
        {
            var cart = _context.Carts.Where(p => p.IsRemoved == false).ToList();
            return cart;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
