using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Crypto
{
    public class clsHashGenerator
    {
        public string password_hashed { get; set; }
        public string salt { get; set; }
        public string passwordHashedWithSalt  { get; set; }

        public clsHashGenerator(string password)
        {
            password_hashed = clsCrypto.HashPassword(password);
            salt = clsCrypto.GeneratePasswordSalt(12);
            passwordHashedWithSalt = clsCrypto.HashPassword(string.Format("{0}{1}", password_hashed, salt));
        }
    }
}