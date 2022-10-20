using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.User
{
    public class CompleteProfileModel
    {
        [Column(TypeName = "nvarchar(20)")]
        public string? FName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? LName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Address { get; set; }

        [Column(TypeName = "nvarchar(13)")]
        public string? Phone { get; set; }
    }
}
