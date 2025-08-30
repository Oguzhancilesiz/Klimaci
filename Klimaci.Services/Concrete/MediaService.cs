using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class MediaService : IMediaService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEFContext _ctx;
        public MediaService(IUnitOfWork uow, IEFContext ctx) { _uow = uow; _ctx = ctx; }

        public async Task<PagedResult<Media>> PagedAsync(int page = 1, int pageSize = 48, string? search = null)
        {
            Expression<Func<Media, bool>> pred = x => string.IsNullOrEmpty(search) || (x.Title!.Contains(search) || x.FileUrl.Contains(search));
            Func<IQueryable<Media>, IOrderedQueryable<Media>> order = iq => iq.OrderByDescending(x => x.CreatedDate);

            var (items, total) = await _uow.Repository<Media>().PagedAsync(page, pageSize, pred, order);
            return new(items, total, page, pageSize);
        }

        public Task<List<Media>> GetAll() => _uow.Repository<Media>().GetAll();
        public Task<Media?> GetById(Guid id) => _uow.Repository<Media>().GetById(id);

        public async Task Delete(Guid id)
        {
            // ilişkileri temizle
            await _ctx.Set<ProjectMedia>().Where(x => x.MediaId == id).ExecuteDeleteAsync();
            await _ctx.Set<ServiceMedia>().Where(x => x.MediaId == id).ExecuteDeleteAsync();
            await _ctx.Set<PageMedia>().Where(x => x.MediaId == id).ExecuteDeleteAsync();
            await _ctx.Set<PartnerMedia>().Where(x => x.MediaId == id).ExecuteDeleteAsync();
            await _ctx.Set<BlogPostMedia>().Where(x => x.MediaId == id).ExecuteDeleteAsync();

            var repo = _uow.Repository<Media>();
            var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e);
            await _uow.SaveChangesAsync();
        }
    }
}
