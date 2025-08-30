using Klimaci.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.MediaDTOs
{
    public record MediaListItemDTO(Guid Id, string FileUrl, string? AltText, string? Title, MediaType MediaType);
}
