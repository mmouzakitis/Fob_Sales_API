using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.KeyConstants;
using FOB_Sales_API.Models.Listings;
using FOB_Sales_API.Models.Marketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace FOB_Sales_API.Models.UserAccounts
{

    public class clsAccountsGrouped
    {
        public string account_type { get; set; }
        public int count { get; set; }
    }
    

    public class clsSalesEmployees
    {
        public string account_id { get; set; }
        public string name { get; set; }
    }


    public class clsUserAccount
    {
        public string status { get; set; }
        public string message { get; set; }

        public int index { get; set; }
        public string account_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string display_name { get; set; }
        public string user_email { get; set; }
        public string account_type { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string phone_number { get; set; }
        public string receive_txt_msg { get; set; }
        public string account_locked { get; set; }
        public bool has_listings { get; set; }
        public bool has_bookings { get; set; }
        public bool from_marketing_list { get; set; }
        public string date_created { get; set; }
        
        public void BindMarketingRecords()
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Clear();
                db.CommandText = "sp_sync_marketing_email_with_account_email";
                int result = Int32.Parse(db.ExecuteStoredProcedure());
                status = KeyConstantsMsgs.success;
                message = KeyConstantsMsgs.information_updated;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_updating_notes;
            }
        }



        public List<clsSalesEmployees> LoadSalesEmplName()
        {
            List<clsSalesEmployees> records = new List<clsSalesEmployees>();
            try
            {
                DAL db = new DAL();
                db.CommandText = "SELECT * FROM v_sales_employees";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsSalesEmployees record = new clsSalesEmployees()
                    {
                        account_id = clsCrypto.Encrypt(DBLogic.DBString(row["account_id"])),
                        name = DBLogic.DBString(row["name"]),
                    };
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return records;
            }
        }



        public List<clsAccountsGrouped> TotalAccountsCount()
        {
            List<clsAccountsGrouped> records = new List<clsAccountsGrouped>();
            try
            {
                DAL db = new DAL();
                db.CommandText = "SELECT * FROM v_accounts_grouped_by";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsAccountsGrouped record = new clsAccountsGrouped()
                    {
                        count = DBLogic.DBInteger(row["count"]),
                        account_type = DBLogic.DBString(row["account_type"]),
                    };
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return records;
            }
        }


        public List<clsUserAccount> LoadUserAccounts(clsSearhObj SearchParameters)
        {
            try
            {
                DAL db = new DAL();
                clsCommon common = new clsCommon();

                string where_clause = common.ConstructAccountWhereClause(SearchParameters, db);
                db.CommandText = "SELECT * FROM v_users " + where_clause + " ";
                DataTable dt = db.ConvertQueryToDataTable();
                clsAddressParser ParseSearchParameter = new clsAddressParser();
                List<clsUserAccount> records = new List<clsUserAccount>();
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    clsUserAccount record = new clsUserAccount()
                    {
                        index = count,
                        account_id = clsCrypto.Encrypt(DBLogic.DBString(row["account_id"])),
                        first_name = DBLogic.DBString(row["first_name"]),
                        last_name = DBLogic.DBString(row["last_name"]),
                        display_name = DBLogic.DBString(row["display_name"]),
                        user_email = DBLogic.DBString(row["user_email"]),
                        account_type = DBLogic.DBString(row["account_type"]),
                        address = DBLogic.DBString(row["street_address"]),
                        city = DBLogic.DBString(row["city"]),
                        state = DBLogic.DBString(row["state_abbr"]),
                        phone_number = DBLogic.DBString(row["phone_number"]),
                        receive_txt_msg = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["receive_text_msg"])), 
                        account_locked = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["account_locked"])),
                        has_listings = false,
                        has_bookings = false,
                        from_marketing_list = Convert.ToBoolean(row["from_marketing_list"]),
                        date_created = DBLogic.LongDateString(row["date_created"].ToString())
                    };
                    records.Add(record);
                    count = count + 1;
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }


              
        public List<clsListingDetails> LoadListingDetails(clsId id)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Add("@account_id", clsCrypto.Decrypt(id.id));
                db.CommandText = "SELECT * FROM v_listing_Details WHERE account_id=@account_id";
                DataTable dt = db.ConvertQueryToDataTable();
                List<clsListingDetails> records = new List<clsListingDetails>();
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    clsListingDetails record = new clsListingDetails()
                    {
                        index = count,
                        listing_category = DBLogic.DBString(row["listing_category"]),
                        listing_name = DBLogic.DBString(row["listing_name"]),
                        listing_city = DBLogic.DBString(row["listing_city"]),
                        listing_state_abbr = DBLogic.DBString(row["listing_state_abbr"]),
                        listing_zip = DBLogic.DBString(row["listing_zip"]),
                        listing_active = DBLogic.DBString(row["listing_active"]),
                        max_capacity = DBLogic.DBString(row["max_capacity"]),
                        coupon = DBLogic.DBString(row["coupon"]),
                        booking_fee_rate = DBLogic.DBString(row["booking_fee_rate"]),
                        total_images = DBLogic.DBString(row["total_images"]),
                        listing_verified = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["listing_verified"])),
                        date_created = DBLogic.LongDateString(row["date_created"].ToString()),
                        //receive_txt_msg = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["receive_text_msg"])),
                        //account_locked = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["account_locked"])),
                    };
                    records.Add(record);
                    count = count + 1;
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }

                
  

        public List<clsBookingDetails> LoadBookingDetails(clsId id)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Add("@listing_id",clsCrypto.Decrypt(id.id));
                db.CommandText = "SELECT * FROM v_booking_details WHERE listing_id=@listing_id";
                DataTable dt = db.ConvertQueryToDataTable();
                List<clsBookingDetails> records = new List<clsBookingDetails>();
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    clsBookingDetails record = new clsBookingDetails()
                    {
                        index = count,
                        first_name = DBLogic.DBString(row["first_name"]),
                        last_name = DBLogic.DBString(row["last_name"]),
                        listing_name = DBLogic.DBString(row["listing_name"]),
                        listing_city = DBLogic.DBString(row["listing_city"]),
                        listing_state_abbr = DBLogic.DBString(row["listing_state_abbr"]),
                        listing_zip = DBLogic.DBString(row["listing_zip"]),
                        listing_active = DBLogic.DBString(row["listing_active"]),
                        date_created = DBLogic.DBString(row["date_created"]),
                        event_date = DBLogic.DBString(row["event_date"]),
                        start_time = DBLogic.DBString(row["start_time"]),
                        end_time = DBLogic.DBString(row["end_time"]),
                        booking_fee = DBLogic.DBString(row["booking_fee"]),
                        aggreed_fee = DBLogic.DBString(row["aggreed_fee"]),
                        booking_active = DBLogic.DBString(row["booking_active"]),

                    };
                    records.Add(record);
                    count = count + 1;
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }

    }
}