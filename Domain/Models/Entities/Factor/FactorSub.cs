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
        public long Fee { get; set; }

        [Required(ErrorMessage = "Enter Mount.")]
        public long Mount { get; set; }

        public int DisCount { get; set; }

        public long TotalPrice => ((Fee * (DisCount / 100)) * Mount);
    }
}
