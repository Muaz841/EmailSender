using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using EmailSender.Authorization.Roles;
using EmailSender.Authorization.Users;
using EmailSender.MultiTenancy;
using EmailSender.EmailSender.EmailSenderEntities;
using System;


namespace EmailSender.EntityFrameworkCore
{
    public class EmailSenderDbContext : AbpZeroDbContext<Tenant, Role, User, EmailSenderDbContext>
    {
        public EmailSenderDbContext(DbContextOptions<EmailSenderDbContext> options)
            : base(options)
        {
        }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }
        
    }
}
