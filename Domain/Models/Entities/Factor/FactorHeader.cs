using Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Factor
{
    public class FactorHeader:BaseEntity
    {
        public FactorHeader()
        {
            FactorSub = new List<FactorSub>();
        }

        [Column(TypeName = "int")]
        public int ID { get; set; }

        [Display(Name = "Date Shamsi")]
        [Column(TypeName = "nvarchar(16)")]
        [Required(ErrorMessage = "Enter Date in Shamsi.")]
        public string? Date { get; set; }

        [Display(Name = "Date Miladi")]
        public DateTime DateTime { get; set; }

        public int FactorNumber { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "nvarchar(Max)")]
        public string? Description { get; set; }

        public ICollection<FactorSub>? FactorSub { get; set; }
    }
}
