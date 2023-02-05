using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class GoogleLoginModel
    {
        [Required]
        public string IdToken { get; set; }

        public DeviceModel Device { get; set; }
    }
}