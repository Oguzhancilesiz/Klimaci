using Klimaci.Core.Paging;
using Klimaci.DTO.ProjectDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IBrandService : IBaseService<Brand, BrandAddDTO, BrandUpdateDTO>
    {
        Task<PagedResult<Brand>> PagedAsync(ListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
