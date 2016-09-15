using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCNBlog.Infrastructure
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder newString = new StringBuilder(str.Length);
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    newString.Append(c);
                }
                else if(c == ' ')
                {
                    newString.Append("-");
                }
            }

            return newString.ToString().ToLower();
        }
    }
}