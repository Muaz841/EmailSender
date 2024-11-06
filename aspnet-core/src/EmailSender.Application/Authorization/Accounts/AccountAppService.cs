using System.Threading.Tasks;
using Abp.Configuration;
using Abp.UI;
using Abp.Zero.Configuration;
using EmailSender.Authorization.Accounts.Dto;
using EmailSender.Authorization.Users;
using EmailSender.EmailSender;

namespace EmailSender.Authorization.Accounts
{
    public class AccountAppService : EmailSenderAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            IEmailSenderManager emailSenderManager,
        UserRegistrationManager userRegistrationManager)
        {
            _emailSenderManager = emailSenderManager;
            _userRegistrationManager = userRegistrationManager;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                true // Assumed email address is always confirmed. Change this if you want to implement email confirmation.
            );
            if (user == null || !user.TenantId.HasValue)
            {
                throw new UserFriendlyException("Please select valid tenant!");
            }
            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
                
                await _emailSenderManager.SendEmailAsync(user.UserName, user.EmailAddress, user.TenantId.Value);           
            
            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }
    }
}
