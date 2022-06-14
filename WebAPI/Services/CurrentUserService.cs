using Application.Interfaces;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public String UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "";
            }
        }
    }
}
