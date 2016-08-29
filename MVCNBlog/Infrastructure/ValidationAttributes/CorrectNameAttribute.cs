using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCNBlog.Infrastructure.ValidationAttributes
{
    public class CorrectNameAttribute : RequiredAttribute
    {
        public CorrectNameAttribute()
        {
            ErrorMessage = "Name should be 3 to 15 length, only letters.";
        }

        public override bool IsValid(object value)
        {
            bool isValidName = false;
            if (value != null)
            {
                string name = value.ToString().ToLower();

                isValidName = Regex.IsMatch(name, @"^(?=.{3,15}$)(([a-z])\2?(?!\2))+$");
            }

            return base.IsValid(value) && (isValidName);
        }
    }
}