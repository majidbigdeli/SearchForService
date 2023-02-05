using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class ForgetPasswordModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}

