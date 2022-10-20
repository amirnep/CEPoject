using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.User
{
    public class ChangeProfile
    {
        [NotMapped]
        public IFormFile Images { get; set; }
    }
}
