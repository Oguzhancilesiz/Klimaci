using Klimaci.Core.Paging;
using Klimaci.DTO;
using Klimaci.DTO.OfficeDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IOfficeService : IBaseService<Office, OfficeAddDTO, OfficeUpdateDTO>
    {
        Task<PagedResult<Office>> PagedAsync(int page = 1, int pageSize = 30, string? search = null);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
