using Klimaci.Core.Paging;
using Klimaci.DTO.ServiceItemDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IServiceItemService : IBaseService<ServiceItem, ServiceItemAddDTO, ServiceItemUpdateDTO>
    {
        Task<PagedResult<ServiceItem>> PagedAsync(ListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task<List<GalleryItemDTO>> GetGalleryAsync(Guid serviceId);
        Task GalleryUpdateAsync(GalleryUpdateDTO dto);
        Task TogglePublishAsync(Guid id, bool isPublished);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
