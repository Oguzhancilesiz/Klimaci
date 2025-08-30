using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.DashboardDTOs
{
    public sealed record DashboardCountersDTO(
         int TotalSubscribers,
         int Confirmed,
         int Pending,
         int Unsubscribed,
         int NewLast24h
     );
}
