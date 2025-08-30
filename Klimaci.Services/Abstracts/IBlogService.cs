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
    public interface IBlogService : IBaseService<BlogPost, BlogPostAddDTO, BlogPostUpdateDTO>
    {
        Task<PagedResult<BlogPost>> PagedAsync(BlogListQueryDTO q);
        Task<SlugCheckResultDTO> SlugCheckAsync(SlugCheckDTO dto);
        Task<List<GalleryItemDTO>> GetGalleryAsync(Guid postId);
        Task GalleryUpdateAsync(GalleryUpdateDTO dto);
        Task TogglePublishAsync(Guid id, bool isPublished);
        Task ReorderAsync(IEnumerable<ReorderItemDTO> items);
    }
}
