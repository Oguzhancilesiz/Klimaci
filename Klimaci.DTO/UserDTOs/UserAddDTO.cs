using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.UserDTOs
{
    public record UserAddDTO
    {
        [Required, MinLength(3)] public string UserName { get; init; } = "";
        [Required, EmailAddress] public string Email { get; init; } = "";
        [Required, MinLength(6)] public string Password { get; init; } = "";
        public List<string> RoleNames { get; init; } = new();
        public bool IsActive { get; init; } = true;
    }

}
