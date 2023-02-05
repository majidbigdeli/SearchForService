using System;
using System.ComponentModel.DataAnnotations;

namespace SearchForApi.Models.Inputs
{
	public class ResendPhoneNumberConfirmationModel
	{
		[Required]
		public string PhoneNumber { get; set; }
	}
}

