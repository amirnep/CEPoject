using Domain.Entities.Commons;
using Domain.Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Factor
{
    public class FactorSub:BaseEntity
    {
        [Column(TypeName = "int")]
        public int ID { get; set; }

        public int FactorHeaderID { get; set; }
        public FactorHeader? FactorHeader { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Enter Correct Fee.")]
        public double Fee { get; set; }

        [Required(ErrorMessage = "Enter Mount.")]
        public double Mount { get; set; }

        public double DisCount { get; set; }

        public double Price => (Fee * (DisCount / 100));

        public double TotalPrice => ((Fee - Price) * Mount);
    }
}
