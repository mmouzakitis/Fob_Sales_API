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

namespace FOB_Sales_API.Models.Business
{
    public class clsBusinessListRecrod
    {
        public int index { get; set; }
        public string id { get; set; }
        public string Business_list_id { get; set; }
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
        public bool has_notes { get; set; }
        public string date_added { get; set; }
    }

    public class clsEmails
    {
        public string email { get; set; }
    }


    public class clsEmailTemplateList
    {
        public string email_token { get; set; }
        public string template_name { get; set; }
    }

    public class clsBusinessEmail
    {
        public string Business_list_id { get; set; }
        public string email_id { get; set; }
        public string email_from { get; set; }
        public string email_to { get; set; }
        public string email_from_id { get; set; }
        public string email_header { get; set; }
        public string email_body { get; set; }
        public string sent_by_id { get; set; }
        public string date_sent { get; set; }
        public string to_multiple_recipients { get; set; }
    }


    public class clsPreviousContacts
    {
        public string Business_list_id { get; set; }
        public string email_sent { get; set; }
        public string date_sent { get; set; }
        public string sent_by { get; set; }
    }



    public class clsNotes
    {
        public string id { get; set; }
        public string notes { get; set; }
    }

    public class clsBusinessListBLL
    {
        public string status { get; set; }
        public string message { get; set; }


