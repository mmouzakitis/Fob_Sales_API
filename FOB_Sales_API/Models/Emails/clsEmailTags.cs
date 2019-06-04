using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Emails
{
  
    public class clsEmailTags
    {
        public string reservation_no { get; set; }
        public string refund_amount { get; set; }
        public string agreed_fee { get; set; }
        public string booking_fee { get; set; }
        public string total_guests { get; set; }
        public string pay_policy { get; set; }
        public string cancelation_policy { get; set; }
        public string canceled_date { get; set; }
        public string canceled_reason { get; set; }
        public string canceled_by { get; set; }
        public string cut_off_date_for_cancelation { get; set; }
        public string customer_first_name { get; set; }
        public string customer_last_name { get; set; }
        public string customer_display_name { get; set; }
        public string customer_email { get; set; }
        public string customer_phone_number { get; set; }
        public bool receive_txt_msg { get; set; }
        public string event_address { get; set; }
        public string event_city { get; set; }
        public string event_state { get; set; }
        public string event_zip { get; set; }
        public string event_location { get; set; }
        public string event_name { get; set; }
        public string event_date { get; set; }
        public string event_start_time { get; set; }
        public string event_end_time { get; set; }
        public string event_duration { get; set; }
        public string contact_person { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public string date_of_booking { get; set; }

    }


}