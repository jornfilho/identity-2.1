using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using DevUtils.PrimitivesExtensions;
using DevUtils.WebConfig;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace IdentitySample.Identity.Services
{
    public class EmailService : IIdentityMessageService
    {
        private bool IsEnabled { get; set; }
        private string SenderEmail { get; set; }
        private string SenderName { get; set; }
        private string Subject { get; set; }
        private string User { get; set; }
        private string Password { get; set; }

        public EmailService()
        {
            IsEnabled = WebConfigExtensions.GetFromAppSettings("EmailService-Enabled").TryParseBool();
            if (!IsEnabled)
                return;

            SenderEmail = WebConfigExtensions.GetFromAppSettings("EmailService-SenderEmail");
            SenderName = WebConfigExtensions.GetFromAppSettings("EmailService-SenderName");
            Subject = WebConfigExtensions.GetFromAppSettings("EmailService-Subject");
            User = WebConfigExtensions.GetFromAppSettings("EmailService-LoginUser");
            Password = WebConfigExtensions.GetFromAppSettings("EmailService-LoginPassword");
        }

        public Task SendAsync(IdentityMessage message)
        {
            return !IsEnabled
                ? Task.FromResult(0)
                : SendMail(message);
        }

        /// <summary>
        /// Implementação do SendGrid
        /// </summary>
        private Task ConfigSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress(SenderEmail, Subject);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(User, Password);

            // Criar um transport web para envio de e-mail
            var transportWeb = new Web(credentials);

            // Enviar o e-mail
            var deliverAsync = transportWeb.DeliverAsync(myMessage);
            return deliverAsync;
        }

        /// <summary>
        /// Implementação de e-mail manual
        /// </summary>
        private Task SendMail(IdentityMessage message)
        {
            var smtpHost = WebConfigExtensions.GetFromAppSettings("EmailService-SmtpHost");
            var smtpPort = WebConfigExtensions.GetFromAppSettings("EmailService-SmtpPort").TryParseInt();


            var mailMessage = new MailMessage();
            var text = HttpUtility.HtmlEncode(message.Body);
            mailMessage.To.Add(new MailAddress(message.Destination));
            mailMessage.Subject = Subject;
            mailMessage.From = new MailAddress(SenderEmail, SenderName);
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));
            

            var smtpClient = new SmtpClient
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Port = smtpPort,
                Host = smtpHost,
                Credentials = new NetworkCredential(User, Password)
            };
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return Task.FromResult(0);
        }
    }
}