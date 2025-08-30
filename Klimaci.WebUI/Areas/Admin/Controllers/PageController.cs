using Klimaci.Core.Abstracts;
using Klimaci.DTO.PageDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class PageController : Controller
    {
        private readonly IPageService _svc; private readonly IUnitOfWork _uow; private readonly IWebHostEnvironment _env;
        public PageController(IPageService svc, IUnitOfWork uow, IWebHostEnvironment env) { _svc = svc; _uow = uow; _env = env; }

        [HttpGet] public async Task<IActionResult> Index([FromQuery] ListQueryDTO q) => View(await _svc.PagedAsync(q ?? new()));
        [HttpGet] public IActionResult Create() => View(new PageAddDTO("", "", null, null, null, 0, true, null, null));
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageAddDTO dto, List<IFormFile> files)
        { var ids = await SaveMediasAsync(files, "uploads/media"); dto = dto with { MediaIds = (dto.MediaIds ?? new()).Concat(ids).Distinct().ToList() }; await _svc.Add(dto); TempData["ok"] = "Sayfa eklendi."; return RedirectToAction(nameof(Index)); }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e == null) return NotFound(); ViewBag.Gallery = await _svc.GetGalleryAsync(id);
            var dto = new PageUpdateDTO(e.Id, e.Title, e.Content, e.Slug, e.SeoTitle, e.SeoDescription, e.DisplayOrder, e.IsPublished, null, null); return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageUpdateDTO dto, List<IFormFile> newFiles)
        { var ids = await SaveMediasAsync(newFiles, "uploads/media"); dto = dto with { MediaIds = (dto.MediaIds ?? new()).Concat(ids).Distinct().ToList() }; await _svc.Update(dto); TempData["ok"] = "Güncellendi."; return RedirectToAction(nameof(Index)); }

        [HttpPost][ValidateAntiForgeryToken] public async Task<IActionResult> Delete([FromForm] IdDTO dto) { await _svc.Delete(dto.Id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }
        [HttpPost] public async Task<IActionResult> TogglePublish([FromBody] TogglePublishDTO dto) { await _svc.TogglePublishAsync(dto.Id, dto.IsPublished); return Ok(); }
        [HttpPost] public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO dto) { await _svc.ReorderAsync(dto.Items ?? new()); return Ok(); }
        [HttpPost] public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto) { return Ok(await _svc.SlugCheckAsync(dto)); }
        [HttpPost] public async Task<IActionResult> GalleryUpdate([FromBody] GalleryUpdateDTO dto) { await _svc.GalleryUpdateAsync(dto); return Ok(); }

        private async Task<List<Guid>> SaveMediasAsync(List<IFormFile> files, string relFolder)
        {
            var ids = new List<Guid>(); if (files == null || files.Count == 0) return ids;
            var root = Path.Combine(_env.WebRootPath, relFolder.Replace("/", Path.DirectorySeparatorChar.ToString())); Directory.CreateDirectory(root);
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            foreach (var f in files.Where(f => f?.Length > 0))
            {
                var ext = Path.GetExtension(f.FileName).ToLowerInvariant(); if (!allowed.Contains(ext)) continue;
                var name = $"{Guid.NewGuid():N}{ext}"; var full = Path.Combine(root, name); using var fs = System.IO.File.Create(full); await f.CopyToAsync(fs);
                var media = new Media { Id = Guid.NewGuid(), FileUrl = "/" + $"{relFolder}/{name}".Replace("\\", "/"), Title = Path.GetFileNameWithoutExtension(f.FileName) };
                await _uow.Repository<Media>().AddAsync(media); ids.Add(media.Id);
            }
            await _uow.SaveChangesAsync(); return ids;
        }
    }
}
