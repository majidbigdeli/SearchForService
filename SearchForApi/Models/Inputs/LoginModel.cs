using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; } // or username

        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        public DeviceModel Device { get; set; }
    }
}

