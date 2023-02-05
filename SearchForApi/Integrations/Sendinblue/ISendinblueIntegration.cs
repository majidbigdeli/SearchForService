using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using sib_api_v3_sdk.Model;

namespace SearchForApi.Integrations.Sendinblue
{
    public interface ISendinblueIntegration
    {
        Task<CreateSmtpEmail> SendEmail(string to, SendinBlueEmailTemplates templateId, object tokens = null);
        Task<CreateUpdateContactModel> CreateOrUpdateContact(string email, object attributes, List<SendinBlueContactLists?> listIds = null);
    }
}

