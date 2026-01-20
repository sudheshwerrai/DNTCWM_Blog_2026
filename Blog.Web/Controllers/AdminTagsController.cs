using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Web.Models.Domain;
using Microsoft.VisualBasic;

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
    }
}
