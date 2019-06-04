using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FOB_Sales_API.Controllers
{
    public class clsCaptcha
    {
        public static Boolean ValidateRecaptcha(string EncodedResponse)
        {
            string PrivateKey = ConfigurationManager.AppSettings["captcha_key"];

            var client = new System.Net.WebClient();

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

            var serializer = new JavaScriptSerializer();
            dynamic data = serializer.Deserialize(GoogleReply, typeof(object));

            Boolean Status = data["success"];
            if(Status == false)
            {
                return Status;
            }
            string challenge_ts = data["challenge_ts"];
            string hostname = data["hostname"];
            return Status;
        }
    }
}