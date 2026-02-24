using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Blog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository = null;
        private readonly ITagRepository _tagRepository = null;

        public AdminBlogPostsController(
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository)
        {
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagRepository.GetAllAsync();
            var blogPost = new BlogPostRequest
            {
                Tags = tags.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }),
                BlogPost=new BlogPost()
            };
            return View(blogPost);
        }

    }
}
