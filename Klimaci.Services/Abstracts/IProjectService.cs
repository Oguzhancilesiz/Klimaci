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
    public interface IProjectService : IBaseService<Project, ProjectAddDTO, ProjectUpdateDTO>
    {
        Task<PagedResult<Project>> PagedAsync(ListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task GalleryUpdateAsync(GalleryUpdateDTO dto);
        Task TogglePublishAsync(Guid id, bool isPublished);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);

        Task<List<GalleryItemDTO>> GetGalleryAsync(Guid projectId);
    }
}
