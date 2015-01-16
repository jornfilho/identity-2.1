using System.Threading.Tasks;
using DevUtils.PrimitivesExtensions;
using DevUtils.WebConfig;
using Microsoft.AspNet.Identity;
using Twilio;

namespace IdentitySample.Identity.Services
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var isEnabled = WebConfigExtensions.GetFromAppSettings("SmsService-Enabled").TryParseBool();
            if (!isEnabled)
                return Task.FromResult(0);

            // Utilizando TWILIO como SMS Provider.
            // https://www.twilio.com/docs/quickstart/csharp/sms/sending-via-rest

            string accountSid = WebConfigExtensions.GetFromAppSettings("SmsService-AccountSid");
            string authToken = WebConfigExtensions.GetFromAppSettings("SmsService-AuthToken");
            string phoneNumber = WebConfigExtensions.GetFromAppSettings("SmsService-PhoneNumber");

            var client = new TwilioRestClient(accountSid, authToken);

            client.SendMessage(phoneNumber, message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }
}