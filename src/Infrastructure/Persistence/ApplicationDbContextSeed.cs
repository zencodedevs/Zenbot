using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities;
using Zenbot.Infrastructure.Shared.Persistence;

namespace Zenbot.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        public static async Task DeleteDuplicateData(ApplicationDbContext dbContext)
        {
            var duplicatebotUserGuild = (dbContext.BotUserGuilds.ToList()).GroupBy(s => new { s.GuildId, s.BotUserId }).SelectMany(grp => grp.Skip(1)); ;
            dbContext.BotUserGuilds.RemoveRange(duplicatebotUserGuild);
            dbContext.SaveChanges();
        }
    }
}
