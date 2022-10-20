using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.User
{
    public class Login
    {
        [Required(ErrorMessage = "Enter Your Email for login.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password.")]
        public string Password { get; set; }
    }
}
