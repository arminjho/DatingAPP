using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    
   public interface IAdminService
    {
        Task<List<UserWithRolesDto>> GetUsersWithRolesAsync();
        Task<IList<string>> EditUserRolesAsync(string username, string[] roles);
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> AddTagAsync(TagDto tagDto);
        Task DeleteTagAsync(int id);
        Task<object> GetPhotoApprovalStatsAsync();
        Task<IEnumerable<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();





    }


}
