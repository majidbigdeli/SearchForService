using System;
using System.Collections.Generic;

namespace SearchForApi.Integrations.Payment.ZarinPal
{
    public class ZarinPalRequestTokenDto
    {
        public List<ZarinPalRequestTokenData> data { get; set; }
        public List<ZarinPalError> errors { get; set; }
    }

    public class ZarinPalRequestTokenData
    {
        public int code { get; set; }
        public string message { get; set; }
        public string authority { get; set; }
        public string fee_type { get; set; }
        public int fee { get; set; }
    }
}
