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
       // public string base_price { get; set; }
        //public string cancellation_cut_off_days { get; set; }
        //public string fishing_type_id { get; set; }
        //public string captain_id { get; set; }
        //public string contact_id { get; set; }
        //public string base_guests { get; set; }
        //public string extra_per_guest { get; set; }
        //public string pay_policy { get; set; }
        //public string transaction_type_id { get; set; }
    }

    public class clsSearchContainer
    {
       public int record_count { get; set; }
       public  List<clsListing> records { get; set; }
    }

    public class clsListing 
    {
        public string l_id { get; set; }
        public int index { get; set; }
        public string acc_id { get; set; }
        public string l_name { get; set; }
        public string b_length { get; set; }
        public string b_manufacturer { get; set; }
        public string l_city { get; set; }
        public string l_state{ get; set; }
        public string l_state_abbr { get; set; }
        public string listers_name { get; set; }
        public string max_guests { get; set; }
        public string l_category { get; set; }
        public bool l_verified { get; set; }
        public string l_category_id { get; set; }
        public string l_desc_first_half { get; set; }
        public string l_desc_second_half { get; set; }
        public bool expand_description { get; set; }
        public int l_status { get; set; }
        public string l_active_text { get; set; }
        public string create_date { get; set; }
        public string main_image_path { get; set; }
        public int no_of_reviews { get; set; }
        public float average_rating { get; set; }
        public string fish_species_ids { get; set; }
        public string feature_ids { get; set; }

        public string pay_policy { get; set; }
        public string cancellation_policy { get; set; }
        public string notes_to_angler { get; set; }
        
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

    public class clsListingDetails : clsListing
    {
        //public string l_category_id { get; set; }
        public string l_description { get; set; }
        public string contact_id { get; set; }

        public string main_image { get; set; }
        public string listers_f_name { get; set; }
        public string listers_l_name { get; set; }
        public string contact_f_name { get; set; }
        public string contact_l_name { get; set; }
        public string captains_name { get; set; }

        public string listers_username { get; set; }
        public string listers_contact_number { get; set; }
        //public string total_rating { get; set; }
        //public string total_raters { get; set; }
        //public string rating { get; set; }
       // public string l_state { get; set; }
        public string state { get; set; }
        //public string meets_listing_criteria { get; set; }
        public int l_active { get; set; }
        public string removed_date { get; set; }
        public string l_zip { get; set; }
        public string provence { get; set; }
        public string listing_description_details { get; set; }
        public string listing_location { get; set; }
        //public string boat_length { get; set; }
        public int max_capacity { get; set; }
        public string l_address { get; set; }
        public string country_id { get; set; }
        public string country { get; set; }
        public string fishing_type_id { get; set; }

        public string cancellation_cut_off_days { get; set; }
    }




    public class clsUpdateListingStatus
    {
        public string l_id { get; set; }
        public string status { get; set; }

    }
}
