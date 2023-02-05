using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
	public class ConfirmPhoneNumberModel
	{
		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string Token { get; set; }

		public DeviceModel Device { get; set; }
	}
}

