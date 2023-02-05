using System;
using System.Collections.Generic;

namespace SearchForApi.Integrations.Payment.ZarinPal
{
    public class ZarinPalVerifyPaymentDto
    {
        public List<ZarinPalVerifyPaymentData> data { get; set; }
        public List<ZarinPalError> errors { get; set; }
    }

    public class ZarinPalVerifyPaymentData
    {
        public int code { get; set; }
        public string message { get; set; }
        public string card_hash { get; set; }
        public string card_pan { get; set; }
        public string ref_id { get; set; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }
}
