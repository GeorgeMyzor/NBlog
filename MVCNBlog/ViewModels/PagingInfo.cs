﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels
{
    public sealed class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }

}