using System;
using System.Net;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.AspNetCore.Http;
using RestSharp;
using SearchForApi.Core;

namespace SearchForApi.Integrations.Payment.PayPing
{
    public class PayPingIntegration : PaymentBaseIntegration
    {
        private readonly string _baseUrl = "https://api.payping.ir/v2/";
        private readonly RestClient _client;

        public PayPingIntegration(string _accessToken) : base(_accessToken)
        {
            _client = new RestClient(_baseUrl);
            _client.AddDefaultHeader("Authorization", $"bearer {_accessToken}");
        }

        public override string Name => "PayPing";

        public override string GetPayUrl(string code) => $"https://api.payping.ir/v2/pay/gotoipg/{code}";

        [Time("refId={refId},amount={amount},description={description}")]
        public override async Task<ResultDto<RequestTokenDto>> RequestToken(string refId, int amount, string description)
        {
            try
            {
                var request = new RestRequest("pay", Method.POST).AddJsonBody(new
                {
                    amount = amount / 10,
                    clientRefId = refId,
                    returnUrl = Cfg.PaymentReturnUrl(refId),
                    description
                });

//                _client.Timeout = 4000;
                var result = await _client.ExecutePostAsync<PayPingRequestTokenDto>(request);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

                var data = result.Data;

                return new ResultDto<RequestTokenDto>
                {
                    Succeeded = true,
                    Result = new RequestTokenDto
                    {
                        Code = data.code,
                        PayUrl = GetPayUrl(data.code),
                        GatewayType = PaymentGatewayType.PayPing
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
                    Result = new RequestTokenDto { GatewayType = PaymentGatewayType.PayPing }
                };
            }
        }

        [Time("paymentRefId={paymentRefId},amount={amount}")]
        public override async Task<ResultDto<VerifyPaymentDto>> VerifyPayment(string paymentRefId, int amount)
        {
            try
            {
                var request = new RestRequest("pay/verify", Method.POST).AddJsonBody(new
                {
                    refId = paymentRefId,
                    amount = amount / 10,
                });

                _client.Timeout = 100000;
                var result = await _client.ExecutePostAsync<PayPingVerifyPaymentDto>(request);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"StatusCode: {result.StatusCode}, Message: {result.Content}");

                var data = result.Data;

                return new ResultDto<VerifyPaymentDto>
                {
                    Succeeded = true,
                    Result = new VerifyPaymentDto
                    {
                        CardNumber = data.cardNumber,
                        CardHashPan = data.cardHashPan,
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
            var statusCode = formData["refid"].ToString();
            var cardNumber = formData["cardnumber"].ToString();
            var cardHashPan = formData["cardhashpan"].ToString();

            var statusCodeIsValid = int.TryParse(statusCode, out int normalizedStatusCode);
            string message = statusCodeIsValid ? GetStatusMessage(normalizedStatusCode) : null;

            return new ResultDto<CallbackResultDto>
            {
                ErrorMessage = message,
                Succeeded = statusCode.Contains("00") && message == null,
                Result = new CallbackResultDto
                {
                    PaymentRefId = statusCode,
                }
            };
        }

        public override string GetStatusMessage(int code)
        {
            string message = null;
            switch (code)
            {
                case 1:
                    message = "تراكنش توسط شما لغو شد";
                    break;
                case 2:
                    message = "رمز کارت اشتباه است.";
                    break;
                case 3:
                    message = "cvv2 یا تاریخ انقضای کارت وارد نشده است";
                    break;
                case 4:
                    message = "موجودی کارت کافی نیست.";
                    break;
                case 5:
                    message = "تاریخ انقضای کارت گذشته است و یا اشتباه وارد شده.";
                    break;
                case 6:
                    message = "کارت شما مسدود شده است";
                    break;
                case 7:
                    message = "تراکنش مورد نظر توسط درگاه یافت نشد";
                    break;
                case 8:
                    message = "بانک صادر کننده کارت شما مجوز انجام تراکنش را صادر نکرده است";
                    break;
                case 9:
                    message = "مبلغ تراکنش مشکل دارد";
                    break;
                case 10:
                    message = "شماره کارت اشتباه است.";
                    break;
                case 11:
                    message = "ارتباط با درگاه برقرار نشد، مجددا تلاش کنید";
                    break;
                case 12:
                    message = "خطای داخلی بانک رخ داده است";
                    break;
                case 15:
                    message = "این تراکنش قبلا تایید شده است";
                    break;
                case 18:
                    message = "کاربر پذیرنده تایید نشده است";
                    break;
                case 19:
                    message =
                      "هویت پذیرنده کامل نشده است و نمی تواند در مجموع بیشتر از ۵۰ هزار تومان دریافتی داشته باشد";
                    break;
                case 25:
                    message = "سرویس موقتا از دسترس خارج است، لطفا بعدا مجددا تلاش نمایید";
                    break;
                case 26:
                    message = "کد پرداخت پیدا نشد";
                    break;
                case 27:
                    message = "پذیرنده مجاز به تراکنش با این مبلغ نمی باشد";
                    break;
                case 28:
                    message = "لطفا از قطع بودن فیلتر شکن خود مطمئن شوید";
                    break;
                case 29:
                    message = "ارتباط با درگاه برقرار نشد";
                    break;
                case 31:
                    message = "امکان تایید پرداخت قبل از ورود به درگاه بانک وجود ندارد";
                    break;
                case 38:
                    message = "آدرس سایت پذیرنده نا معتبر است";
                    break;
                case 39:
                    message = "پرداخت ناموفق، مبلغ به حساب پرداخت کننده برگشت داده خواهد شد";
                    break;
                case 44:
                    message = "RefId نامعتبر است";
                    break;
                case 46:
                    message = "توکن ساخت پرداخت با توکن تایید پرداخت مغایرت دارد";
                    break;
                case 47:
                    message = "مبلغ تراکنش مغایرت دارد";
                    break;
                case 48:
                    message = "پرداخت از سمت شاپرک تایید نهایی نشده است";
                    break;
                case 49:
                    message = "ترمینال فعال یافت نشد، لطفا مجددا تلاش کنید";
                    break;
            }

            return message;
        }
    }
}

