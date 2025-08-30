using Klimaci.Core.Paging;
using Klimaci.DTO.UserDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IUserService
    {
        Task<PagedResult<AppUser>> PagedAsync(int page = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool desc = false);
        Task<AppUser?> GetByIdAsync(Guid id);
        Task<(bool ok, string? error)> AddAsync(UserAddDTO dto);
        Task<(bool ok, string? error)> UpdateAsync(UserUpdateDTO dto);
        Task<(bool ok, string? error)> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<(bool ok, string? error)> ToggleLockAsync(Guid id, bool isActive);
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<(bool ok, string? error)> UpdateUserRolesAsync(AssignRolesDTO dto);
    }
}
