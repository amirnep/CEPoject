using Domain.Entities.Commons;
using Domain.Models.Entities.Cart;
using Domain.Models.Entities.Comments;
using Domain.Models.Entities.Factor;
using Microsoft.AspNetCore.Http;
using Store.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Products
{
    public class Product:BaseEntity
    {
        //Cascade On Delete...
        public Product()
        {
            OtherColors = new List<OtherColors>();
            ProductsGallery = new List<ProductsGallery>();
            Cart = new List<Cart.Cart>();
            Comment = new List<Comment>();
        }

        [Column(TypeName = "int")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Product Name.")]
        [Column(TypeName = "nvarchar(20)")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Enter Product Id.")]
        [Column(TypeName = "nvarchar(20)")]
        public string? ProductCode { get; set; }

        [Required(ErrorMessage = "Enter Quantity.")]
        public long Quantity { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Enter Price.")]
        public long Price { get; set; }

        [NotMapped]
        public IFormFile Images { get; set; }


        public string? ImageUrl { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? HDD { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? SSD { get; set; }

        [Required(ErrorMessage = "Enter RAM.")]
        [Column(TypeName = "nvarchar(20)")]
        public string? RAM { get; set; }

        [Required(ErrorMessage = "Enter CPU.")]
        public string? CPU { get; set; }

        [Required(ErrorMessage = "Enter Graphic.")]
        public string? Graphic { get; set; }

        public string? ScreenSize { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Weight { get; set; }

        [Required(ErrorMessage = "Enter Memory.")]
        [Column(TypeName = "nvarchar(20)")]
        public string? Memory { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Battery { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Camera { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? SelfCamera { get; set; }

        [Required(ErrorMessage = "Enter Category.")]
        [Column(TypeName = "nvarchar(50)")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Enter Description.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Enter Content.")]
        public string? Content { get; set; }

        public string? DiscountText { get; set; }

        public int DiscountNum { get; set; }

        public ICollection<ProductsGallery> ProductsGallery { get; set; }
        public ICollection<OtherColors> OtherColors { get; set; }
        public ICollection<Cart.Cart> Cart { get; set; }
        public ICollection<Comment> Comment { get; set; }
        public ICollection<FactorSub> FactorSub { get; set; }
    }
}
