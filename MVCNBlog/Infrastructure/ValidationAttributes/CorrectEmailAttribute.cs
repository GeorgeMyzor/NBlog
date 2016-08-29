using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCNBlog.Infrastructure.ValidationAttributes
{
    public class CorrectEmailAttribute : RequiredAttribute
    {
        public CorrectEmailAttribute()
        {
            ErrorMessage = "Email should be 5 to 30 length.";
        }

        public override bool IsValid(object value)
        {
            bool isValidEmail = false;
            if (value != null)
            {
                string name = value.ToString().ToLower();

                isValidEmail = Regex.IsMatch(name,
                    @"^(?=.{5,30}$)[a-z0-9!#$%&'*+=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            }

            return base.IsValid(value) && (isValidEmail);
        }
    }
}