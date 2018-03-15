using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lands.API.Helpers
{
    public class MailHelper
    {
        public static async Task SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(WebApiApplication.AdminUser);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebApiApplication.AdminUser,
                    Password = WebApiApplication.AdminPassWord
                };

                smtp.Credentials = credential;
                smtp.Host = WebApiApplication.SMTPName;
                smtp.Port = int.Parse(WebApiApplication.SMTPPort);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        public static async Task SendMail(List<string> mails, string subject, string body)
        {
            var message = new MailMessage();

            foreach (var to in mails)
            {
                message.To.Add(new MailAddress(to));
            }

            message.From = new MailAddress(WebApiApplication.AdminUser);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebApiApplication.AdminUser,
                    Password = WebApiApplication.AdminPassWord
                };

                smtp.Credentials = credential;
                smtp.Host = WebApiApplication.SMTPName;
                smtp.Port = int.Parse(WebApiApplication.SMTPPort);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}