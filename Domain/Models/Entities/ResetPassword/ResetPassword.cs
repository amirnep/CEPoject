using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.ResetPassword
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Enter Your UserName.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "NewPassword is Required.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmNewPassword is Required.")]
        public string? ConfirmNewPassword { get; set; }
    }
}
