using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record IdDTO(Guid Id);
    public record IdsDTO(List<Guid> Ids);

    // Drag&drop sıralama için toplu güncelleme
    public record ReorderItemDTO(Guid Id, int DisplayOrder);
    public record ReorderRequestDTO(List<ReorderItemDTO> Items);

    // Yayın durumu / Status değişimi (SlugEntity ve BaseEntity için ortak)
    public record TogglePublishDTO(Guid Id, bool IsPublished);
    public record ChangeStatusDTO(Guid Id, int Status); // Core.Enums.Status int değeri

    // Basit listeleme sorgusu (grid filtreleri)
    public record ListQueryDTO(
        int Page = 1,
        int PageSize = 12,
        string? Search = null,
        string? SortBy = null,       // "Title","CreatedDate","DisplayOrder" vs.
        bool SortDesc = false,
        bool? OnlyPublished = null,  // true/false/null
        Guid? BrandId = null,        // projelerde işe yarar
        Guid? CategoryId = null      // blog için opsiyonel
    );

    // CRUD dönüşünde pratik sonuç
    public record CommandResultDTO(Guid Id, string? Slug = null);
}
