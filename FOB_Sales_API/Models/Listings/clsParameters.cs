using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Listings
{

    public class clsSearchListings
    {
        public string l_name { get; set; } = "";
        public string acc_token { get; set; } = "";
        public string l_token { get; set; } = "";
    }

    public class clsUpdateListingContact
    {
        public string listing_id { get; set; } = "";
        public string contact_id { get; set; } = "";
        public string update_type { get; set; } = "";
    }

    public class clsUpdateListingCaptain
    {
        public string listing_id { get; set; } = "";
        public string captain_id { get; set; } = "";
        public string update_type { get; set; } = "";
    }

    
    //public class clsUpdateListingKeyValue
    //{
    //    public string id { get; set; } = "";
    //    public string value { get; set; } = "";
    //}
    public class clsAvailSearch
    {
        public string list_token { get; set; }
        public string month { get; set; }
        public string index { get; set; }
    }

    public class clsAvailabilityOverview
    {
        public string availability_status { get; set; }
        public string month { get; set; }
        public string availability_status_css { get; set; }
        public string index { get; set; }
    }

    public class clsMainSearch
    {
        public string search_parameter { get; set; } = "";
        public string search_type { get; set; } = "";
        public string cat_token { get; set; } = "";
        public string order_by { get; set; } = "";
        public int start_index { get; set; } = 1;
        public int records_per_page { get; set; } = 10;
        //public int start_record { get; set; } = 10;
        //public int end_record { get; set; } = 10;
    }

    public class clsMultipliIds
    {
        public string email_token { get; set; }
        public string account_token { get; set; }
        public string marketing_token { get; set; }
    }


    public class clsId
    {
        public string id { get; set; }
    }

    public class clsIds
    {
        public List<string> ids  { get; set; }
    }
}