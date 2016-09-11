using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Helpers
{
    public static class TagsHighlightHelper
    {
        public static MvcHtmlString HighLightTags(this HtmlHelper html, string artcileContent)
        {
            var tags = GetTags(artcileContent).ToList();
            List<string> newTags = new List<string>();
            foreach (var tag in tags)
            {
                string newTag = tag.SurroundWith("<span style=\"background-color: #ffd500;\">", "</span>");
                newTags.Add(newTag);
            }

            for (int i = 0; i < tags.Count(); i++)
            {
                artcileContent = artcileContent.Replace(tags[i], newTags[i]);
            }

            return MvcHtmlString.Create(artcileContent);
        }

        private static IEnumerable<string> GetTags(string content)
        {
            var matches = Regex.Matches(content, "#[a-zA-Z0-9_.-]+");

            var tags = new List<string>();

            foreach (var match in matches)
            {
                tags.Add(match.ToString());
            }

            return tags;
        }

        public static string SurroundWith(this string text, string starts, string ends)
        {
            return starts + text + ends;
        }
    }
}