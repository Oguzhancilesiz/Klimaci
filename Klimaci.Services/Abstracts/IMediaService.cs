using Klimaci.Core.Paging;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IMediaService
    {
        Task<PagedResult<Media>> PagedAsync(int page = 1, int pageSize = 48, string? search = null);
        Task<List<Media>> GetAll();
        Task<Media?> GetById(Guid id);
        Task Delete(Guid id); // join'ları da temizle
    }
}
