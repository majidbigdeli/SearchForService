using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
    public class DiscountModel
    {
        [Required]
        public string DiscountCode { get; set; }
    }
}