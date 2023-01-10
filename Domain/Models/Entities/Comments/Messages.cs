using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Comments
{
    public class Messages
    {
        [Required(ErrorMessage = "Enter Comment.")]
        public string? Message { get; set; }
    }
}
