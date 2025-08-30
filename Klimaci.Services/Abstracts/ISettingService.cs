using Klimaci.Core.Paging;
using Klimaci.DTO.SettingDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface ISettingService
    {
        Task<PagedResult<Setting>> PagedAsync(int page = 1, int pageSize = 30, string? group = null, string? search = null);
        Task<Setting?> GetByIdAsync(Guid id);
        Task<string?> GetValueAsync(string key);
        Task UpsertAsync(SettingUpsertDTO dto);
        Task UpsertManyAsync(IEnumerable<SettingUpsertDTO> items);
        Task UpdateAsync(SettingUpdateDTO dto);
        Task DeleteAsync(Guid id);
    }
}
