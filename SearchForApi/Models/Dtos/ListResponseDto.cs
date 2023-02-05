using System;
using System.Collections.Generic;

namespace SearchForApi.Models.Dtos
{
	public class ListResponseDto<T> : ResponseDto<List<T>>
	{
		public ListResponseDto(List<T> result) : base(result) { }
	}
}

