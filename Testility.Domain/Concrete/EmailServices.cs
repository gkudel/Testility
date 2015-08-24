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
            "lukasz_oz@interia.pl",
            message.Destination,
            message.Subject,
            message.Body
            );

            SmtpClient client = new SmtpClient("poczta.interia.pl", 587) { Credentials = new NetworkCredential("lukasz_oz@interia.pl", "q1w2e3r4t5y6") };
            client.SendAsync(mailMessage, null);

            return client.SendMailAsync(mailMessage);
        }
    }
}

