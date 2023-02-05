using System;
namespace SearchForApi.Integrations.Symspell
{
	public class SymspellResultDto
	{
		public string term { get; set; }
		public int distance { get; set; }
		public int count { get; set; }
	}
}

