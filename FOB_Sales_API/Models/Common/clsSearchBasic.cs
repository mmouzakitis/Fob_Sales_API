using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOBAdmin.Models.UserAccount
{
    public class clsSearchBasic
    {
        public string id { get; set; } = "";
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string email { get; set; } = "";
        public string date1 { get; set; } = "";
    }

    public class clsSearchSimple
    {
        public string search { get; set; } = "";
        public string email { get; set; } = "";
    }

}