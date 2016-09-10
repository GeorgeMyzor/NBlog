using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Helpers
{
    public static class PopularArticlesHelper
    {
        public static MvcHtmlString GetPopular(this HtmlHelper html, IEnumerable<ArticleViewModel> articles,
            Func<int, string> articleUrl)
        {
            StringBuilder result = new StringBuilder();

            foreach (var article in articles)
            {
                CreateLinkTag(articleUrl, article, result);
            }

            return MvcHtmlString.Create(result.ToString());
        }

        private static void CreateLinkTag(Func<int, string> articleUrl, ArticleViewModel article, StringBuilder result)
        {
            TagBuilder tagLi = new TagBuilder("li");
            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttribute("href", articleUrl(article.Id));
            tag.InnerHtml = article.Title;
            tag.MergeAttribute("value", article.Id.ToString());

            tagLi.InnerHtml = tag.ToString();
            result.Append(tagLi);
        }
    }
}