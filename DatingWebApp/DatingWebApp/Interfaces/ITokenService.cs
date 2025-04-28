using DatingWebApp.Entities;

namespace DatingWebApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
