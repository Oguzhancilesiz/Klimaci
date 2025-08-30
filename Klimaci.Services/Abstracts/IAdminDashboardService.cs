using Klimaci.DTO.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IAdminDashboardService
    {
        /// <summary>
        /// Admin dashboard verilerini getirir.
        /// </summary>
        /// <param name="days">Günlük abonelik grafiği için kaç günlük veri.</param>
        Task<AdminDashboardVM> GetAsync(int days = 30);
    }
}
