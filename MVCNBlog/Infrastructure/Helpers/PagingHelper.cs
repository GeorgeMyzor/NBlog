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
        private const int PageLinkIndent = 1;
        /// <summary>
        /// Creates <a></a> tag in <li></li> tag for every page number. Every <a></a> tag with number on it 
        /// contains link to page with same number. Only one link will be with "selected" class. 
        /// Also creates link to first and last pages;
        /// </summary>
        /// <param name="html">Html helper to be extended.</param>
        /// <param name="pagingInfo">Info about pages.</param>
        /// <param name="pageUrl">Delegate that transform page number into Url.</param>
        /// <returns>li tag with page link.</returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            var startPage = GetStartPage(pagingInfo);
            var lastPage = GetLastPage(pagingInfo);
            
            if (startPage > 1)
            {
                var firstTagLi = CreatePageTag(pagingInfo, pageUrl, 1);

                result.Append(firstTagLi);

                if (startPage > 2)
                {
                    TagBuilder span = new TagBuilder("span");
                    span.SetInnerText("...");
                    result.Append(span);
                }
            }

            for (int i = startPage; i <= lastPage; i++)
            {
                var tagLi = CreatePageTag(pagingInfo, pageUrl, i);
                result.Append(tagLi);
            }

            if (lastPage < pagingInfo.TotalPages)
            {
                if (lastPage < pagingInfo.TotalPages - 1)
                {
                    TagBuilder span = new TagBuilder("span");
                    span.SetInnerText("...");
                    result.Append(span);
                }

                var lastTagLi = CreatePageTag(pagingInfo, pageUrl, pagingInfo.TotalPages);

                result.Append(lastTagLi);
            }
            

            return MvcHtmlString.Create(result.ToString());
        }

        private static TagBuilder CreatePageTag(PagingInfo pagingInfo, Func<int, string> pageUrl, int i)
        {
            TagBuilder tagLi = new TagBuilder("li");
            tagLi.InnerHtml = pageUrl(i);

            if (i == pagingInfo.CurrentPage)
            {
                TagBuilder pageNum = new TagBuilder("input");
                pageNum.MergeAttribute("type", "hidden");
                pageNum.MergeAttribute("value",i.ToString());
                pageNum.MergeAttribute("id","pageNum");

                tagLi.InnerHtml += pageNum.ToString();
            }
            
            return tagLi;
        }

        private static int GetLastPage(PagingInfo pagingInfo)
        {
            int lastPage;
            if (pagingInfo.CurrentPage + PageLinkIndent > pagingInfo.TotalPages)
                lastPage = pagingInfo.TotalPages;
            else
                lastPage = pagingInfo.CurrentPage + PageLinkIndent;
            return lastPage;
        }

        private static int GetStartPage(PagingInfo pagingInfo)
        {
            int startPage;
            if (pagingInfo.CurrentPage - PageLinkIndent < 1)
                startPage = 1;
            else
                startPage = pagingInfo.CurrentPage - PageLinkIndent;
            return startPage;
        }
    }
}