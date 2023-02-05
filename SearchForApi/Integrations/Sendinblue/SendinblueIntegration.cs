using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;
using Newtonsoft.Json.Linq;
using SearchForApi.Core;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace SearchForApi.Integrations.Sendinblue
{
    public class SendinblueIntegration : ISendinblueIntegration
    {
        private readonly TransactionalEmailsApi _emailApi;
        private readonly ContactsApi _contactsApi;

        public SendinblueIntegration()
        {
            Configuration.Default.AddApiKey("api-key", Cfg.SendinBlueApiKey);
            _emailApi = new TransactionalEmailsApi();
            _contactsApi = new ContactsApi();
        }

        [Time("to={to},templateId={templateId}")]
        public async Task<CreateSmtpEmail> SendEmail(string to, SendinBlueEmailTemplates templateId, object tokens = null)
        {
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(
                    to: new List<SendSmtpEmailTo>() { new SendSmtpEmailTo(to) }, templateId: (int)templateId, _params: tokens, sender: new SendSmtpEmailSender(Cfg.SendinBlueSenderName, Cfg.SendinBlueSenderEmail));

                return await _emailApi.SendTransacEmailAsync(sendSmtpEmail);
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", "Sendinblue", "SMTP");
                return null;
            }
        }

        [Time("email={email}")]
        public async Task<CreateUpdateContactModel> CreateOrUpdateContact(string email, object attributes, List<SendinBlueContactLists?> listIds = null)
        {
            try
            {
                return await _contactsApi.CreateContactAsync(new CreateContact(email, JObject.FromObject(attributes), listIds: listIds?.Select(p => (long?)p).ToList(), updateEnabled: true));
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Third-Party Exception: {integration}/{api}", "Sendinblue", "Contact");
                return null;
            }
        }
    }
}

