using Klimaci.Core.Paging;
using Klimaci.DTO.RoleDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IRoleService
    {
        Task<PagedResult<AppRole>> PagedAsync(int page = 1, int pageSize = 20, string? search = null);
        Task<AppRole?> GetByIdAsync(Guid id);
        Task<(bool ok, string? error)> AddAsync(RoleAddDTO dto);
        Task<(bool ok, string? error)> UpdateAsync(RoleUpdateDTO dto);
        Task<(bool ok, string? error)> DeleteAsync(Guid id);
        Task<List<AppRole>> GetAllAsync();
    }
}
