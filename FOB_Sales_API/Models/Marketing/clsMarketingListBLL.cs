using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.KeyConstants;
using FOB_Sales_API.Models.Listings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Marketing
{
    public class clsMarketingListRecrod
    {
        public string marketing_list_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string business_name { get; set; }
        public string business_email { get; set; }
        public string business_phone { get; set; }
        public string business_address { get; set; }
        public string business_url { get; set; }
        public string business_city { get; set; }
        public string business_state { get; set; }
        public string business_zip { get; set; }
        public string contacted { get; set; }
        public string created_by_id { get; set; }
        public string created_by { get; set; }
        public bool contacted_bool { get; set; }
        public string date_added { get; set; }
    }

    public class clsEmailTemplateList
    {
        public string email_token { get; set; }
        public string template_name { get; set; }
    }

    public class clsMarketingEmail
    {
        public string marketing_list_id { get; set; }
        public string email_id { get; set; }
        public string email_from { get; set; }
        public string email_to { get; set; }
        public string email_from_id { get; set; }
        public string email_header { get; set; }
        public string email_body { get; set; }
        public string sent_by_id { get; set; }
        public string date_sent { get; set; }
    }


    public class clsPreviousContacts
    {
        public string marketing_list_id { get; set; }
        public string email_sent { get; set; }
        public string date_sent { get; set; }
        public string sent_by { get; set; }
    }

    public class clsSearhMarketingList
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public bool? contacted { get; set; }
    }

    

    public class clsMarketingListBLL
    {
        public string status { get; set; }
        public string message { get; set; }


        public List<clsMarketingEmail> LoadMarketingEmailLog(clsId IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.id));
                db.CommandText = "SELECT * FROM marketing_list_contact_events WHERE marketing_list_id=@id";
                DataTable dt = db.ConvertQueryToDataTable();
                List<clsMarketingEmail> records = new List<clsMarketingEmail>();
                foreach (DataRow row in dt.Rows)
                {
                    clsMarketingEmail record = new clsMarketingEmail();
                    record.date_sent = DBLogic.LongDateString(row["date_contacted"].ToString());
                    record.email_from = DBLogic.DBString(row["from_email"]);
                    record.email_to = DBLogic.DBString(row["to_email"]);
                    record.email_header = DBLogic.DBString(row["email_header"]);
                    record.email_body = DBLogic.DBString(row["email_body"]);
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

        public void SendMarketingEmail(clsMarketingEmail emailObj)
        {
            try
            {
                clsEmailBLL SendEmail = new clsEmailBLL(emailObj.email_header, emailObj.email_body, KeyConstants.KeyConstants.FishOnBooking, "", emailObj.email_from, emailObj.email_to);
                SendEmail.SendEmail();
            }
            catch (Exception ex)
            {
                status = KeyConstantsMsgs.error;
                message = "Email might not have been sent, please do not try too many times or the receiver will register us as spam.";
                return;
            }
            try
            {
                DAL db = new DAL();
                db.Parameters("marketing_list_id", clsCrypto.Decrypt(emailObj.marketing_list_id));
                db.Parameters("account_id", clsCrypto.Decrypt(emailObj.email_from_id));
                db.Parameters("from_email", emailObj.email_from);
                db.Parameters("to_email", emailObj.email_to);
                db.Parameters("email_header", emailObj.email_header);
                db.Parameters("email_body", emailObj.email_body);
                db.CommandText = "sp_create_new_marketing_contact_events";
                db.ExecuteStoredProcedure();
                status = KeyConstantsMsgs.success;
                message = "Email Sent successfully";
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = "Could not send email";
            }
        }

        public List<clsEmailTemplateList> LoadMarketingEmailTemplates()
       {
            try
            {
                DAL db = new DAL();
                db.CommandText = "SELECT email_id,email_title FROM system_emails WHERE is_marketing_template='true' AND is_active='true' ";
                DataTable dt = db.ConvertQueryToDataTable();
                List<clsEmailTemplateList> records = new List<clsEmailTemplateList>();
                records = (from DataRow dr in dt.Rows
                           select new clsEmailTemplateList()
                           {
                               email_token = clsCrypto.Encrypt(dr["email_id"].ToString()),
                               template_name = DBLogic.DBString(dr["email_title"])
                           }).ToList();
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }

        
        public clsMarketingEmail LoadMarketingEmail(clsMultipliIds IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.email_token));
                db.Parameters("marketing_list_id", clsCrypto.Decrypt(IdObj.marketing_token));

                db.CommandText = "SELECT * FROM v_system_emails WHERE email_id=@id ";
                DataTable dt = db.ConvertQueryToDataTable();
                clsMarketingEmail record = new clsMarketingEmail();
                foreach (DataRow row in dt.Rows)
                {
                    record.email_id = clsCrypto.Encrypt(DBLogic.DBString(row["email_id"]));
                    record.email_header = DBLogic.DBString(row["email_header"]);
                    record.email_body = DBLogic.DBString(row["email_body"]);
                }
                db.Parameters("account_id", clsCrypto.Decrypt(IdObj.account_token));
                db.CommandText = "SELECT account_id, member_email,first_name,last_name FROM meet_the_team WHERE account_id=@account_id";
                DataTable dtUser = db.ConvertQueryToDataTable();
              
                db.CommandText = "SELECT business_email FROM marketing_list WHERE marketing_list_id=@marketing_list_id";
                string email_to = db.ExecuteScalar().ToString();
                db.CommandText = "SELECT coupon FROM listing_coupon_bank WHERE owner_id=@account_id";
                string promo_code = db.ExecuteScalar().ToString();
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE  setting_id=5";
                string default_booking_fee = db.ExecuteScalar().ToString();
                

                db.CommandText = "SELECT first_name +' '+ last_name AS NAME FROM marketing_list WHERE marketing_list_id=@marketing_list_id";
                string customers_name = db.ExecuteScalar().ToString();
                 
                foreach (DataRow row in dtUser.Rows)
                {
                    record.email_from_id  = clsCrypto.Encrypt(DBLogic.DBString(row["account_id"]));
                    record.email_from = DBLogic.DBString(row["member_email"]);
                    record.email_to = email_to;
                    string my_first_name = row["first_name"].ToString();
                    string my_last_name = row["last_name"].ToString();
                    record.email_body = record.email_body.Replace("{customers_name}", customers_name).Replace("{promo_code}", promo_code).Replace("{my_name}", my_first_name + " " + my_last_name).Replace("{default_booking_fee}",default_booking_fee);
                }

                return record;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



        public clsMarketingListRecrod LoadSingleMarketingRecord(clsId IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.id));
                db.CommandText = "SELECT * FROM v_marketing_list WHERE marketing_list_id=@id";
                DataTable dt = db.ConvertQueryToDataTable();
                clsMarketingListRecrod record = new clsMarketingListRecrod();
                foreach (DataRow row in dt.Rows)
                {
                    record.marketing_list_id = clsCrypto.Encrypt(DBLogic.DBString(row["marketing_list_id"]));
                    record.first_name = DBLogic.DBString(row["first_name"]);
                    record.last_name = DBLogic.DBString(row["last_name"]);
                    record.business_phone = DBLogic.DBString(row["business_phone"]);
                    record.business_name = DBLogic.DBString(row["business_name"]);
                    record.business_email = DBLogic.DBString(row["business_email"]);
                    record.business_city = DBLogic.DBString(row["business_city"]);
                    record.business_address = DBLogic.DBString(row["business_address"]);
                    record.business_url = DBLogic.DBString(row["business_website"]);
                    record.business_city = DBLogic.DBString(row["business_city"]);
                    record.business_state = DBLogic.DBString(row["business_state"]);
                    record.business_zip = DBLogic.DBString(row["business_zip"]);
                    record.created_by = DBLogic.DBString(row["created_by"]);
                }
                return record;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }


        public List<clsMarketingListRecrod> LoadMarketingList(clsSearhMarketingList SearchParameters)
        {
            try
            {
                DAL db = new DAL();
                clsAddressParser ParseSearchParameter = new clsAddressParser();
               
                string where_clause = ConstructMarketingWhereClause(SearchParameters, db);
         
                db.CommandText = "SELECT * FROM v_marketing_list " + where_clause + " ORDER BY date_created DESC";
                DataTable dt = db.ConvertQueryToDataTable();

                List<clsMarketingListRecrod> records = new List<clsMarketingListRecrod>();
                records = (from DataRow row in dt.Rows
                           select new clsMarketingListRecrod()
                           {
                               marketing_list_id = clsCrypto.Encrypt(DBLogic.DBString(row["marketing_list_id"])),
                               first_name = DBLogic.DBString(row["first_name"]),
                               last_name = DBLogic.DBString(row["last_name"]),
                               business_phone = DBLogic.DBString(row["business_phone"]),
                               business_name = DBLogic.DBString(row["business_name"]),
                               business_email = DBLogic.DBString(row["business_email"]),
                               business_address = DBLogic.DBString(row["business_address"]),
                               business_url = DBLogic.DBString(row["business_website"]),
                               business_city = DBLogic.DBString(row["business_city"]),
                               business_state = DBLogic.DBString(row["business_state"]),
                               business_zip = DBLogic.DBString(row["business_zip"]),
                               created_by = DBLogic.DBString(row["created_by"]),
                               contacted_bool = DBLogic.DBBool(row["has_been_contacted"]),
                               contacted = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["has_been_contacted"])),
                               date_added = DBLogic.LongDateString(row["date_created"].ToString())
                           }).ToList();
                return records;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



        private string ConstructMarketingWhereClause(clsSearhMarketingList SearchParameters, DAL db)
        {
            string where_clause = string.Empty;
            bool add_AND = false;
            db.ClearParameters();
            if (string.IsNullOrEmpty(SearchParameters.address) == false)
            {
                db.Parameters("address", SearchParameters.address);
                where_clause = "business_address=@address";
                add_AND = true;
            }
            if (string.IsNullOrEmpty(SearchParameters.city) == false)
            {
                db.Parameters("city", SearchParameters.city);
                if(add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_city=@city";
                }
                add_AND = true;
            }
            if (string.IsNullOrEmpty(SearchParameters.state) == false)
            {
                db.Parameters("state", SearchParameters.state);
                
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_state=@state";
                }
                add_AND = true;
            }
            if(string.IsNullOrEmpty(SearchParameters.email) == false)
            {
                db.Parameters("email", SearchParameters.email);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_email=@email";
                }
                add_AND = true;
            }
            if (SearchParameters.contacted != null)
            {
                db.Parameters("contacted", SearchParameters.contacted.ToString());
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "has_been_contacted=@contacted";
                }
                add_AND = true;
            }
            if (where_clause.Length > 0)
            {
                where_clause = " WHERE " + where_clause;
                add_AND = true;
            }
            return where_clause;
        }


        public void CreateNewMarketingRecord(clsMarketingListRecrod record)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Clear();
                db.Parameters("first_name", record.first_name.Trim());
                db.Parameters("last_name", record.last_name.Trim());
                db.Parameters("business_name", record.business_name.Trim());
                db.Parameters("business_phone",DBLogic.FormatPhoneForDatabase(record.business_phone.Trim()));
                db.Parameters("business_email", record.business_email.Trim());
                db.Parameters("business_address", record.business_address.Trim());
                db.Parameters("business_website", record.business_url.Trim());
                db.Parameters("business_city", record.business_city.Trim());
                db.Parameters("business_state", record.business_state.Trim());
                db.Parameters("business_zip", record.business_zip.Trim());
                db.Parameters("created_by_id", clsCrypto.Decrypt(record.created_by_id));
                db.CommandText = "sp_create_new_marketing_contact";
                int result = Int32.Parse(db.ExecuteStoredProcedure());
                if(result == -10)
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.business_name_already_exists;
                }else if(result == -20)
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.email_exists;
                }
                else
                {
                    status = KeyConstantsMsgs.success;
                    message = KeyConstantsMsgs.marketing_record_created;
                }
                
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_creating_new_marketing_record;
            }
        }
        


        public void UpdateMarketingRecord(clsMarketingListRecrod record)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Clear();
                db.Parameters("marketing_list_id", clsCrypto.Decrypt(record.marketing_list_id));
                db.Parameters("first_name", record.first_name.Trim());
                db.Parameters("last_name", record.last_name.Trim());
                db.Parameters("business_name", record.business_name.Trim());
                db.Parameters("business_phone",DBLogic.FormatPhoneForDatabase(record.business_phone.Trim()));
                db.Parameters("business_email", record.business_email.Trim());
                db.Parameters("business_address", record.business_address.Trim());
                db.Parameters("business_website", record.business_url.Trim());
                db.Parameters("business_city", record.business_city.Trim());
                db.Parameters("business_state", record.business_state.Trim());
                db.Parameters("business_zip", record.business_zip.Trim());
                db.CommandText = "sp_update_marketing_contact";
                int result = Int32.Parse(db.ExecuteStoredProcedure());
                status = KeyConstantsMsgs.success;
                message = KeyConstantsMsgs.information_updated;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_updating_marketing_list;
            }
        }



    }


}