using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailTempalateManagers
{
    public interface IEmailTemplateManager
    {
        Task<EmailTemplateDto> CreateTemplateAsync(EmailTemplateDto template);
        Task<EmailTemplateDto> GetTemplateByIdAsync(int id);
        Task<List<EmailTemplateDto>> GetAllTemplatesAsync();
        Task UpdateTemplateAsync(EmailTemplateDto updatetemplate);
        Task DeleteTemplateAsync(int id);
    }
}
