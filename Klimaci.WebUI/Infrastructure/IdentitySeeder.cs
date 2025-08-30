using Klimaci.Entity;
using Klimaci.WebUI.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Klimaci.WebUI.Infrastructure
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            var roleMgr = sp.GetRequiredService<RoleManager<AppRole>>();
            var userMgr = sp.GetRequiredService<UserManager<AppUser>>();
            var admin = sp.GetRequiredService<IOptions<Options.AdminOptions>>().Value;

            if (!await roleMgr.RoleExistsAsync(admin.Role))
                await roleMgr.CreateAsync(new AppRole { Name = admin.Role });

            var user = await userMgr.FindByEmailAsync(admin.Email);

            if (user is null)
            {
                user = new AppUser
                {
                    UserName = admin.UserName,
                    Email = admin.Email,
                    EmailConfirmed = true,
                    Status = Klimaci.Core.Enums.Status.Active
                };

                var res = await userMgr.CreateAsync(user, admin.Password);
                if (res.Succeeded)
                    await userMgr.AddToRoleAsync(user, admin.Role);
            }
            else if (admin.ResetPasswordOnStartup)
            {
                var token = await userMgr.GeneratePasswordResetTokenAsync(user);
                await userMgr.ResetPasswordAsync(user, token, admin.Password);
            }

            if (!await userMgr.IsInRoleAsync(user!, admin.Role))
                await userMgr.AddToRoleAsync(user!, admin.Role);
        }
    }
}
