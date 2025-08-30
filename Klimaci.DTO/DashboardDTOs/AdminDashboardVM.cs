using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.DashboardDTOs
{
    public sealed class AdminDashboardVM
    {
        public DashboardCountersDTO Counters { get; init; } = new(0, 0, 0, 0, 0);
        public List<ChartPointDTO> DailySignups { get; init; } = new();
        public List<RecentSubscriberDTO> RecentSubscribers { get; init; } = new();

        public double ConfirmedRate =>
            Counters.TotalSubscribers == 0 ? 0 : (double)Counters.Confirmed / Counters.TotalSubscribers * 100.0;

        public double UnsubscribedRate =>
            Counters.TotalSubscribers == 0 ? 0 : (double)Counters.Unsubscribed / Counters.TotalSubscribers * 100.0;
    }
}
