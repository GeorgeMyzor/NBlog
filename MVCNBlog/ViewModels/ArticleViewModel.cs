using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title should be 5 to 40 length.")]
        [StringLength(40, ErrorMessage = "Title should be 5 to 40 length.", MinimumLength = 5)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content should be 5 to 400 length.")]
        [StringLength(400, ErrorMessage = "Content should be 5 to 400 length.", MinimumLength = 5)]
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? AuthorId { get; set; }
        public UserViewModel Author { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}