using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ganss.XSS;

namespace FOB_Sales_API.Models.HtmlSanitizer1
{
    public class clsHtmlSanitizer
    {
        public enum SanitizingType
        {
            Script,
            Html,
            both
        }

        HtmlSanitizer sanitizer = new HtmlSanitizer();

        public string SanitizeString(string user_input, SanitizingType type)
        {
            string value = string.Empty;

            if(type == SanitizingType.Script)
            {
                value = sanitizer.Sanitize(user_input);
            }
            if (type == SanitizingType.Html)
            {
                value = HttpUtility.HtmlEncode(user_input);
            }
            if (type == SanitizingType.both)
            {
                value = sanitizer.Sanitize(HttpUtility.HtmlEncode(user_input));
            }
            return value;
           // return user_input;
        }

    }
}