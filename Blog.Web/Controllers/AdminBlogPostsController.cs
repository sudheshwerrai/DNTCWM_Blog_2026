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
                BlogPost = new BlogPost()
            };
            return View(blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostRequest blogPostRequest)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Heading = blogPostRequest.BlogPost.Heading,
                    PageTitle = blogPostRequest.BlogPost.PageTitle,
                    Content = blogPostRequest.BlogPost.Content,
                    ShortDescription = blogPostRequest.BlogPost.ShortDescription,
                    FeaturedImageUrl = blogPostRequest.BlogPost.FeaturedImageUrl,
                    UrlHandle = blogPostRequest.BlogPost.UrlHandle,
                    PublishedDate = blogPostRequest.BlogPost.PublishedDate,
                    Author = blogPostRequest.BlogPost.Author,
                    Visible = blogPostRequest.BlogPost.Visible,
                };

                var listOfTags = new List<Tag>();

                foreach (var selectedTagId in blogPostRequest.SelectedTags)
                {
                    var selectedTag = Guid.Parse(selectedTagId);
                    var existingTag = await _tagRepository.GetAsync(selectedTag);
                    if (existingTag != null)
                    {
                        listOfTags.Add(existingTag);
                    }
                }

                blogPost.Tags = listOfTags;
                await _blogPostRepository.AddAsync(blogPost);
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPostFromDb = await _blogPostRepository.GetAsync(id);
            var tags = await _tagRepository.GetAllAsync();

            var blogPost = new BlogPostRequest();
            blogPost.BlogPost = blogPostFromDb;
            blogPost.Tags = tags.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            blogPost.SelectedTags = blogPostFromDb.Tags.Select(t => t.Id.ToString()).ToArray();

            return View(blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BlogPostRequest blogPostRequest)
        {
            if (ModelState.IsValid && blogPostRequest.BlogPost.Id == id)
            {
                var blogPost = new BlogPost
                {
                    Id= blogPostRequest.BlogPost.Id,
                    Heading = blogPostRequest.BlogPost.Heading,
                    PageTitle = blogPostRequest.BlogPost.PageTitle,
                    Content = blogPostRequest.BlogPost.Content,
                    ShortDescription = blogPostRequest.BlogPost.ShortDescription,
                    FeaturedImageUrl = blogPostRequest.BlogPost.FeaturedImageUrl,
                    UrlHandle = blogPostRequest.BlogPost.UrlHandle,
                    PublishedDate = blogPostRequest.BlogPost.PublishedDate,
                    Author = blogPostRequest.BlogPost.Author,
                    Visible = blogPostRequest.BlogPost.Visible,
                };

                var listOfTags = new List<Tag>();

                foreach (var selectedTagId in blogPostRequest.SelectedTags)
                {
                    var selectedTag = Guid.Parse(selectedTagId);
                    var existingTag = await _tagRepository.GetAsync(selectedTag);
                    if (existingTag != null)
                    {
                        listOfTags.Add(existingTag);
                    }
                }

                blogPost.Tags = listOfTags;

                await _blogPostRepository.UpdateAsync(blogPost);
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Delete(Guid id)
        {
            await _blogPostRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
