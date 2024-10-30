using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EmailSender.EmailSender.QueueEmail;
using System.Net.Mail;
using System.Net;
using Abp.Threading.Timers;
using Abp.Domain.Uow;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.MultiTenancy;
using Abp.Net.Mail;


namespace EmailSender.EmailSender.EmailWorker
{

    public class EmailSendingWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {        
        private readonly IQueuedEmailManager _emailQueueRepository;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSendingWorker> logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;        

        public EmailSendingWorker(
            IAbpSession AbpSession,
            IUnitOfWorkManager unitOfWorkManager,
            AbpTimer timer,
                 IQueuedEmailManager emailQueueRepository,
                 IEmailSenderManager emailSenderManager,
                 IConfiguration configuration,
                 ISettingManager settingManager,
        ILogger<EmailSendingWorker> logger) : base(timer)
        {
            _abpSession = AbpSession;
            _settingManager = settingManager;
            _emailQueueRepository = emailQueueRepository;
            _configuration = configuration;
            _emailSenderManager = emailSenderManager;
            logger = this.logger;
            Timer.Period = (20 * 1000); 
            _unitOfWorkManager = unitOfWorkManager;
        }
        protected override async void DoWork()
        {
            int? tenantid = _abpSession.TenantId;
            var Id = tenantid.HasValue && tenantid.Value != 0 ? tenantid.Value : MultiTenancyConsts.DefaultTenantId;

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var emails = await _emailQueueRepository.GetPendingEmailsAsync();
                if (emails == null) {return;}

                foreach (var email in emails)
                {
                    try
                    {
                        //Fetching-data
                        var smtpHost = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, Id);
                        var smtpPort = int.Parse(await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Port, Id));
                        var smtpUserName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, Id);
                        var smtpPassword = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, Id);
                        var smtpSenderEmail = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, Id);
                        var smtpEnableSsl = await _settingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.EnableSsl, Id);

                        using (var client = new SmtpClient(smtpHost, smtpPort)
                        {
                            Credentials = new NetworkCredential(smtpUserName, smtpPassword),
                            EnableSsl = smtpEnableSsl 
                        })                     
                        {
                            // Prepare the MailMessage
                            var mailMessage = new MailMessage
                            {
                                From = new MailAddress(smtpSenderEmail),
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
