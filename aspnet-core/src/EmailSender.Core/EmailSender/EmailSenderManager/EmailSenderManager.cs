using Abp.Configuration;
using Abp.Dependency;
using Abp.Net.Mail;
using EmailSender.EmailSender.EmailTempalateManagers;
using EmailSender.EmailSender.QueueEmail;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EmailSender.EmailSender.EmailSender
{
    public class EmailSenderManager : ITransientDependency, IEmailSenderManager
    {
        private readonly IEmailTemplateManager _templateManager;
        private readonly IQueuedEmailManager _queuedEmailManager;
        private readonly IConfiguration _configuration;
        private readonly ISettingManager _settingManager;
        public EmailSenderManager(ISettingManager settingManager,IEmailTemplateManager templateManager, IQueuedEmailManager queuedEmailManager, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._templateManager = templateManager;
            this._queuedEmailManager = queuedEmailManager;
            this._settingManager = settingManager;
        }

        public  async Task<bool> SendEmailAsync( string username, string useremail, int tenantId)
        {

                var emailtemplate = await _templateManager.GetTemplateByIdAsync(tenantId);

            var tokenReplacements = new Dictionary<string, string>
        {
            { "{{username}}", username },
            { "{{useremail}}", useremail }
        };
            // Replace tokens in subject and body
            emailtemplate.Subject = ReplaceTokens(emailtemplate.Subject, tokenReplacements);
            emailtemplate.Content = ReplaceTokens(emailtemplate.Content, tokenReplacements);
            
            var emailQueue = new QueuedEmailDto
            {
                To = useremail,
                ToName = username,
                Subject = emailtemplate.Subject,
                Body = emailtemplate.Content,
                TenantId = tenantId.ToString(),
                Status = "pending",
                From = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, tenantId),
                FromName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromDisplayName, tenantId),
            };
            // Add the email to the queue
            await _queuedEmailManager.AddQueueEmailAsync(emailQueue);
            return true;
        }


        private string ReplaceTokens(string template, Dictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(template))
                return template;

            foreach (var token in tokens)
            {
                template = template.Replace(token.Key, token.Value);
            }

            return template;
        }

   
    }
  

}
