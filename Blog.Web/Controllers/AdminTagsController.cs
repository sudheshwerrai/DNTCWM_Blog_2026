using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository = null;
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags = await _tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TagRequest tagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Name = tagRequest.Name,
                    DisplayName = tagRequest.DisplayName
                };
                await _tagRepository.AddAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tagRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tagFromDb = await _tagRepository.GetAsync(id);
            if (tagFromDb == null)
                return NotFound();

            var tagVM = new TagRequest
            {
                Id = tagFromDb.Id,
                Name = tagFromDb.Name,
                DisplayName = tagFromDb.DisplayName
            };
            return View(tagVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagRequest tagRequest)
        {
            if (ModelState.IsValid)
            {
                var tag = new Tag
                {
                    Id = tagRequest.Id,
                    Name = tagRequest.Name,
                    DisplayName = tagRequest.DisplayName
                };

                await _tagRepository.UpdateAsync(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tagRequest);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tagFromDb = await _tagRepository.GetAsync(id);
            if (tagFromDb == null)
                return BadRequest();
            await _tagRepository.DeleteAsync(tagFromDb);
            return RedirectToAction(nameof(Index));
        }
    }
}
