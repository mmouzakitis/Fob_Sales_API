using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FOB_Sales_API.Models.Listings
{

    //Michael Mouzakitis 04-13-2019

    public class clsListingNew
    {
        public string acc_token { get; set; }
        public string cat_token { get; set; }
        [Required(ErrorMessage = "Cntact is required")]
        public string contact_token { get; set; }
        [Required(ErrorMessage = "Captain  is required")]
        public string captain_token { get; set; }
        [Required(ErrorMessage = "Listing name is required")]
        public string l_name { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public int b_length { get; set; }
        [Required(ErrorMessage = "Boat length is required")]
        public int b_capacity { get; set; }
        [Required(ErrorMessage = "Manufacturer name is required")]
        public string b_manufacturer { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "110:First name cannot be longer than 25 characters.")]
        public string l_description { get; set; }
        [Required(ErrorMessage = "Street Address is required")]
        public string l_address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string l_city { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string l_state { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public string l_zip { get; set; }
        [Required(ErrorMessage = "Charge per hour is required")]
        public string charge_per_hour { get; set; }
        public string coupon_code { get; set; }
    }

    public class clsListingStatus
    {
        [Required]
        public string listing_token { get; set; }
        [Required]
        public string acc_token { get; set; }
    }

    public class clsLockListings
    {
        public string account_id { get; set; }
        public string status { get; set; }
    }

    public class clsAdsSearch
    {
        public string address { get; set; }
        public string search_type { get; set; }
    }

    public class clsUpdateListingDetails
    {
        public string l_id { get; set; }
        public string cat_token { get; set; }
        public string l_active { get; set; }
        public string b_length { get; set; }
        public string b_manufacturer { get; set; }
        public string guest_capacity { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string notes_to_angler { get; set; }
        public string cancellation_policy { get; set; }

    }

  

    public class clsListingList
    {
        public string l_id { get; set; }
        public string l_name { get; set; }
    }


    public class clsListingAd
    {
        public string l_id { get; set; }
        public bool active { get; set; }
        public bool is_taken { get; set; }
        public string l_name { get; set; }
        public string l_city { get; set; }
        public string l_state { get; set; }
        public string image_path { get; set; }
        public string l_category { get; set; }
    }

    public class clsListingDetails 
    {
        public int index { get; set; }
        public string listing_category { get; set; }
        public string listing_name { get; set; }
        public string listing_city { get; set; }
        public string listing_state_abbr { get; set; }
        public string listing_zip { get; set; }
        public string listing_active { get; set; }
        public string max_capacity { get; set; }
        public string coupon { get; set; }
        public string booking_fee_rate { get; set; }
        public string total_images { get; set; }
        public string listing_verified { get; set; }
        public string date_created { get; set; }
        public string account_locked { get; set; }
        public string receive_txt_msg { get; set; }
    }


    public class clsBookingDetails
    {
        public int index { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string listing_name { get; set; }
        public string listing_state_abbr { get; set; }
        public string listing_zip { get; set; }
        public string listing_active { get; set; }
        public string listing_city { get; set; }
        public string date_created { get; set; }
        public string event_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string booking_fee { get; set; }
        public string aggreed_fee { get; set; }
        public string booking_active { get; set; }                     
    }

 

    public class clsUpdateListingStatus
    {
        public string l_id { get; set; }
        public string status { get; set; }

    }
}
