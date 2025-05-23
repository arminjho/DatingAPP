using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class TagRepository(Db_Context context) : ITagRepository
    {
        public async Task<Tag> GetOrCreateTagAsync(string tagName)

        {

            var tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);

            if (tag != null) return tag;



            tag = new Tag { Name = tagName };

            context.Tags.Add(tag);

            await context.SaveChangesAsync();

            return tag;
        }
    }
}
