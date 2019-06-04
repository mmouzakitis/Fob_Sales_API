using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Interfaces
{

        interface IFoBMessages
        {
            string status { get; set; }
            string message { get; set; }
        }


}