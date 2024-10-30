using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;


namespace EmailSender.EntityFrameworkCore.Seed.Host
{
    public class DefaultSettingsCreator
    {
        private readonly EmailSenderDbContext _context;

        public DefaultSettingsCreator(EmailSenderDbContext context)
        {
            _context = context;
        }

        public void Create()
        {  
            int? tenantId = null;

            if (EmailSenderConsts.MultiTenancyEnabled == true)
            {
                tenantId = MultiTenancyConsts.DefaultTenantId;
            }

            // Emailing +     //smtp
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "solvefy@mydomain.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "solvefy", tenantId);        
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "sandbox.smtp.mailtrap.io", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "2525", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "90e3ee095f3aca", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "f5ae4075ab1cf9", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "Solvefydomain.com", tenantId);
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false", tenantId);
            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
