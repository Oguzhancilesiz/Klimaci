using Klimaci.Core.Paging;
using Klimaci.DTO.UserDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _users;
        private readonly RoleManager<AppRole> _roles;

        public UserService(UserManager<AppUser> users, RoleManager<AppRole> roles)
        {
            _users = users; _roles = roles;
        }

        public async Task<PagedResult<AppUser>> PagedAsync(int page = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool desc = false)
        {
            var q = _users.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(x => x.UserName!.Contains(search) || x.Email!.Contains(search));

            q = (sortBy?.ToLowerInvariant()) switch
            {
                "createddate" => desc ? q.OrderByDescending(x => x.CreatedDate) : q.OrderBy(x => x.CreatedDate),
                _ => desc ? q.OrderByDescending(x => x.UserName) : q.OrderBy(x => x.UserName)
            };

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, total, page, pageSize);
        }

        public Task<AppUser?> GetByIdAsync(Guid id) => _users.Users.FirstOrDefaultAsync(x => x.Id == id)!;

        public async Task<(bool ok, string? error)> AddAsync(UserAddDTO dto)
        {
            var u = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = true,
                Status = dto.IsActive ? Core.Enums.Status.Active : Core.Enums.Status.DeActive,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var r1 = await _users.CreateAsync(u, dto.Password);
            if (!r1.Succeeded) return (false, string.Join("; ", r1.Errors.Select(e => e.Description)));

            foreach (var rn in dto.RoleNames.Distinct())
            {
                if (await _roles.RoleExistsAsync(rn))
                    await _users.AddToRoleAsync(u, rn);
            }
            if (!dto.IsActive) await _users.SetLockoutEndDateAsync(u, DateTimeOffset.MaxValue);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> UpdateAsync(UserUpdateDTO dto)
        {
            var u = await _users.FindByIdAsync(dto.Id.ToString());
            if (u is null) return (false, "Kullanıcı bulunamadı.");

            u.UserName = dto.UserName;
            u.Email = dto.Email;
            u.Status = dto.IsActive ? Core.Enums.Status.Active : Core.Enums.Status.DeActive;
            u.ModifiedDate = DateTime.UtcNow;

            var r = await _users.UpdateAsync(u);
            if (!r.Succeeded) return (false, string.Join("; ", r.Errors.Select(e => e.Description)));

            await ToggleLockAsync(dto.Id, dto.IsActive);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var u = await _users.FindByIdAsync(dto.Id.ToString()); if (u is null) return (false, "Kullanıcı yok.");
            var token = await _users.GeneratePasswordResetTokenAsync(u);
            var r = await _users.ResetPasswordAsync(u, token, dto.NewPassword);
            return r.Succeeded ? (true, null) : (false, string.Join("; ", r.Errors.Select(e => e.Description)));
        }

        public async Task<(bool ok, string? error)> ToggleLockAsync(Guid id, bool isActive)
        {
            var u = await _users.FindByIdAsync(id.ToString()); if (u is null) return (false, "Kullanıcı yok.");
            await _users.SetLockoutEndDateAsync(u, isActive ? null : DateTimeOffset.MaxValue);
            return (true, null);
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            var u = await _users.FindByIdAsync(userId.ToString()); if (u is null) return new();
            var roles = await _users.GetRolesAsync(u);
            return roles.ToList();
        }

        public async Task<(bool ok, string? error)> UpdateUserRolesAsync(AssignRolesDTO dto)
        {
            var u = await _users.FindByIdAsync(dto.UserId.ToString()); if (u is null) return (false, "Kullanıcı yok.");
            var allRoles = await _roles.Roles.Select(x => x.Name!).ToListAsync();
            var current = await _users.GetRolesAsync(u);

            var wanted = dto.RoleNames.Distinct().Where(r => allRoles.Contains(r)).ToList();
            var toAdd = wanted.Except(current).ToList();
            var toRem = current.Except(wanted).ToList();

            var r1 = await _users.AddToRolesAsync(u, toAdd);
            var r2 = await _users.RemoveFromRolesAsync(u, toRem);
            if (!r1.Succeeded || !r2.Succeeded)
            {
                var errs = r1.Errors.Concat(r2.Errors).Select(e => e.Description);
                return (false, string.Join("; ", errs));
            }
            return (true, null);
        }
    }
}
