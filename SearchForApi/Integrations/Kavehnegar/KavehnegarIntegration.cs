using System;
using System.Threading.Tasks;
using Kavenegar;
using Kavenegar.Core.Exceptions;
using Kavenegar.Core.Models;
using MethodTimer;
using SearchForApi.Core;

namespace SearchForApi.Integrations.Kavehnegar
{
    public class KavehnegarIntegration : IKavehnegarIntegration
    {
        private readonly KavenegarApi _api;

        public KavehnegarIntegration()
        {
            _api = new KavenegarApi(Cfg.KavehnegarApiKey);
        }

        [Time("phoneNumber={phoneNumber},token={token}")]
        public async Task<ResultDto<SendResult>> SendVerificationToken(string phoneNumber, string token)
        {
            try
            {
                var result = await _api.VerifyLookup(phoneNumber, token, "VerifyPhoneNumber");

                return new ResultDto<SendResult>
                {
                    Succeeded = true,
                    Result = result
                };
            }
            catch (ApiException ex)
            {
                Serilog.Log.Error(ex, "Third-Party Exception: {integration}/{api}", "Kavehnegar", "Verification-Token");
                return new ResultDto<SendResult>
                {
                    Succeeded = false,
                    ErrorMessage = ex.Message,
                };
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", "Kavehnegar", "Verification-Token");
                return new ResultDto<SendResult>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}

