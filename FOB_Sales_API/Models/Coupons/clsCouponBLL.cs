using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.Interfaces;
using FOB_Sales_API.Models.KeyConstants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Coupons
{
    public class clsCouponBLL : IFoBMessages
    {
        public string status { get; set; }
        public string message { get; set; }
        public const string error_applying_coupon = "There was an error applying coupon";


        public List<clsUsedCouponDetails> LoadUsedCouponDetails(string listing_id)
        {
            try
            {
                List<clsUsedCouponDetails> list = new List<clsUsedCouponDetails>();
                DAL db = new DAL();
                db.Parameters("listing_id", clsCrypto.Decrypt(listing_id));
                db.CommandText = "SELECT * FROM v_used_promo_bookings WHERE listing_id=@listing_id AND promo_used='true' ";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsUsedCouponDetails record = new clsUsedCouponDetails();
                    record.event_date = DBLogic.LongDateString(dt.Rows[0]["event_date"].ToString());
                    record.start_time = DBLogic.DBString(dt.Rows[0]["start_time"]);
                    record.end_time = DBLogic.DBString(dt.Rows[0]["end_time"]);
                    list.Add(record);
                }
                return list;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error;
                return null;
            }
        }


        public clsCouponTally GetPromotionalTally(string listing_id)
        {
            try
            {
                clsCouponTally coupon_tally = new clsCouponTally();
                DAL db = new DAL();
                db.Parameters("listing_id", clsCrypto.Decrypt(listing_id));
                db.CommandText = "SELECT promo_transactions_remaining,promo_transactions_used FROM listings WHERE listing_id=@listing_id ";
                DataTable dt = db.ConvertQueryToDataTable();
                if (dt.Rows.Count == 1)
                {
                    coupon_tally.unused_coupons = DBLogic.DBInteger(dt.Rows[0]["promo_transactions_remaining"]);
                    coupon_tally.consumed_coupons = DBLogic.DBInteger(dt.Rows[0]["promo_transactions_used"]);
                }
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE setting_id=6 ";
                coupon_tally.discount_fee_rate = (string)db.ExecuteScalar();
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE setting_id=21 ";
                coupon_tally.no_of_discounts = (string)db.ExecuteScalar();
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE setting_id=5 ";
                coupon_tally.booking_fee = (string)db.ExecuteScalar();

                return coupon_tally;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error;
                return null;
            }
        }


        public bool CouponIsValid(string coupon_name)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("coupon", coupon_name.Trim());
                db.CommandText = "SELECT Count(*) FROM listing_coupon_bank WHERE coupon=@coupon AND is_valid='true'";
                int result = (int)db.ExecuteScalar();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return false;
            }
        }


        public void ApplyCoupon(clsApplyCoupon coupon)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("listing_id", clsCrypto.Decrypt(coupon.l_token));
                db.Parameters("coupon", coupon.coupon.Trim());
                db.CommandText = "sp_apply_coupon";
                string result = db.ExecuteStoredProcedure();
                if (result == "-10")
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.coupon_is_used;
                }
                if (result == "-20")
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.cannot_apply_to_self;
                }
                if (result == "-30")
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.already_sent_to_user;
                }
                if (result == "1" || result=="2")
                {
                    status = KeyConstantsMsgs.success;
                    message = KeyConstantsMsgs.coupon_applied;
                }
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = error_applying_coupon;
            }
        }

        
        public List<clsPromoCodeHolders> LoadPromoCodeHolders()
        {
            try
            {
                List<clsPromoCodeHolders> coupon_list = new List<clsPromoCodeHolders>();
                DAL db = new DAL();
                db.CommandText = "SELECT *  FROM v_promo_code_holders ";
                DataTable dt = db.ConvertQueryToDataTable();
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count += 1;
                    clsPromoCodeHolders record = new clsPromoCodeHolders();
                    record.index = count;
                    record.coupon_id = clsCrypto.Encrypt(DBLogic.DBString(row["coupon_id"]));
                    record.owner_id = clsCrypto.Encrypt(DBLogic.DBString(row["owner_id"]));
                    record.secondary_owner_id = clsCrypto.Encrypt(DBLogic.DBString(row["secondary_owner_id"]));
                    record.first_name = DBLogic.DBString(row["first_name"]);
                    record.last_name = DBLogic.DBString(row["last_name"]);
                    record.first_name_secondary = DBLogic.DBString(row["first_name_secondary"]);
                    record.last_name_secondary = DBLogic.DBString(row["last_name_secondary"]);
                    coupon_list.Add(record);
                }
                return coupon_list;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_retrieving_coupons;
                return null;
            }
        }



        public List<clsMyCoupon> LoadMyPromoCodes(string account_id)
        {
            try
            {
                List<clsMyCoupon> coupon_list = new List<clsMyCoupon>();
                DAL db = new DAL();
                db.Parameters("account_id", clsCrypto.Decrypt(account_id));
                db.CommandText = "SELECT *  FROM v_my_coupons WHERE owner_id=@account_id ";
                DataTable dt = db.ConvertQueryToDataTable();
                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count += 1;
                    clsMyCoupon record = new clsMyCoupon();
                    record.index = count;
                    record.coupon_id = clsCrypto.Encrypt(DBLogic.DBString(row["coupon_id"]));
                    record.account_id = clsCrypto.Encrypt(DBLogic.DBString(row["owner_id"]));
                    record.is_primary = true;
                    record.coupon_name = DBLogic.DBString(row["coupon"]);
                    record.commission_rate = DBLogic.ConvertToCurrency(DBLogic.DBDouble(row["commission_rate"]));
                    coupon_list.Add(record);
                }
                db.CommandText = "SELECT *  FROM v_my_coupons_secondary WHERE secondary_owner_id=@account_id ";
                DataTable dt_secondary = db.ConvertQueryToDataTable();
                int count_secondary = 0;
                foreach (DataRow row in dt_secondary.Rows)
                {
                    count_secondary += 1;
                    clsMyCoupon record = new clsMyCoupon();
                    record.index = count;
                    record.coupon_id = clsCrypto.Encrypt(DBLogic.DBString(row["coupon_id"]));
                    record.account_id = clsCrypto.Encrypt(DBLogic.DBString(row["secondary_owner_id"]));
                    record.is_primary = false;
                    record.coupon_name = DBLogic.DBString(row["coupon"]);
                    record.commission_rate = DBLogic.ConvertToCurrency(DBLogic.DBDouble(row["secondary_commission_rate"]));
                    coupon_list.Add(record);
                }
                return coupon_list;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_retrieving_coupons;
                return null;
            }
        }

        private string GetFirstDayOfMonth(int month, int year)
        {
           return new DateTime(year, month, 1).ToShortDateString();
        }

        private string GetLastDayOfMonth(int month, int year)
        {
            var start_date = new DateTime(year, month, 1);
            var end_date = start_date.AddMonths(1).AddDays(-1);
            return end_date.ToShortDateString();
        }

        
    
        public List<clsCommissionAggregate> CommissionByDate(clsSearchAggCoupons search)
        {
            try
            {
                List<clsCommissionAggregate> commission_list = new List<clsCommissionAggregate>();
                DAL db = new DAL();
                db.Parameters("promo_code_id", clsCrypto.Decrypt(search.id));
                db.Parameters("begining_of_month", GetFirstDayOfMonth(search.month,search.year));
                db.Parameters("end_of_month", GetLastDayOfMonth(search.month,search.year));
                db.CommandText = "SELECT sales_code_id,CAST(event_date AS DATE) AS date_of_event,count(*) as bookings FROM v_promo_code_bookings where sales_code_id=@promo_code_id AND booking_active='true' AND event_date>=@begining_of_month AND event_date<=@end_of_month GROUP BY sales_code_id,CAST(event_date AS DATE) ";
                DataTable dt = db.ConvertQueryToDataTable();
                int count = 0;
                int bookings = 0;
                double commission_rate = 0;
                double total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count += 1;
                    clsCommissionAggregate record = new clsCommissionAggregate();
                    record.index = count;
                    record.is_total = false;
                    record.sales_code_id = clsCrypto.Encrypt(DBLogic.DBString(row["sales_code_id"]));
                    record.bookings = DBLogic.DBInteger(DBLogic.DBDouble(row["bookings"]));
                    bookings = bookings + record.bookings;
                    record.date_of_event = DBLogic.LongDateString(row["date_of_event"].ToString());
                    record.event_date_short = DBLogic.ShortDateString((row["date_of_event"].ToString()));
                    commission_list.Add(record);
                }
                if (count > 1)
                {
                    clsCommissionAggregate record = new clsCommissionAggregate();
                    record.index = count + 1;
                    record.sales_code_id = "-1";
                    record.is_total = true;
                    record.bookings = bookings;
                    record.commission = commission_rate;
                    record.total = DBLogic.ConvertToCurrency(total);
                    record.date_of_event = GetCommissionSum(search);
                    commission_list.Add(record);
                }
                return commission_list;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_retrieving_coupons;
                return null;
            }
        }

        public string GetCommissionSum(clsSearchAggCoupons search)
        {
            DAL db = new DAL();
            db.Parameters("promo_code_id", clsCrypto.Decrypt(search.id));
            db.Parameters("begining_of_month", GetFirstDayOfMonth(search.month, search.year));
            db.Parameters("end_of_month", GetLastDayOfMonth(search.month, search.year));
            db.CommandText = "SELECT * FROM v_booking_details_small WHERE sales_code_id=@promo_code_id AND event_date>=@begining_of_month  AND event_date<=@end_of_month AND booking_active='true'   ";
            DataTable dt = db.ConvertQueryToDataTable();
            double commission = 0;
            double total_commission = 0;
            double total_secondary_commission = 0;
            foreach (DataRow row in dt.Rows)
            {
                clsCommissionDetails record = new clsCommissionDetails();
                total_commission = total_commission + DBLogic.DBDouble(row["commission"]);
                total_secondary_commission = total_secondary_commission + DBLogic.DBDouble(row["secondary_commission"]);
            }
            if(search.is_primary == true)
            {
                commission = total_commission;
            }
            else
            {
                commission = total_secondary_commission;
            }

            return DBLogic.ConvertToCurrency(commission);
        }


        public List<clsCommissionDetails> CommissionDetailsByDate(clsSerchCommissionByDate search)
        {
            try
            {
                List<clsCommissionDetails> commission_list = new List<clsCommissionDetails>();
                DAL db = new DAL();
                db.Parameters("promo_code_id", clsCrypto.Decrypt(search.id));
                db.Parameters("event_date", search.event_date);
                db.CommandText = "SELECT * FROM v_booking_details_small WHERE sales_code_id=@promo_code_id AND event_date=@event_date AND booking_active='true'   ";
                DataTable dt = db.ConvertQueryToDataTable();
                int count = 0;
                double total_commission = 0;
                foreach (DataRow row in dt.Rows)
                {
                    count += 1;
                    clsCommissionDetails record = new clsCommissionDetails();
                    record.index = count;
                    record.l_name = DBLogic.DBString(row["listing_name"]);
                    record.l_city = DBLogic.DBString(row["listing_city"]);
                    record.l_zip = DBLogic.DBString(row["listing_zip"]);
                    record.l_state = DBLogic.DBString(row["listing_state_abbr"]);
                    record.l_category = DBLogic.DBString(row["listing_category"]);
                    record.end_time = DBLogic.LongDateString(DBLogic.DBString(row["start_time"]));
                    record.start_time = DBLogic.LongDateString(DBLogic.DBString(row["end_time"]));
                    record.date_of_event = DBLogic.ShortDateString(DBLogic.DBString(row["event_date"]));
                    record.commission_rate = DBLogic.DBDouble(row["commission"]);
                    record.secondary_commission_rate = DBLogic.DBDouble(row["secondary_commission"]);
                    if (search.is_primary)
                    {
                        total_commission = total_commission + record.commission_rate;
                        record.commission_currency = DBLogic.ConvertToCurrency(DBLogic.DBDouble(row["commission"]));
                    }
                    else
                    {
                        total_commission = total_commission + record.secondary_commission_rate;
                        record.commission_currency = DBLogic.ConvertToCurrency(DBLogic.DBDouble(row["secondary_commission"]));
                    }
                    commission_list.Add(record);
                }
                if (count > 1)
                {
                    clsCommissionDetails record = new clsCommissionDetails();
                    record.index = count;
                    record.l_name = "Total";
                    record.l_city = "";
                    record.l_zip = "";
                    record.l_state = "";
                    record.l_category = "";
                    record.end_time = "";
                    record.start_time = "";
                    record.date_of_event = "";
                    record.commission_rate = total_commission;
                    record.commission_currency = DBLogic.ConvertToCurrency(total_commission);
                    commission_list.Add(record);
                }
                return commission_list;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_retrieving_coupons;
                return null;
            }
        }



    }
}