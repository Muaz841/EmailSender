using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace EmailSender.EmailSender.EmailTempalateManagers
{
    public class EmailTemplateManager : ITransientDependency, IEmailTemplateManager
    {
        private readonly IRepository<EmailTemplate, int> templateRepository;
        private readonly IObjectMapper objectMapper;

        public EmailTemplateManager(IRepository<EmailTemplate, int> emailTemplateRepository, IObjectMapper objectMapper)
        {
            templateRepository = emailTemplateRepository;
            this.objectMapper = objectMapper;
        }

        public async Task<EmailTemplateDto> CreateTemplateAsync(EmailTemplateDto templateDto)
        {
            var template = objectMapper.Map<EmailTemplate>(templateDto);
            await templateRepository.InsertAsync(template);
            var savedTemplateDto = objectMapper.Map<EmailTemplateDto>(template);
            return savedTemplateDto;

        }
        

        public Task DeleteTemplateAsync(int id)
        {
            var del = templateRepository.GetAsync(id);
            templateRepository.DeleteAsync(del.Id);
            return Task.CompletedTask;
        }

        public async Task<List<EmailTemplateDto>> GetAllTemplatesAsync()
        {
            var templates =  await templateRepository.GetAllListAsync();            
            return  objectMapper.Map<List<EmailTemplateDto>>(templates);                    
        }

        public async Task<EmailTemplateDto> GetTemplateByIdAsync(int tenantId)
        {

            var template = await templateRepository.FirstOrDefaultAsync(t => t.TenantId == tenantId);
            return objectMapper.Map<EmailTemplateDto>(template);
        }

        public async Task UpdateTemplateAsync(EmailTemplateDto updatetemplate)
        {
            var template = await templateRepository.GetAsync(updatetemplate.Id);
            objectMapper.Map(updatetemplate, template);
        }
    }
}
