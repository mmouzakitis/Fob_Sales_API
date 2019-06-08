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
    public class clsUserAccount
    {
        public int index { get; set; }
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
        public string date_created { get; set; }


        public List<clsUserAccount> LoadUserAccounts(clsSearhObj SearchParameters)
        {
            try
            {
                DAL db = new DAL();
                clsCommon common = new clsCommon();

                string where_clause = common.ConstructAccountWhereClause(SearchParameters, db);
                db.CommandText = "SELECT * FROM v_marketing_list " + where_clause + " ORDER BY date_created DESC";
                DataTable dt = db.ConvertQueryToDataTable();

                clsAddressParser ParseSearchParameter = new clsAddressParser();
                List<clsUserAccount> records = new List<clsUserAccount>();
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    clsUserAccount record = new clsUserAccount()
                    {
                        index = count,
                        first_name = DBLogic.DBString(row["first_name"]),
                        last_name = DBLogic.DBString(row["last_name"]),
                        display_name = DBLogic.DBString(row["display_name"]),
                        user_email = DBLogic.DBString(row["user_email"]),
                        account_type = DBLogic.DBString(row["account_type"]),
                        address = DBLogic.DBString(row["street_address"]),
                        city = DBLogic.DBString(row["city"]),
                        state = DBLogic.DBString(row["state_abbr"]),
                        phone_number = DBLogic.DBString(row["phone_number"]),
                        receive_txt_msg = DBLogic.DBString(row["receive_text_msg"]),
                        account_locked = DBLogic.DBString(row["account_locked"]),
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


    }
}