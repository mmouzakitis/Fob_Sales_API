using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.Listings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Marketing
{
    public class clsMarketingAd
    {
        public string image_url { get; set; }
        public string image_path { get; set; }
        public string image_title { get; set; }
        public int padding { get; set; }
    }

    public class clsAdPayments
    {
        public int marketing_add_payment_id { get;set;}
        public int my_marketing_ad_id { get;set;}
        public string credit_card_transaction_id  { get;set;}
        public string authorization_code  { get;set;}
        public string charge_reversed  { get;set;}
        public string marketing_city { get;set;}
        public string image_title { get;set;}
        public string ad_start_date { get;set;}
        public string ad_end_date { get;set;}
    }

    public class clsMyMarketingAds
    {
        public string account_token { get; set; }
        public string image_url { get; set; }
        public string image_path { get; set; }
        public int padding { get; set; }
    }

    public class clsCity
    {
        public string city { get; set; }
    }
    public class clsMarketingAdBLL
    {

        public List<clsMarketingAd> LoadMarketingAds(clsAdsSearch SearchParameters)
        {

            try
            {
                DAL db = new DAL();
                clsAddressParser ParseSearchParameter = new clsAddressParser();
                ParseSearchParameter.ParseAddress(SearchParameters.address);

                string where_clause = "";
                if (SearchParameters.search_type == "destination")
                {
                    if (ParseSearchParameter.has_city == true)
                    {
                        db.parameters.Add("city", ParseSearchParameter.parameter_value);
                        if (ParseSearchParameter.parameter_value != "") { where_clause = " AND marketing_city=@city "; }
                    }
                    if (ParseSearchParameter.has_state == true)
                    {
                        db.parameters.Add("state", ParseSearchParameter.parameter_value);
                        if (ParseSearchParameter.parameter_value != "") { where_clause = " AND marketing_state=@state "; }
                    }
                }
                List<clsMarketingAd> records = new List<clsMarketingAd>();
                db.CommandText = "SELECT TOP 7 * FROM marketing_ads_live WHERE date_of_add=CONVERT(VARCHAR(10), GETDATE(), 111) " + where_clause + " ORDER BY NEWID()";
              
                DataTable dt = db.ConvertQueryToDataTable();
                if(dt.Rows.Count <= 0)
                {
                    db.CommandText = "SELECT TOP 7 * FROM marketing_ads_live_premium ORDER BY NEWID()";
                    dt = db.ConvertQueryToDataTable();
                }
                foreach (DataRow row in dt.Rows)
                {
                    clsMarketingAd record = new clsMarketingAd();
                    record.image_url = DBLogic.DBString(row["image_url"]);
                    record.image_path = DBLogic.DBString(row["image_path"]);
                    record.image_title = DBLogic.DBString(row["image_title"]);
                    record.padding = DBLogic.DBInteger(row["padding"]);
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }


        public List<clsMyMarketingAds> MyMarketingAds(clsId Id)
        {
            try
            {
                DAL db = new DAL();
                List<clsMyMarketingAds> records = new List<clsMyMarketingAds>();
                db.parameters.Add("account_id",clsCrypto.Decrypt(Id.id));
                db.CommandText = "SELECT * FROM v_my_marketing_ads WHERE account_id=@account_id";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsMyMarketingAds record = new clsMyMarketingAds();
                    record.image_url = DBLogic.DBString(row["image_url"]);
                    record.image_path = DBLogic.DBString(row["image_path"]);
                    record.padding = DBLogic.DBInteger(row["padding"]);
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



        public List<clsAdPayments> MyPurchasedAdds(clsId Id)
        {
            try
            {
                DAL db = new DAL();
                List<clsAdPayments> records = new List<clsAdPayments>();
                db.parameters.Add("account_id", clsCrypto.Decrypt(Id.id));
                db.CommandText = "SELECT * FROM v_my_marketing_ads_purchased WHERE account_id=@account_id";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsAdPayments record = new clsAdPayments();
                    record.marketing_add_payment_id = DBLogic.DBInteger(row["marketing_add_payment_id"]);
                    record.my_marketing_ad_id = DBLogic.DBInteger(row["my_marketing_ad_id"]);
                    record.credit_card_transaction_id = DBLogic.DBString(row["credit_card_transaction_id"]);
                    record.authorization_code = DBLogic.DBString(row["authorization_code"]);
                    record.charge_reversed = DBLogic.DBString(row["charge_reversed"]);
                    record.marketing_city = DBLogic.DBString(row["marketing_city"]);
                    record.ad_start_date = DBLogic.DBString(row["ad_start_date"]);
                    record.ad_end_date = DBLogic.DBString(row["ad_end_date"]);
                    records.Add(record);
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