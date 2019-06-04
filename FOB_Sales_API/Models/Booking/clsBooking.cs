using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOB_Sales_API.Models.Listings;
using FOB_Sales_API.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace FOB_Sales_API.Models.Booking
{


    public class clsBooking 
    {
        public int index { get; set; }
        public string b_id { get; set; }
        public string reservation_no { get; set; }
        public string acc_id { get; set; }
        public string s_id { get; set; }
        public string l_id { get; set; }
        public bool event_active { get; set; }
        //public string booking_status_str { get; set; }
        public string booking_status_text { get; set; }
        public string customers_name { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string listing_name { get; set; }
        public string listing_location { get; set; }
        public string date_of_booking { get; set; }
        public string category { get; set; }
        public string event_date { get; set; }
        public string event_date_long { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string trip_duration { get; set; }
        public double base_price { get; set; }
        public string base_price_currency { get; set; }
        public int base_guests { get; set; }
        //public int cancellation_cut_off_days { get; set; }
        public bool can_cancel { get; set; }
        public double booking_fee_rate { get; set; }
        public int promo_transactions_remaining { get; set; }
        public double agreed_fee { get; set; }
        public double booking_fee { get; set; }
        public double total_booking_cost { get; set; }
        //public double cost_per_extra_guest { get; set; }
        public string agreed_fee_currency { get; set; }
        public string booking_fee_currency { get; set; }
        //public string total_booking_cost_currency { get; set; }
        public int no_of_guests { get; set; }
        public string location { get; set; }
        public double max_capacity { get; set; }
        public bool past_booking { get; set; }
        public bool booking_active { get; set; }

    }

    
    public class clsCancelPurchase
    {
        [Required(ErrorMessage = "1110:There is missing information, cannot cancel")]
        public string b_token { get; set; }

        [StringLength(500)]
        [Required(ErrorMessage = "1111:Cancel reason required")]
        public string cancel_reason { get; set; }
        
        public string lookup_id { get; set; }

        [Required(ErrorMessage = "1113:There is missing information, cannot cancel, maybe your session has expired")]
        public string a_token { get; set; }
    }



    public class clsCalendarBooking
    {
        public string id { get; set; }
        public bool is_blocked_by_captain { get; set; }
        public string title { get; set; }
        public bool event_active { get; set; }
        public string backgroundColor { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

    public class clsSearchCalendar
    {
        public string l_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    public class clsSearchBookings
    {
        [StringLength(12)]
        public string event_date { get; set; } = "";
        [StringLength(20)]
        public string last_name { get; set; } = "";
        public string reservation_no { get; set; } = "";
    }

    public class clsAvailability
    {
        public string b_token { get; set; }
    }


    public class clsCompletedBooking
    {
        public string acc_token { get; set; }
        public string event_date { get; set; }
        public decimal amount_to_charge { get; set; }
        public string s_token { get; set; }
        public string l_token { get; set; }
        public string no_of_guests { get; set; }
        public int booking_confirmation_no { get; set; }
        public string credit_card_transaction_id { get; set; }
        public string authorization_code { get; set; }
        public string booking_message { get; set; }
        public bool booking_succeeded { get; set; }
        public bool credit_transaction_succeeded { get; set; }
        public string credit_card_message { get; set; }
        public int time_offset_greenwich { get; set; }
        public string first_name_on_card { get; set; }
        public string last_name_on_card { get; set; }
    }

    public class clsCompleteBookingByCaptain
    {
        public string l_token { get; set; }
        public string s_token { get; set; }
        [StringLength(10, ErrorMessage = "1001:Date cannot be longer than 10 characters. Example: 11/11/2019")]
        public string event_date { get; set; }
        public string price { get; set; }
        [StringLength(3, ErrorMessage = "1002:Number of guests cannot exceed 999.")]
        public string no_of_guests { get; set; }
        [StringLength(20, ErrorMessage = "1003:Name cannot be longer than 20 characters.")]
        public string f_name { get; set; }
        [StringLength(20, ErrorMessage = "1004:Last name cannot be longer than 20 characters")]
        public string l_name { get; set; }
        [StringLength(50, ErrorMessage = "1005:Email cannot be longer than 50 characters")]
        public string email { get; set; }
        [StringLength(13, ErrorMessage = "10046:Phone number cannot be longer than 13 characters")]
        public string phone_no { get; set; }
        public string booking_confirmation_no { get; set; }
        public string booking_message { get; set; }
        public bool booking_succeeded { get; set; }
    }


    public class clsPreBookingSearch
    {
        public string s_id { get; set; }
        [StringLength(10)]
        public string event_date { get; set; }
        //[StringLength(3)]
        public int no_of_guests { get; set; }
    }

    public class clsBookingTotals
    {
        public int count { get; set; }
        public string event_date { get; set; }
        public string event_date_long { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string listing_name { get; set; }
        public string listing_state { get; set; }
        public string listing_city { get; set; }
        public string listing_guests { get; set; }
    }

    public class clsBookingStatsSearch
    {
        public int year { get; set; }
        public int month { get; set; }
        public string event_date { get; set; }
        public string search_type { get; set; }

        public string GetLastDayOfMonth()
        {
            DateTime endOfMonth = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return endOfMonth.ToShortDateString();
        }

        public string GetFirstDayOfMonth()
        {
            DateTime endOfMonth = new DateTime(year, month, 1);
            return endOfMonth.ToShortDateString();
        }
    }


    public class clsBookingDetails : clsBooking
    {
        public string l_address { get; set; }
        public string l_city { get; set; }
        public string l_state_abbr { get; set; }
        public string l_state { get; set; }
        public string l_description { get; set; }

        public string boat_length { get; set; }
        //public int max_capacity { get; set; }
        public int charge_type_id { get; set; }
        public string charge_type { get; set; }
        public string c_id { get; set; }
        public string c_name { get; set; }
        public string c_email { get; set; }
        public string c_phone { get; set; }
        public string canceled_date { get; set; }
        public string canceled_reason { get; set; }
        public string canceled_by_lookup { get; set; }
        public int canceled_by_lookup_id { get; set; }

        public bool has_errors { get; set; }
        public bool error_msg { get; set; }

        public string date_created { get; set; }
        public string edited_by_id { get; set; }
    }


    public class clsNewBooking
    {
        [Required]
        public string list_id { get; set; }
        [Required]
        public string acc_id { get; set; }
        [Required]
        public string slot_id { get; set; }
        [Required]
        public string event_date { get; set; }
        [Required]
        public string no_of_guests { get; set; }
        public string edited_by_id { get; set; }
    }

}