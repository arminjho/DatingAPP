namespace DatingWebApp.DTOs
{
    public class PhotoApprovalStatsDto
    {

        public string Username { get; set; } = string.Empty;
        public int ApprovedPhotos { get; set; }
        public int UnapprovedPhotos { get; set; }
    }
}
