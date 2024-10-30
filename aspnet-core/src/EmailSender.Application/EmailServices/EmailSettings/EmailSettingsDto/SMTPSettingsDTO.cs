using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailSettings.EmailSettingsDto
{
    public class SmtpSettingsDto
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public bool EnableSsl { get; set; }

        public string SenderEmail { get; set; }
    }

}
