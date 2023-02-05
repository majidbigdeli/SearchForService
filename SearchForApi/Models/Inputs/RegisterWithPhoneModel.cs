using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
	public class RegisterWithPhoneModel
	{
        [Required]
        public string PhoneNumber { get; set; }

        public DeviceModel Device { get; set; }
    }
}

