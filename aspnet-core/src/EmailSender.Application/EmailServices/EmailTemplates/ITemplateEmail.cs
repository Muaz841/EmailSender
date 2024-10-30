using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailTemplates
{
    public interface ITemplateEmailService
    {

        Task<List<EmailTemplateDto>> GetTemplate();

        Task CreateTemplate(EmailTemplateDto templateDto);

        Task DeleteTemplate(int id);
    }
}
