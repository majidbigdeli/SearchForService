using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        public DeviceModel Device { get; set; }
    }
}

