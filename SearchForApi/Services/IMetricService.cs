using System.Threading.Tasks;

namespace SearchForApi.Services
{
    public interface IMetricService
    {
         Task<string> Get();
    }
}