using Domain.Models.Entities;
using Domain.Models.Entities.Cart;
using Domain.Models.Entities.Comments;
using Domain.Models.Entities.Factor;
using Domain.Models.Entities.Products;
using Domain.Models.Entities.User;
using Microsoft.EntityFrameworkCore;
using Store.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductsGallery> ProductsGalleries { get; set; }

        public DbSet<OtherColors> OtherColors { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<FactorHeader> FactorHeaders { get; set; }

        public DbSet<FactorSub> FactorSubs { get; set; }
    }
}
