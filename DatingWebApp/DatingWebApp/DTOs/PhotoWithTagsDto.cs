namespace DatingWebApp.DTOs
{
    public class PhotoWithTagsDto:PhotoDto
    {
        public List<TagDto> Tags { get; set; } = [];
    }
}
