using DatingWebApp.Entities;

namespace DatingWebApp.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
