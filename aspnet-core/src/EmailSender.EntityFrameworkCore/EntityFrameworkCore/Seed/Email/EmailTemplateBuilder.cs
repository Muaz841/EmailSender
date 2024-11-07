using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EntityFrameworkCore.Seed.Email
{
    public class EmailTemplateBuilder
    {
        private readonly EmailSenderDbContext _context;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EmailTemplateBuilder(EmailSenderDbContext context, IAbpSession abpSession, IRepository<Tenant, int> tenantRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _context = context;
            _abpSession = abpSession;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Create()
        {
            // Start the unit of work here
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var allTenants = await _tenantRepository.GetAllListAsync();

                foreach (var tenant in allTenants)
                {
                    int tenantId = tenant.Id;

                    // Check if the template exists for the tenant
                    var check = _context.EmailTemplates.FirstOrDefault(t => t.TenantId == tenantId);
                    if (check != null) continue; // If the template already exists for this tenant, skip to the next tenant

                    // If no template exists, create a new one
                    var template = new EmailTemplate
                    {
                        TenantId = tenantId, // Associate with the current tenant
                        Name = "seeding check",
                        Subject = "Welcome to Our Service!",
                        Content = "<!DOCTYPE html>\r\n  <html>\r\n  <body>\r\n    " +
                        "     <img src=\"https://i.postimg.cc/7L5LJqGz/head.png\" alt=\"head\" width=\"100%\" height=\"50%\">\r\n  \r\n " +
                        "     <p style=\"display:flex; justify-content:center;\">\r\n          <strong>Dear Hassan Majid,</strong>\r\n " +
                        "     </p>\r\n  \r\n      <p style=\"display:flex; justify-content:center;\">\r\n        " +
                        "  We are thrilled to welcome you to Solvefy! Your registration has been successfully completed.\r\n  " +
                        "    </p>\r\n  \r\n      <p style=\"display:flex; justify-content:center;\">\r\n       " +
                        "   You can now log in to your account using the following details:\r\n      </p>\r\n  \r\n   " +
                        "   <!-- Form-like input fields (though they won't function in most email clients) -->\r\n    " +
                        "  <div style=\"display:flex; flex-direction:row; justify-content:center ;\">\r\n\r\n      " +
                        "  <div  class=\"col-md-3\" style=\"display:flex; flex-direction:column; justify-content:center ;\">\r\n\r\n   " +
                        "       <label>Username</label>      \r\n      " +
                        "    <input  type=\"text\" value=\"{{username}}\" style=\"padding: 5px; margin-right: 10px;\" disabled>\r\n      " +
                        "  </div>\r\n\r\n      " +
                        "  <div class=\"col-md-3\" style=\"display:flex; flex-direction:column; justify-content:center ;\">     " +
                        "      \r\n          <label>Email</label> \r\n          <input  type=\"email\" value=\"{{useremail}}\" style=\"padding: 5px;\" disabled>\r\n    " +
                        "    </div>\r\n       \r\n      </div>\r\n  \r\n      <!-- Replace with the direct image URL for the footer image -->\r\n  " +
                        "    <img src=\"https://i.postimg.cc/vmCZWCfN/foot.png\" alt=\"foot\" width=\"100%\" height=\"50%\">\r\n  \r\n  </body>\r\n  </html>\r\n  \r\n\r\n",
                        Bcc = "bcc@example.com",
                        IsActive = true,
                        Token = "{{username}}, {{useremail}}",
                        Cc = "cc@example.com"
                    };

                    // Add the template for this tenant
                    await _context.EmailTemplates.AddAsync(template);
                }

                // Save changes after processing all tenants
                await _context.SaveChangesAsync();

                // Commit the unit of work
                await unitOfWork.CompleteAsync();
            }
        }
    }
}
