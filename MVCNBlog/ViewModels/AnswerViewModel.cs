﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVCNBlog.ViewModels.User;

namespace MVCNBlog.ViewModels
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Comment should be from 1 to 200 length.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Comment should be from 1 to 200 length.")]
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsAnswer { get; set; }
        public UserViewModel Author { get; set; }
    }
}