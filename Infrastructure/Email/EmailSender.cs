using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string userEmail, string emailSubject, string emailBody)
        {
            try
            {
                string fromEmail = _config["Email:User"];
                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    To = { userEmail },
                    Subject = emailSubject,
                    IsBodyHtml = true,
                    Body = emailBody,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                using SmtpClient smtpClient = new(_config["Email:SmtpServer"]);
                smtpClient.Credentials = new NetworkCredential(fromEmail, _config["Email:Key"]);
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }
            catch
            {
                //ignore it or you can retry .
            }

            //var client = new SendGridClient(config["SendGrid:Key"]);
            //var message = new SendGridMessage
            //{
            //    From = new EmailAddress("admin@easybag.ir", config["SendGrid:User"]),
            //    Subject = emailSubject,
            //    PlainTextContent = msg,
            //    HtmlContent = msg
            //};
            //message.AddTo(new EmailAddress(userEmail));
            //message.SetClickTracking(false, false);
            //await client.SendEmailAsync(message);
        }
    }
}
