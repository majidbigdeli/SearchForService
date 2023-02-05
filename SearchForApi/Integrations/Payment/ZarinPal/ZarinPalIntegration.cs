using System;
using System.Net;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Http;
using RestSharp;
using SearchForApi.Core;

namespace SearchForApi.Integrations.Payment.ZarinPal
{
    public class ZarinPalIntegration : PaymentBaseIntegration
    {
        private readonly string _baseUrl = "https://api.zarinpal.com/pg/v4/payment/";
        private readonly RestClient _client;

        public ZarinPalIntegration(string _accessToken) : base(_accessToken)
        {
            _client = new RestClient(_baseUrl);
        }

        public override string Name => "ZarinPal";

        public override string GetPayUrl(string code) => $"https://www.zarinpal.com/pg/StartPay/{code}";

        [Time("refId={refId},amount={amount},description={description}")]
        public override async Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description)
        {
            try
            {
                var request = new RestRequest("request.json", Method.POST).AddJsonBody(new
                {
                    merchant_id = AccessToken,
                    amount,
                    callback_url = Cfg.PaymentReturnUrl(refId),
                    description,
                });

                var result = await _client.ExecutePostAsync<ZarinPalRequestTokenDto>(request);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

                var data = result.Data.data.Count > 0 ? result.Data.data[0] : null;
                var errors = result.Data.errors.Count > 0 ? result.Data.errors[0] : null;

                if (data?.code != 100)
                    throw new Exception(GetStatusMessage(errors.code));

                return new ResultDto<RequestTokenDto>
                {
                    Succeeded = true,
                    Result = new RequestTokenDto
                    {
                        Code = data.authority,
                        PayUrl = GetPayUrl(data.authority),
                        GatewayType = PaymentGatewayType.ZarinPal
                    }
                };
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", Name, "Request-Token");
                return new ResultDto<RequestTokenDto>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message,
                    Result = new RequestTokenDto { GatewayType = PaymentGatewayType.ZarinPal }
                };
            }
        }

        [Time("paymentRefId={paymentRefId},amount={amount}")]
        public override async Task<ResultDto<VerifyPaymentDto>> VerifyPayment(string paymentRefId, int amount)
        {
            try
            {
                var request = new RestRequest("verify.json", Method.POST).AddJsonBody(new
                {
                    merchant_id = AccessToken,
                    amount,
                    authority = paymentRefId,
                });

                var result = await _client.ExecutePostAsync<ZarinPalVerifyPaymentDto>(request);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

                var data = result.Data.data.Count > 0 ? result.Data.data[0] : null;
                var errors = result.Data.errors.Count > 0 ? result.Data.errors[0] : null;

                if (data?.code != 100)
                    throw new Exception(GetStatusMessage(errors.code));

                return new ResultDto<VerifyPaymentDto>
                {
                    Succeeded = true,
                    Result = new VerifyPaymentDto
                    {
                        CardNumber = data.card_pan,
                        CardHashPan = data.card_hash,
                    }
                };
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", Name, "Verify-Payment");
                return new ResultDto<VerifyPaymentDto> { Succeeded = false, ErrorMessage = e.Message };
            }
        }

        public override ResultDto<CallbackResultDto> ParseCallbackResult(IFormCollection formData, IQueryCollection queryString)
        {
            var status = queryString["Status"].ToString();
            var refId = queryString["Authority"].ToString();

            var isSucceeded = status == "OK";
            string message = isSucceeded ? GetStatusMessage(100) : GetStatusMessage(-51);

            return new ResultDto<CallbackResultDto>
            {
                ErrorMessage = message,
                Succeeded = isSucceeded,
                Result = new CallbackResultDto
                {
                    PaymentRefId = isSucceeded ? refId : status,
                }
            };
        }

        public override string GetStatusMessage(int code)
        {
            string message = null;
            switch (code)
            {
                case -9:
                    message = "خطای اعتبار سنجی";
                    break;
                case -10:
                    message = "ای پی و يا مرچنت كد پذيرنده صحيح نيست";
                    break;
                case -11:
                    message = "مرچنت کد فعال نیست لطفا با تیم پشتیبانی ما تماس بگیرید";
                    break;
                case -12:
                    message = "تلاش بیش از حد در یک بازه زمانی کوتاه.";
                    break;
                case -15:
                    message = "ترمینال شما به حالت تعلیق در آمده با تیم پشتیبانی تماس بگیرید";
                    break;
                case -16:
                    message = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است.";
                    break;
                case 100:
                    message = "عملیات موفق";
                    break;
                case -30:
                    message = "اجازه دسترسی به تسویه اشتراکی شناور ندارید";
                    break;
                case -31:
                    message = "حساب بانکی تسویه را به پنل اضافه کنید مقادیر وارد شده واسه تسهیم درست نیست";
                    break;
                case -32:
                    message = "Wages is not valid, Total wages(floating) has been overload max amount.";
                    break;
                case -33:
                    message = "درصد های وارد شده درست نیست";
                    break;
                case -34:
                    message = "مبلغ از کل تراکنش بیشتر است";
                    break;
                case -35:
                    message = "تعداد افراد دریافت کننده تسهیم بیش از حد مجاز است";
                    break;
                case -40:
                    message = "Invalid extra params, expire_in is not valid.";
                    break;
                case -50:
                    message = "مبلغ پرداخت شده با مقدار مبلغ در وریفای متفاوت است";
                    break;
                case -51:
                    message = "پرداخت ناموفق";
                    break;
                case -52:
                    message = "خطای غیر منتظره با پشتیبانی تماس بگیرید";
                    break;
                case -53:
                    message = "اتوریتی برای این مرچنت کد نیست";
                    break;
                case -54:
                    message = "اتوریتی نامعتبر است";
                    break;
                case 101:
                    message = "تراکنش وریفای شده";
                    break;
            }

            return message;
        }
    }
}
