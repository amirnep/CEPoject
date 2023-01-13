using Domain.Entities.Commons;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.User
{
    public class User:BaseEntity
    {
        //Cascade On Delete...
        //public User()
        //{
            //Contact = new List<Contact>();
        //}

        [Column(TypeName = "int")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter Your UserName.")]
        public string? UserName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? FName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? LName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Address { get; set; }

        [Column(TypeName = "nvarchar(13)")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Enter Your Email for login.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        public string? Role { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password.")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password Confirm.")]
        [Compare("Password", ErrorMessage = "Passwords does not match.")]
        public string? ConfirmPassword { get; set; }

        public ICollection<Cart.Cart>? Cart { get; set; }
    }
}
