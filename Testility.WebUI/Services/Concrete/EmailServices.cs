using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Concrete
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {

            var mailMessage = new MailMessage(
            "gkudel@outlook.com",
            message.Destination,
            message.Subject,
            message.Body
            );

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("gkudel@outlook.com", "123edcrfV") ,
                EnableSsl = true
            };            
            client.Send(mailMessage);
            //return client.SendMailAsync(mailMessage);
            return Task.FromResult(0);
        }
    }
}

