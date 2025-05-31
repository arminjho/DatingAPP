using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    
   public interface IAdminService
    {
        Task<List<UserWithRolesDto>> GetUsersWithRolesAsync();
        Task<IList<string>> EditUserRolesAsync(string username, string[] roles);
        Task<object> GetPhotoApprovalStatisticsByUser();
        Task<IEnumerable<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();


    }


}
