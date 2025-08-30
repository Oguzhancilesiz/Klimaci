using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.UserDTOs
{

    public record AssignRolesDTO
    {
        [Required] public Guid UserId { get; init; }
        public List<string> RoleNames { get; init; } = new();
    }
}
