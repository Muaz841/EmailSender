using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailTempalateManagers.EmailDto
{
    public class EmailTemplateDto : Entity<int>
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Bcc { get; set; }
        public bool? IsActive { get; set; }
        public string Token { get; set; }
        public string Cc { get; set; }
    }
}
