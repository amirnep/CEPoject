using Domain.Entities.Commons;
using Domain.Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Comments
{
    public class Comment:BaseEntity
    {
        [Column(TypeName = "int")]
        public int ID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Enter Your Email.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter Comment.")]
        public string? Message { get; set; }

        public bool Confirm { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

    }
}
