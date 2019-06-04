using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FOB_Sales_API.Models.Crypto
{
    public class clsRandomNumber
    {
        public static string RandomNumber()
        {
            //const string pool = "aA01b2B3c4C57d6D73e8E49f0F1g2G73h462H5i65I7j8J39k0K-l1L23m5M6n7N8o9O0pPqQrRsStTuUvVwWxXyYzZ";
            //var builder = new StringBuilder();

            //var rnd = new Random(DateTime.Now.Millisecond);
            //for (var i = 0; i <= 5; i++)
            //{
            //    var c = pool[rnd.Next(0, pool.Length)];
            //    builder.Append(c);
            //}

            Random random = new Random();
            const string chars1 = "Aa1Bb2Cc3Dd4Ee5Ff6Gg7Hh8Ii9Jj1Kk2LlM2N0Oo1Pp2Qq3RrSsTtUuV0v1W2w3X4x5Y6y7Z89z56789";
            string str = new string(Enumerable.Repeat(chars1, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            return str;// builder.ToString();
            //var chars = "A1BCD2E-FG-HI3-JKLM4NO-PQR5ST6UVW7XYZ8abcdef9ghij^+-";
            //var stringChars = new char[6];
            //var random = new Random();

            //for (int i = 0; i < stringChars.Length; i++)
            //{
            //    stringChars[i] = chars[random.Next(chars.Length)];
            //}

           // var finalString = new String(stringChars);
            //return finalString;
            ////var rnd = new Random(DateTime.Now.Millisecond);
            //int ticks = rnd.Next(1999, 45999);
            //return ticks.ToString();
        }
    }
}