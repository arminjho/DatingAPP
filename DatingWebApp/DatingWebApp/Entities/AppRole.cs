using Microsoft.AspNetCore.Identity;

namespace DatingWebApp.Entities
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
