using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels
{
    public sealed class ErrorViewModel
    {
        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}