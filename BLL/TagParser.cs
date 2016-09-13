using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public static class TagParser
    {
        public static IEnumerable<string> GetTags(string content)
        {
            var matches = Regex.Matches(content, "#[a-zA-Z0-9_.-]+");

            var tags = new List<string>();
            
            foreach (var match in matches)
            {
                if(!tags.Contains(match.ToString()))
                    tags.Add(match.ToString());
            }

            return tags;
        }
    }
}
