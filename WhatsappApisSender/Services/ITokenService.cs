using WhatsappApisSender.Models;

namespace WhatsappApisSender.Services
{
    public interface ITokenService
    {
        public string GenerateToken(AppUser user, IList<string> roles);
        public string GenerateRefreshToken();
    }
}
