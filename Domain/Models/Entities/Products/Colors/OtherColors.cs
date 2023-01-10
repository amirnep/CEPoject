using Domain.Entities.Commons;
using Domain.Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Models.Entities
{
    public class OtherColors:BaseEntity
    {
        [Column(TypeName = "int")]
        public int ID { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Color { get; set; }
    }
}
