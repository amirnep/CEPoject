using Domain.Models.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ICartRepository
    {
        IEnumerable<Cart> GetCarts();
        void DeleteCart(int IDCart);
        void Save();
    }
}
