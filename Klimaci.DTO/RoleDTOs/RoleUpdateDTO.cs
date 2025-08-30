using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.RoleDTOs
{
    public record RoleUpdateDTO
    {
        [Required] public Guid Id { get; init; }
        [Required, MinLength(2)] public string Name { get; init; } = "";
    }
}
