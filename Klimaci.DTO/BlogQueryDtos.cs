using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record BlogListQueryDTO(
    int Page = 1,
    int PageSize = 12,
    string? Search = null,
    Guid? CategoryId = null,
    Guid? TagId = null,
    bool? OnlyPublished = true,
    DateTime? From = null,
    DateTime? To = null,
    string? SortBy = "PublishDate",
    bool SortDesc = true
);
}
