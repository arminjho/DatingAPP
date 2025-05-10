using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class UserRepository(Db_Context context, IMapper mapper) : IUserRepository
    {
        public async Task<MemberDto?> GetMemberAsync(string username)
        {
            var user= await context.Users
                 .Where(x => x.UserName == username)
                 .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();

            if (user == null) throw new Exception("User je NULL");
            var memberDto=mapper.Map<MemberDto>(user);
            Console.WriteLine("MemberDto:", memberDto);
            return memberDto;

        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();
            query=query.Where(x=>x.UserName != userParams.CurrentUsername);

            if(userParams.Gender != null)
            {
                query=query.Where(x=>x.Gender == userParams.Gender);    
            }

              var minDob=DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge-1));  
              var maxDob=DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            query=query.Where(x=>x.DateOfBirth>=minDob && x.DateOfBirth<=maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), 
                userParams.PageNumber,userParams.PageSize);
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);   
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x=> x.UserName==username);

        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(x=>x.Photos)
                .ToListAsync();

        }

    
        

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
