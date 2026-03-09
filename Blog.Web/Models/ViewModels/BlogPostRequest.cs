using Blog.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Models.ViewModels
{
    public class BlogPostRequest
    {
        public BlogPost BlogPost { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Tags { get; set; }        
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
