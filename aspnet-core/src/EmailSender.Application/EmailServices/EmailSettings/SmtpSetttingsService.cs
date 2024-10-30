using Abp.Configuration;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using EmailSender.Authorization.Users;
using EmailSender.EmailServices.EmailSettings.EmailSettingsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailSettings
{
    public class SmtpSettingsService : EmailSenderAppServiceBase
    {
        private readonly ISettingManager _settingManager;
        public SmtpSettingsService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<SmtpSettingsDto> GetSmtpSettingsAsync(int? tenantid)
        {
            var Id = tenantid.HasValue && tenantid.Value != 0 ? tenantid.Value :  MultiTenancyConsts.DefaultTenantId;
          
            var settings = new SmtpSettingsDto
            {
                Host = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, Id),
                Port = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Port, Id),
                UserName = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, Id),
                Password = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, Id),
                Domain = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Domain, Id),
                EnableSsl = await _settingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.EnableSsl, Id),
                SenderEmail = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, Id),
            };
            return settings;
        }
        public async Task UpdateSmtpSettingsAsync(SmtpSettingsDto input)
        {
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, input.Host);
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, input.Port);
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, input.UserName);
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, input.Password);
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, input.Domain);
            await _settingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, input.EnableSsl.ToString());
            
        }
    }

}

