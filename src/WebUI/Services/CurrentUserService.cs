
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Zenbot.Domain.Interfaces;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.WebUI.Services
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
