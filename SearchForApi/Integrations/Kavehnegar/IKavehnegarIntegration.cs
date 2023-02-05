using System.Threading.Tasks;
using Kavenegar.Core.Models;

namespace SearchForApi.Integrations.Kavehnegar
{
	public interface IKavehnegarIntegration
	{
		Task<ResultDto<SendResult>> SendVerificationToken(string phoneNumber, string token);
	}
}

