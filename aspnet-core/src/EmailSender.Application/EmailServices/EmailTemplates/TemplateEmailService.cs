using EmailSender.EmailSender.EmailTempalateManagers;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailTemplates
{
    public class TemplateEmailService : EmailSenderAppServiceBase, ITemplateEmailService
    {
        private readonly IEmailTemplateManager _emailtemplate;
        public TemplateEmailService(IEmailTemplateManager emailtemplate)
        {
            _emailtemplate = emailtemplate;
        }

        //show Template
        public async Task<List<EmailTemplateDto>> GetTemplate()
        {
            return await _emailtemplate.GetAllTemplatesAsync();

        }

        public Task CreateTemplate(EmailTemplateDto templateDto)
        {
            return _emailtemplate.CreateTemplateAsync(templateDto);
        }

        public Task DeleteTemplate(int id)
        {
            return _emailtemplate.DeleteTemplateAsync(id);
        }

    }
}
