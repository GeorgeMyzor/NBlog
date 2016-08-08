using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public UserViewModel Author { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}