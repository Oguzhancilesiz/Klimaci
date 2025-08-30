using Klimaci.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.MediaDTOs
{
    public record MediaAddDTO(
        string FileUrl,
        MediaType MediaType = MediaType.Image,
        string? AltText = null,
        string? Title = null,
        long? FileSizeBytes = null,
        int? Width = null,
        int? Height = null,
        int? DurationSec = null
    );
}
