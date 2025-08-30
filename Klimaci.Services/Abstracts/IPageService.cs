using Klimaci.Core.Paging;
using Klimaci.DTO.PageDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IPageService : IBaseService<Page, PageAddDTO, PageUpdateDTO>
    {
        Task<PagedResult<Page>> PagedAsync(ListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task<List<GalleryItemDTO>> GetGalleryAsync(Guid pageId);
        Task GalleryUpdateAsync(GalleryUpdateDTO dto);
        Task TogglePublishAsync(Guid id, bool isPublished);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
