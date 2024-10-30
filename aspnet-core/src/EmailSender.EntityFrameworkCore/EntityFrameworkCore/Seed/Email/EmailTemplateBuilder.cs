using Abp.Runtime.Session;
using EmailSender.EmailSender.EmailSenderEntities;
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
        private readonly int _tenantId;
        private readonly IAbpSession _abpSession;
        public EmailTemplateBuilder (EmailSenderDbContext context, int tenantId)
        {
            _context = context; 
            _tenantId = tenantId;
        }
        public async Task Create()
        {
            await TemplateSeeder(); 
        }

        private  async Task TemplateSeeder()
        {
        var check = _context.EmailTemplates.FirstOrDefault(t =>  t.TenantId == _tenantId);
            if (check != null) return;
            var temp = new EmailTemplate
            {
                TenantId = 1,
                Name = "seeding check",
                Subject = "Welcome to Our Service!",
                Content = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Welcome to the Team!</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f7f9fc;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);\r\n            text-align: center;\r\n        }\r\n        h1 {\r\n            color: #4a90e2;\r\n            font-size: 24px;\r\n        }\r\n        p {\r\n            font-size: 16px;\r\n            line-height: 1.6;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n\r\n<div class=\"container\">\r\n    <h1>Welcome, {{username}}!</h1>\r\n    <p>We’re excited to have you on board. You’ll soon receive further instructions at {{useremail}}.</p>\r\n    <p>Welcome to the team!</p>\r\n</div>\r\n\r\n</body>\r\n</html>\r\n",
                Bcc = "bcc@example.com",
                IsActive = true,
                Token = Guid.NewGuid().ToString(),
                Cc = "cc@example.com"
            };
               _context.EmailTemplates.AddAsync(temp);
             await _context.SaveChangesAsync();
        }
    }
}
