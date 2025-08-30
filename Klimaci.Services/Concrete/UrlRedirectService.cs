using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO.UrlRedirectDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class UrlRedirectService : IUrlRedirectService
    {
        private readonly IUnitOfWork _uow;
        public UrlRedirectService(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<UrlRedirect>> PagedAsync(int page = 1, int pageSize = 30, string? search = null)
        {
            Expression<Func<UrlRedirect, bool>> pred = x => string.IsNullOrWhiteSpace(search)
                || x.OldPath.Contains(search) || x.NewPath.Contains(search);
            var (items, total) = await _uow.Repository<UrlRedirect>().PagedAsync(page, pageSize, pred, iq => iq.OrderByDescending(x => x.CreatedDate));
            return new(items, total, page, pageSize);
        }

        public Task<UrlRedirect?> GetByIdAsync(Guid id) => _uow.Repository<UrlRedirect>().GetById(id);

        public async Task AddAsync(UrlRedirectAddDTO dto)
        {
            var repo = _uow.Repository<UrlRedirect>();
            var e = dto.Adapt<UrlRedirect>(); e.Id = Guid.NewGuid();
            e.OldPath = Normalize(dto.OldPath); e.NewPath = Normalize(dto.NewPath);
            await repo.AddAsync(e); await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(UrlRedirectUpdateDTO dto)
        {
            var repo = _uow.Repository<UrlRedirect>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e);
            e.OldPath = Normalize(dto.OldPath); e.NewPath = Normalize(dto.NewPath);
            await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var repo = _uow.Repository<UrlRedirect>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }

        private static string Normalize(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) return "/";
            p = p.Trim();
            if (!p.StartsWith("/")) p = "/" + p;
            return p;
        }
    }
}
