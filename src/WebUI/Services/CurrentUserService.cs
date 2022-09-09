
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ZenAchitecture.Domain.Interfaces;
using ZenAchitecture.Domain.Shared.Interfaces;

namespace ZenAchitecture.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);


    }
}
