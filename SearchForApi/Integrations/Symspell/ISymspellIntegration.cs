using System;
using System.Threading.Tasks;

namespace SearchForApi.Integrations.Symspell
{
	public interface ISymspellIntegration
	{
        Task<string> CorrectPhrase(string phrase, int distance = 2);
    }
}

