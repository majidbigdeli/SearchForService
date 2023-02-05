using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class ChangePasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
    }
}

