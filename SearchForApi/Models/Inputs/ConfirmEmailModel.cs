using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class ConfirmEmailModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}

