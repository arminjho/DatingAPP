using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingWebApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class Seed
    {
        public static async Task SeedUsers(Db_Context context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options= new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData,options);
            if(users==null)return;

            foreach(var user in users)
            {
                using var hmac=new HMACSHA512();    
                user.UserName=user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pas$$w0rd"));
                user.PasswordSalt=hmac.Key;
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();   
            
        }
    }
}
