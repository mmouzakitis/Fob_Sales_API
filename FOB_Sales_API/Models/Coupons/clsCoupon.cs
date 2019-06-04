using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Coupons
{

    public class clsEmailCoupon
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "User email cannot be longer than 50 characters")]
        [EmailAddress(ErrorMessage = "A valid email is required")]
        public string email_to { get; set; }

        public string coupon { get; set; }
        //public string coupon_value { get; set; }
    }


    public class clsApplyCoupon
    {
        public string l_token { get; set; }
        public string coupon { get; set; }
    }

    
    public class clsListToken
    {
        public string l_token { get; set; }
    }

    public class clsAccToken
    {
        public string acc_token { get; set; }
    }

    public class clsUsedCouponDetails
    {
        public string event_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string reservation { get; set; }
    }


    public class clsCouponTally
    {
        public int consumed_coupons { get; set; }
        public int unused_coupons { get; set; }
        public string discount_fee_rate { get; set; }
        public string no_of_discounts { get; set; }
        public string booking_fee { get; set; }
    }

    public class clsCoupon
    {
        public int index { get; set; }
        public string owner_id { get; set; }
        public string coupon { get; set; }
        public string coupon_value { get; set; }
        public bool coupon_active { get; set; }
        public string coupon_sent_to { get; set; }
        public string date_coupon_sent { get; set; }
        public bool was_sent { get; set; }
        public string used_by { get; set; }
    }

    
    public class clsCommissionAggregate
    {
        public int index { get; set; }
        public string sales_code_id { get; set; }
        public bool is_total { get; set; }
        public int bookings { get; set; }
        public double commission { get; set; }
        public string total { get; set; }
        public double total_double { get; set; }
        public string date_of_event { get; set; }
        public string event_date_short { get; set; }
    }
    
    public class clsCommissionDetails
    {
        public int index { get; set; }
        public int sales_code_id { get; set; }
        public string l_name { get; set; }
        public string l_city { get; set; }
        public string l_zip { get; set; }
        public string l_state { get; set; }
        public string l_category { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string date_of_event { get; set; }
        public double commission_rate { get; set; }
        public double secondary_commission_rate { get; set; }
        public string commission_currency { get; set; }
    }

    public class clsSearchAggCoupons
    {
        public string id { get; set; }
        public int month { get; set; }
        public int  year { get; set; }
        public bool is_primary { get; set; }
    }

    public class clsSerchCommissionByDate
    {
        public string id { get; set; }
        public string event_date { get; set; }
        public bool is_primary { get; set; }
    }

    public class clsMyCoupon
    {
        public int index { get; set; }
        public string coupon_id { get; set; }
        public string account_id { get; set; }
        public string coupon_name { get; set; }
        public string commission_rate { get; set; }
        public bool is_primary { get; set; }
    }

    public class clsPromoCodeHolders
    {
        public int index { get; set; }
        public string coupon_id { get; set; }
        public string owner_id { get; set; }
        public string secondary_owner_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string first_name_secondary { get; set; }
        public string last_name_secondary { get; set; }
    }
}
