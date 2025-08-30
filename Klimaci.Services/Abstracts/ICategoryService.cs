using Klimaci.Core.Paging;
using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface ICategoryService : IBaseService<Category, CategoryAddDTO, CategoryUpdateDTO>
    {
        Task<PagedResult<Category>> PagedAsync(ListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
