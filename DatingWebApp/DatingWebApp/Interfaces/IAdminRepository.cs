using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalStatisticsByUser();
        Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();
    }
}
