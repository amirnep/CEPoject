using Domain.Models.Entities.Products;
using Store.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();

        Product GetProduct(int IDProduct);

        void DeleteProduct(int IDProduct);
        void DeleteProductGallery(int IDProductGallery);
        void DeleteProductColors(int IDProductColor);

        void Save();
    }
}
