using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCNBlog.ViewModels
{
    public sealed class ListViewModel<T> 
    {
        public IEnumerable<T> ViewModels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}