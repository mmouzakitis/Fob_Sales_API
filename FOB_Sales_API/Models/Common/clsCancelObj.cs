using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Common
{
    public class clsCancelObj
    {
        public string id { get; set; }
        public string cancel_reason { get; set; }
        public string GUID { get; set; }
        public string edited_by_id { get; set; }
    }

}