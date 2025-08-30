using Klimaci.Core.Paging;
using Klimaci.DTO;
using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO.FaqDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IFaqService : IBaseService<FaqItem, FaqItemAddDTO, FaqItemUpdateDTO>
    {
        Task<PagedResult<FaqItem>> PagedAsync(int page = 1, int pageSize = 30, string? search = null);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
