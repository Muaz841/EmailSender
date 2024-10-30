using Abp.Application.Services;
using EmailSender.MultiTenancy.Dto;

namespace EmailSender.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

