using Klimaci.Core.Abstracts;
using Klimaci.Core.Extensions;
using Klimaci.Core.Paging;
using Klimaci.DTO.ProjectDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        private readonly IUnitOfWork _uow;                 // Sadece medya kaydı için
        private readonly IWebHostEnvironment _env;         // Dosya kaydı için

        public ProjectController(IProjectService service, IUnitOfWork uow, IWebHostEnvironment env)
        {
            _service = service;
            _uow = uow;
            _env = env;
        }

        // LIST
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ListQueryDTO q)
        {
            var model = await _service.PagedAsync(q ?? new());
            return View(model); // View modeli: PagedResult<Project>
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _uow.Repository<Brand>().GetAll();
            return View(new ProjectAddDTO(
                Title: "", Description: null, City: null, CompletedAt: null, BrandId: null,
                Slug: null, SeoTitle: null, SeoDescription: null, DisplayOrder: 0, IsPublished: true,
                MediaIds: null, CoverMediaId: null
            ));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectAddDTO dto, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await _uow.Repository<Brand>().GetAll();
                return View(dto);
            }

            // Yüklenen dosyaları Media olarak kaydet ve ID'leri DTO'ya ekle
            var uploadedIds = await SaveMediasAsync(files, "uploads/media");
            var mergedIds = (dto.MediaIds ?? new List<Guid>()).Concat(uploadedIds).Distinct().ToList();
            dto = dto with { MediaIds = mergedIds };

            await _service.Add(dto);
            TempData["ok"] = "Proje eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _service.GetById(id);
            if (e is null) return NotFound();

            ViewBag.Brands = await _uow.Repository<Brand>().GetAll();
            ViewBag.Gallery = await _service.GetGalleryAsync(id); // ← YENİ

            var dto = new ProjectUpdateDTO(
                e.Id, e.Title, e.Description, e.City, e.CompletedAt,
                e.BrandId, e.Slug, e.SeoTitle, e.SeoDescription,
                e.DisplayOrder, e.IsPublished, null, null
            );
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectUpdateDTO dto, List<IFormFile> newFiles)
        {
            var e = await _service.GetById(dto.Id);
            if (e is null) return NotFound();

            // Yeni yüklenen görselleri ekle
            var uploadedIds = await SaveMediasAsync(newFiles, "uploads/media");
            var mergedIds = (dto.MediaIds ?? new List<Guid>()).Concat(uploadedIds).Distinct().ToList();
            dto = dto with { MediaIds = mergedIds };

            await _service.Update(dto);
            TempData["ok"] = "Proje güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // DELETE (soft)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] IdDTO dto)
        {
            await _service.Delete(dto.Id);
            TempData["ok"] = "Proje silindi.";
            return RedirectToAction(nameof(Index));
        }

        // PUBLISH TOGGLE (ajax)
        [HttpPost]
        public async Task<IActionResult> TogglePublish([FromBody] TogglePublishDTO dto)
        {
            await _service.TogglePublishAsync(dto.Id, dto.IsPublished);
            return Ok();
        }

        // REORDER (ajax)
        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO dto)
        {
            await _service.ReorderAsync(dto.Items ?? new());
            return Ok();
        }

        // SLUG CHECK (ajax)
        [HttpPost]
        public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto)
        {
            var result = await _service.SlugCheckAsync(dto);
            return Ok(result);
        }

        // GALLERY UPDATE (ajax)
        [HttpPost]
        public async Task<IActionResult> GalleryUpdate([FromBody] GalleryUpdateDTO dto)
        {
            await _service.GalleryUpdateAsync(dto);
            return Ok();
        }

        // ----------------- Helpers -----------------

        // Basit medya kaydetme: dosyayı wwwroot/uploads/... altına yazar, Media kaydı oluşturur, MediaId döner
        private async Task<List<Guid>> SaveMediasAsync(List<IFormFile> files, string relFolder)
        {
            var ids = new List<Guid>();
            if (files == null || files.Count == 0) return ids;

            var root = Path.Combine(_env.WebRootPath, relFolder.Replace("/", Path.DirectorySeparatorChar.ToString()));
            Directory.CreateDirectory(root);

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var f in files.Where(f => f?.Length > 0))
            {
                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext)) continue;

                var name = $"{Guid.NewGuid():N}{ext}";
                var full = Path.Combine(root, name);

                using (var fs = System.IO.File.Create(full))
                    await f.CopyToAsync(fs);

                var media = new Media
                {
                    Id = Guid.NewGuid(),
                    FileUrl = "/" + $"{relFolder}/{name}".Replace("\\", "/"),
                    Title = Path.GetFileNameWithoutExtension(f.FileName)
                };

                await _uow.Repository<Media>().AddAsync(media);
                ids.Add(media.Id);
            }

            await _uow.SaveChangesAsync();
            return ids;
        }
    }
}
