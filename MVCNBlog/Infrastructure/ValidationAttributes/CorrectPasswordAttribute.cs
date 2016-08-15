using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCNBlog.Infrastructure.ValidationAttributes
{
    public class CorrectPasswordAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValidPassword = false;
            if (value != null)
            {
                isValidPassword = Regex.IsMatch(value.ToString(), @"^(?=.{5,15}$)(?=.*[0-9])([a-zA-Z0-9])+$");
            }

            return base.IsValid(value) && (isValidPassword);
        }
    }
}