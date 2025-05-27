using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class PhotoRepository(Db_Context context):IPhotoRepository
    {
        public async Task<Photo?> GetPhotoById(int id)
        {
            return await context.Photos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PhotoWithTagsDto>> GetUnapprovedPhotos()
        {
            return await context.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .Include(p=>p.PhotoTags)
                .ThenInclude(pt=>pt.Tag)
                .Select(u => new PhotoWithTagsDto
                {
                    Id = u.Id,
                    Url = u.Url,
                    IsApproved = u.IsApproved,
                    Tags=u.PhotoTags.Select(pt=> new TagDto
                    {
                        Id=pt.TagId,
                        Name=pt.Tag.Name
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<IEnumerable<Photo>> GetPhotosByTagsAsync(List<string> tagNames)

        {

            return await context.Photos

                .Where(p => p.IsApproved)
                .Where(p => p.PhotoTags.Any(pt => tagNames.Contains(pt.Tag.Name)))
                .Include(p => p.PhotoTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();

        }



        public async Task<IEnumerable<Photo>> GetUnapprovedPhotosByTagsAsync(List<string> tagNames)

        {

            return await context.Photos

                .IgnoreQueryFilters()
                .Where(p => !p.IsApproved)
                .Where(p => p.PhotoTags.Any(pt => tagNames.Contains(pt.Tag.Name)))
                .Include(p => p.PhotoTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            context.Photos.Remove(photo);
        }

    }
}
