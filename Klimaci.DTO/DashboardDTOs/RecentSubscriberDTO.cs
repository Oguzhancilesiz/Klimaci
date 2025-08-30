using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.DashboardDTOs
{
    public sealed record RecentSubscriberDTO(
        Guid Id,
        string Email,
        string? Name,
        DateTime CreatedAt,
        DateTime? ConfirmedAt,
        DateTime? UnsubscribedAt
    );
}
