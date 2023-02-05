using System;
using System.ComponentModel.DataAnnotations;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Inputs
{
    public class RequestPlanModel
    {
        [Required]
        public PlanType PlanId { get; set; }

        [Required]
        public bool is3Months { get; set; }

        [Required]
        // [Url(ErrorMessage = "Invalid Redirect Url")] // fixme:
        public string RedirectUrl { get; set; }

        public string DiscountCode { get; set; }
    }
}

