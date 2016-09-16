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
            var startPage = GetStartPage(pagingInfo);

            var lastPage = GetLastPage(pagingInfo);

            for (int i = startPage; i <= lastPage; i++)
            {
                TagBuilder tagLi = new TagBuilder("li");
                TagBuilder tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", pageUrl(i));
                tagA.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tagA.AddCssClass("selected");
                tagA.AddCssClass("page");
                tagA.MergeAttribute("value", i.ToString());

                tagLi.InnerHtml = tagA.ToString();
                result.Append(tagLi);
            }

            return MvcHtmlString.Create(result.ToString());
        }

        private static int GetLastPage(PagingInfo pagingInfo)
        {
            int lastPage;
            if (pagingInfo.CurrentPage + 2 > pagingInfo.TotalPages)
                lastPage = pagingInfo.TotalPages;
            else
                lastPage = pagingInfo.CurrentPage + 2;
            return lastPage;
        }

        private static int GetStartPage(PagingInfo pagingInfo)
        {
            int startPage;
            if (pagingInfo.CurrentPage - 2 < 1)
                startPage = 1;
            else
                startPage = pagingInfo.CurrentPage - 2;
            return startPage;
        }
    }
}