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
            "xxxx",
            message.Destination,
            message.Subject,
            message.Body
            );

            SmtpClient client = new SmtpClient("xxxxx", 465) { Credentials = new NetworkCredential("xxx", "xxx") };
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.SendMailAsync(mailMessage);
            return Task.FromResult(0);
        }
    }
}

