using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EmailSender.EmailSender.QueueEmail;
using System.Net.Mail;
using System.Net;
using Abp.Threading.Timers;
using Abp.Domain.Uow;
using Abp.Configuration;
using Abp.Net.Mail;
using EmailSender.MultiTenancy;
using Abp.Domain.Repositories;
using static EmailSender.Authorization.Roles.StaticRoleNames;
using static EmailSender.EmailSender.EmailSenderManager.SmtpDto.SmtpSettingsMethod;


namespace EmailSender.EmailSender.EmailWorker
{
    public class EmailSendingWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        
        private readonly IQueuedEmailManager _emailQueueRepository;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly ILogger<EmailSendingWorker> logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<Tenant, int> _tenantRepository;

        public EmailSendingWorker(

            IUnitOfWorkManager unitOfWorkManager,
              AbpTimer timer,
                 IQueuedEmailManager emailQueueRepository,
                 IEmailSenderManager emailSenderManager,
                 IConfiguration configuration,
                 ISettingManager settingManager,
                 ILogger<EmailSendingWorker> logger,
                  IRepository<Tenant, int> tenantRepository) : base(timer)
        {

            _settingManager = settingManager;
            _emailQueueRepository = emailQueueRepository;           
            _emailSenderManager = emailSenderManager;
            logger = this.logger;
            Timer.Period = (10 * 1000);
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;
        }
        protected override async void DoWork()  
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var allTenant = await _tenantRepository.GetAllAsync();
                foreach (var tenant in allTenant)
                {
                    int TenantId = tenant.Id;
                    //Fetching-SMTP SETTINGS
                    var smtpSettings = await SmtpSettingsHelper.GetSmtpSettingsAsync(_settingManager, TenantId);

                    var emails = await _emailQueueRepository.GetPendingEmailsTWAsync(TenantId);
                    if (emails == null) { return; }

                    foreach (var email in emails)
                    {
                        try
                        {
                            using (var client = new SmtpClient(smtpSettings.Host, Int32.Parse(smtpSettings.Port))
                            {
                                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                                EnableSsl = smtpSettings.EnableSsl
                            })
                            {
                                // Prepare the MailMessage
                                var mailMessage = new MailMessage
                                {
                                    From = new MailAddress(smtpSettings.SenderEmail),
                                    Subject = email.Subject,
                                    Body = email.Body,
                                    IsBodyHtml = true
                                };
                                mailMessage.To.Add(email.To);

                                // Send the email
                                await client.SendMailAsync(mailMessage);
                                await _emailQueueRepository.UpdateEmailStatusAsync(email.Id, "sent");
                            }
                        }
                        catch (Exception ex)
                        {
                            await unitOfWork.CompleteAsync();
                            _emailQueueRepository.IncrementRetryCountAsync(email.Id);
                        }
                    }

                }
            }

        }
    }
}
