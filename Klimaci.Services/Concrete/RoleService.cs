using Klimaci.Core.Paging;
using Klimaci.DTO.RoleDTOs;
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
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roles;
        public RoleService(RoleManager<AppRole> roles) { _roles = roles; }

        public async Task<PagedResult<AppRole>> PagedAsync(int page = 1, int pageSize = 20, string? search = null)
        {
            var q = _roles.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(x => x.Name!.Contains(search));
            var total = await q.CountAsync();
            var items = await q.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, total, page, pageSize);
        }

        public Task<AppRole?> GetByIdAsync(Guid id) => _roles.Roles.FirstOrDefaultAsync(x => x.Id == id)!;

        public async Task<(bool ok, string? error)> AddAsync(RoleAddDTO dto)
        {
            var r = await _roles.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpperInvariant(),
                Status = Core.Enums.Status.Active,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            });
            return r.Succeeded ? (true, null) : (false, string.Join("; ", r.Errors.Select(e => e.Description)));
        }

        public async Task<(bool ok, string? error)> UpdateAsync(RoleUpdateDTO dto)
        {
            var role = await _roles.FindByIdAsync(dto.Id.ToString()); if (role is null) return (false, "Rol yok.");
            role.Name = dto.Name; role.NormalizedName = dto.Name.ToUpperInvariant(); role.ModifiedDate = DateTime.UtcNow;
            var r = await _roles.UpdateAsync(role);
            return r.Succeeded ? (true, null) : (false, string.Join("; ", r.Errors.Select(e => e.Description)));
        }

        public async Task<(bool ok, string? error)> DeleteAsync(Guid id)
        {
            var role = await _roles.FindByIdAsync(id.ToString()); if (role is null) return (false, "Rol yok.");
            var r = await _roles.DeleteAsync(role);
            return r.Succeeded ? (true, null) : (false, string.Join("; ", r.Errors.Select(e => e.Description)));
        }

        public Task<List<AppRole>> GetAllAsync() => _roles.Roles.OrderBy(x => x.Name).ToListAsync();
    }
}
