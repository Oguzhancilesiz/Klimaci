using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.SettingDTOs
{
    public record SettingUpsertDTO(string Key, string? Value, string? Group, string? Description);

}
