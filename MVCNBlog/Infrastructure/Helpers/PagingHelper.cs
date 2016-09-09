using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Helpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            int pageCount;
            int currentPage;
            if (pagingInfo.CurrentPage - 2 < 1)
                currentPage = 1;
            else
                currentPage = pagingInfo.CurrentPage - 2;

            if (pagingInfo.CurrentPage + 2 > pagingInfo.TotalPages)
                pageCount = pagingInfo.TotalPages;
            else
                pageCount = pagingInfo.CurrentPage + 2;

            for (int i = currentPage; i <= pageCount; i++)
            {
                TagBuilder tagLi = new TagBuilder("li");
                TagBuilder tag = new TagBuilder("a"); 
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("selected");
                tag.AddCssClass("page");
                tag.MergeAttribute("value", i.ToString());

                tagLi.InnerHtml = tag.ToString();
                result.Append(tagLi);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}