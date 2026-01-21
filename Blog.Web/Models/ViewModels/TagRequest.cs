using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.ViewModels
{
    public class TagRequest
    {
        public Guid Id { get; set; }
        [DisplayName("Tag Name")]
        public string Name { get; set; }
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
    }
}
