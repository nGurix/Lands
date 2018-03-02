using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Lands.Backend.Helpers
{
    public class MailHelper
    {
        public static async Task SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(MvcApplication.AdminUser);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = MvcApplication.AdminUser,
                    Password = MvcApplication.AdminPassWord
                };

                smtp.Credentials = credential;
                smtp.Host = MvcApplication.SMTPName;
                smtp.Port = int.Parse(MvcApplication.SMTPPort);
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

            message.From = new MailAddress(MvcApplication.AdminUser);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = MvcApplication.AdminUser,
                    Password = MvcApplication.AdminPassWord
                };

                smtp.Credentials = credential;
                smtp.Host = MvcApplication.SMTPName;
                smtp.Port = int.Parse(MvcApplication.SMTPPort);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}