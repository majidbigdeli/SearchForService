using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class ConfirmForgetPasswordModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}

