using Domain.Entities.Commons;
using Domain.Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Cart
{
    public class Cart:BaseEntity
    {
        [Column(TypeName = "int")]
        public int ID { get; set; }

        [DataType(DataType.Currency)]
        public long Fee { get; set; }

        public long Mount { get; set; }

        public long TotalPrice => Fee * Mount;

        public string? DisCount { get; set; }

        public bool Paid { get; set; }


        public int UserID { get; set; }
        public User.User? user { get; set; }

        public int ProductID { get; set; }
        public Product? product { get; set; }
    }
}
