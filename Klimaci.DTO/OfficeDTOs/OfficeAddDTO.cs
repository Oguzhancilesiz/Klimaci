using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.OfficeDTOs
{
    public record OfficeAddDTO(string Title, string? Address, string? City, string? Phone, string? Email,
                                decimal? Latitude, decimal? Longitude, int DisplayOrder, bool IsPublished);
}