        public List<clsBusinessEmail> LoadBusinessEmailLog(clsId IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.id));
                db.CommandText = "SELECT * FROM business_list_contact_events WHERE Business_list_id=@id";
                DataTable dt = db.ConvertQueryToDataTable();
                List<clsBusinessEmail> records = new List<clsBusinessEmail>();
                foreach (DataRow row in dt.Rows)
                {
                    clsBusinessEmail record = new clsBusinessEmail();
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

        public void SendBusinessEmail(clsBusinessEmail emailObj)
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
                db.Parameters("Business_list_id", clsCrypto.Decrypt(emailObj.Business_list_id));
                db.Parameters("account_id", clsCrypto.Decrypt(emailObj.email_from_id));
                db.Parameters("from_email", emailObj.email_from);
                db.Parameters("to_email", emailObj.email_to);
                db.Parameters("email_header", emailObj.email_header);
                db.Parameters("email_body", emailObj.email_body);
                db.CommandText = "sp_create_new_Business_contact_events";
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

            string[] emails = emailObj.to_multiple_recipients.Split(',');
            foreach (var email in emails)
            {
                try
                {
                    clsEmailBLL SendEmail = new clsEmailBLL(emailObj.email_header, emailObj.email_body, KeyConstants.KeyConstants.FishOnBooking, "", emailObj.email_from, email);
                    SendEmail.SendEmail();
                    DAL db = new DAL();
                    db.Parameters("Business_list_id", clsCrypto.Decrypt(emailObj.Business_list_id));
                    db.Parameters("account_id", clsCrypto.Decrypt(emailObj.email_from_id));
                    db.Parameters("from_email", emailObj.email_from);
                    db.Parameters("to_email", email);
                    db.Parameters("email_header", emailObj.email_header);
                    db.Parameters("email_body", emailObj.email_body);
                    db.CommandText = "sp_create_new_Business_contact_events";
                    db.ExecuteStoredProcedure();
                }
                catch (Exception) 
                {
                    status = KeyConstantsMsgs.error;
                    message = "Email might not have been sent, please do not try too many times or the receiver will register us as spam.";
                }
            }
        }


  
        public string LoadBusinessEmailsForSending(clsId id)
        {
            if (id.id.StartsWith(",")){
                id.id = id.id.Remove(0, 1);
            }
            try
            {
                string emails = string.Empty;
                DAL db = new DAL();
                db.CommandText = "SELECT business_email FROM Business_list WHERE Business_list_id IN ("+ id.id +") ";
                DataTable dt = db.ConvertQueryToDataTable();

                foreach(DataRow dr in dt.Rows)
                {
                    if (dr["business_email"].ToString().Length > 5)
                    {
                        emails = emails + "," + dr["business_email"].ToString();
                    }
                }
                if (emails.StartsWith(","))
                {
                    emails = emails.Remove(0, 1);
                }
                return emails;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



        public List<clsEmailTemplateList> LoadBusinessEmailTemplates()
       {
            try
            {
                DAL db = new DAL();
                db.CommandText = "SELECT email_id,email_title FROM system_emails WHERE is_business_template='true' AND is_active='true' ";
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

        
        public clsBusinessEmail LoadBusinessEmail(clsMultipliIds IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.email_token));
                db.Parameters("Business_list_id", clsCrypto.Decrypt(IdObj.Business_token));

                db.CommandText = "SELECT * FROM v_system_emails WHERE email_id=@id ";
                DataTable dt = db.ConvertQueryToDataTable();
                clsBusinessEmail record = new clsBusinessEmail();
                foreach (DataRow row in dt.Rows)
                {
                    record.email_id = clsCrypto.Encrypt(DBLogic.DBString(row["email_id"]));
                    record.email_header = DBLogic.DBString(row["email_header"]);
                    record.email_body = DBLogic.DBString(row["email_body"]);
                }
                db.Parameters("account_id", clsCrypto.Decrypt(IdObj.account_token));
                db.CommandText = "SELECT account_id, member_email,first_name,last_name FROM meet_the_team WHERE account_id=@account_id";
                DataTable dtUser = db.ConvertQueryToDataTable();
              
                db.CommandText = "SELECT business_email FROM business_list WHERE Business_list_id=@Business_list_id";
                string email_to = db.ExecuteScalar().ToString();
                db.CommandText = "SELECT coupon FROM listing_coupon_bank WHERE owner_id=@account_id";
                string promo_code = db.ExecuteScalar().ToString();
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE  setting_id=5";
                string default_booking_fee = db.ExecuteScalar().ToString();
                

                db.CommandText = "SELECT first_name +' '+ last_name AS NAME FROM business_list WHERE Business_list_id=@Business_list_id";
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

     
        public bool? BusinessNameExists(clsStr str)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("business_name", str.str.Trim());
                db.CommandText = "SELECT TOP 1 1 FROM business_list WHERE business_name=@business_name";
                int count = DBLogic.DBInteger(db.ExecuteScalar());
                if(count > 0)
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
                status = KeyConstantsMsgs.error;
                return null;
            }
        }


        public string LoadBusinessNotes(clsId IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.id));
                db.CommandText = "SELECT notes FROM business_list WHERE Business_list_id=@id";
                string notes = DBLogic.DBString(db.ExecuteScalar());
                status = KeyConstantsMsgs.success;
                return notes;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                return "";
            }
        }



        public void UpdateBusinessNotes(clsNotes record)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Clear();
                db.Parameters("business_list_id", clsCrypto.Decrypt(record.id));
                db.Parameters("notes", record.notes.Trim());
                db.CommandText = "sp_update_business_notes";
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





        public clsBusinessListRecrod LoadSingleBusinessRecord(clsId IdObj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("id", clsCrypto.Decrypt(IdObj.id));
                db.CommandText = "SELECT * FROM v_Business_list WHERE Business_list_id=@id";
                DataTable dt = db.ConvertQueryToDataTable();
                clsBusinessListRecrod record = new clsBusinessListRecrod();
                foreach (DataRow row in dt.Rows)
                {
                    record.Business_list_id = clsCrypto.Encrypt(DBLogic.DBString(row["Business_list_id"]));
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


        public List<clsBusinessListRecrod> LoadBusinessList(clsSearhObj SearchParameters)
        {
            try
            {
                DAL db = new DAL();
                clsCommon common = new clsCommon();
                string where_clause = common.ConstructMarketingWhereClause(SearchParameters, db);
                db.CommandText = "SELECT * FROM v_Business_list " + where_clause + " ORDER BY date_created DESC";
                DataTable dt = db.ConvertQueryToDataTable();

                clsAddressParser ParseSearchParameter = new clsAddressParser();
                List<clsBusinessListRecrod> records = new List<clsBusinessListRecrod>();
                int count = 1;
                foreach(DataRow row in dt.Rows)
                {
                    clsBusinessListRecrod record = new clsBusinessListRecrod()
                    {
                        index = count,
                        id = DBLogic.DBString(row["Business_list_id"]),
                        Business_list_id = clsCrypto.Encrypt(DBLogic.DBString(row["Business_list_id"])),
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
                        has_notes = Convert.ToBoolean(row["has_notes"]),
                        contacted = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["has_been_contacted"])),
                        date_added = DBLogic.LongDateString(row["date_created"].ToString())
                    };
                    records.Add(record);
                    count = count + 1;
                }
                return records;

                //records = (from DataRow row in dt.Rows
                //           select new clsBusinessListRecrod()
                //           {
                //               Business_list_id = clsCrypto.Encrypt(DBLogic.DBString(row["Business_list_id"])),
                //               first_name = DBLogic.DBString(row["first_name"]),
                //               last_name = DBLogic.DBString(row["last_name"]),
                //               business_phone = DBLogic.DBString(row["business_phone"]),
                //               business_name = DBLogic.DBString(row["business_name"]),
                //               business_email = DBLogic.DBString(row["business_email"]),
                //               business_address = DBLogic.DBString(row["business_address"]),
                //               business_url = DBLogic.DBString(row["business_website"]),
                //               business_city = DBLogic.DBString(row["business_city"]),
                //               business_state = DBLogic.DBString(row["business_state"]),
                //               business_zip = DBLogic.DBString(row["business_zip"]),
                //               created_by = DBLogic.DBString(row["created_by"]),
                //               contacted_bool = DBLogic.DBBool(row["has_been_contacted"]),
                //               contacted = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["has_been_contacted"])),
                //               date_added = DBLogic.LongDateString(row["date_created"].ToString())
                //           }).ToList();
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



  
        public void CreateNewBusinessRecord(clsBusinessListRecrod record)
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
                db.CommandText = "sp_create_new_Business_contact";
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
                    message = KeyConstantsMsgs.Business_record_created;
                }
                
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_creating_new_business_record;
            }
        }
        


        public void UpdateBusinessRecord(clsBusinessListRecrod record)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Clear();
                db.Parameters("Business_list_id", clsCrypto.Decrypt(record.Business_list_id));
                db.Parameters("first_name", record.first_name.Trim());
                db.Parameters("last_name", record.last_name.Trim());
                db.Parameters("business_name", record.business_name.Trim());
                db.Parameters("business_phone",DBLogic.FormatPhoneForDatabase(record.business_phone.Trim() ));
                db.Parameters("business_email", record.business_email.Trim());
                db.Parameters("business_address", record.business_address.Trim());
                db.Parameters("business_website", record.business_url.Trim());
                db.Parameters("business_city", record.business_city.Trim());
                db.Parameters("business_state", record.business_state.Trim());
                db.Parameters("business_zip", record.business_zip.Trim());
                db.CommandText = "sp_update_Business_contact";
                int result = Int32.Parse(db.ExecuteStoredProcedure());
                status = KeyConstantsMsgs.success;
                message = KeyConstantsMsgs.information_updated;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_updating_Business_list;
            }
        }



    }


}