using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender
{
    public interface IEmailSenderManager
    {

        Task<bool> SendEmailAsync(string username, string useremail, int tenantId);
    }
}
