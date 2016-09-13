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
            IList<string> newTags;
            var tags = GetTags(artcileContent, out newTags).ToList();

            for (int i = 0; i < tags.Count(); i++)
            {
                artcileContent = artcileContent.Replace(tags[i], newTags[i]);
            }

            return MvcHtmlString.Create(artcileContent);
        }

        private static IEnumerable<string> GetTags(string content, out IList<string> hightlightedTags)
        {
            var matches = Regex.Matches(content, "#[a-zA-Z0-9_.-]+");

            var tags = new List<string>();
            hightlightedTags = new List<string>();

            foreach (var match in matches)
            {
                if(tags.Contains(match.ToString()))
                    continue;
                tags.Add(match.ToString());

                string newTag = match.ToString().SurroundWith("<span style=\"background-color: #C0C0C0;\">", "</span>");
                hightlightedTags.Add(newTag);
            }

            return tags;
        }

        public static string SurroundWith(this string text, string starts, string ends)
        {
            return starts + text + ends;
        }
    }
}