using System.Threading.Tasks;
using SearchForApi.Models.Entities;

namespace SearchForApi.Factories
{
    public interface IBannedKeywordFactory
    {
         Task<string> MaskBannedKeyword(string text, SceneLangaugeType language);
         Task<bool> HasBannedKeyword(string text, SceneLangaugeType language);
    }
}