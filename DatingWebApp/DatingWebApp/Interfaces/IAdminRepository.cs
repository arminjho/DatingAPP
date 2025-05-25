using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalStatsAsync();
        Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();
    }
}
