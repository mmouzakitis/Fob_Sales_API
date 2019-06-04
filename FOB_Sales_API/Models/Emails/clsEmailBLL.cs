using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.KeyConstants;
using System.Collections.Generic;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Reflection;
using System;
using System.Reflection;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using FOB_Sales_API.Models.ApplicationWideSettings;

namespace FOB_Sales_API.Models.Emails
{

    public struct clsCustomTags
    {
        public string column_name;
        public string column_value;

        public clsCustomTags(string name, string value)
        {
            column_name = name;
            column_value = value;
        }
       
    }

    public class clsEmailBLL
    {

        private static string _send_grid_key { get; set; }
        private static string _twilio_account_sid { get; set; }
        private static string _twilio_phone_number { get; set; }
        private static string _twilio_auth_token { get; set; }
        
        private EmailAddress email_from { get; set; }
        private string email_from_title_name { get; set; }
        private List<EmailAddress> email_to = new List<EmailAddress>();
        public EmailAddress cc_email { get; set; }
        private string email_header { get; set; }
        private string email_body_html { get; set; }
        private string email_body_text { get; set; }
        private string txt_msg { get; set; }
        private string phone_number_to { get; set; }
        private bool _receive_txt_msg { get; set; }
        private string receiver_id { get; set; }
        private string account_id { get; set; }
        private string booking_id { get; set; }
        private bool test_email { get; set; } = false;
        private EmailType email_type { get; set; }
        private List<clsCustomTags> custom_tags = new List<clsCustomTags>();
        clsReflection emailTagReflection = new clsReflection(typeof(clsEmailTags));
        public string error_message { get; set; }

        public clsEmailBLL(EmailType _email_type, string _email_from, string _email_to, List<clsCustomTags> _custom_tag, string blank)
        {
            clsApplicationWideSettings settings = new clsApplicationWideSettings();
            _send_grid_key = settings.GetSetting(9);

            email_type = _email_type;
            account_id = "";
            booking_id = "";
            custom_tags = _custom_tag;
            email_from = new EmailAddress(_email_from, KeyConstants.KeyConstants.FishOnBooking);
            email_to.Add(new EmailAddress(_email_to, KeyConstants.KeyConstants.FishOnBooking));
            GetEmailTemplate();
            ReplaceEmailTags();
        }

        public clsEmailBLL(string _email_header, string _email_body, string from_name, string from_phone_number, string from_email, string to_email)
        {
            clsApplicationWideSettings settings = new clsApplicationWideSettings();
            _send_grid_key = settings.GetSetting(9);

            email_header = _email_header;
            string extra_info = string.Empty;
            //if (from_name.Length > 0)
            //{
            //    extra_info = "From Name: " + from_name + "<br>";
            //}
            if (from_phone_number.Length > 0)
            {
                extra_info += "Phone Number: " + from_phone_number + "<br>";
            }
            email_body_html = extra_info + _email_body;
            email_from = new EmailAddress { Email = from_email, Name = from_name };
            email_to.Add(new EmailAddress { Email = to_email, Name = to_email });
        }

        public clsEmailBLL(EmailType _email_type, string _account_id, string _booking_id, List<clsCustomTags> _custom_tag)
        {
            clsApplicationWideSettings settings = new clsApplicationWideSettings();
            _send_grid_key = settings.GetSetting(9);

            email_type = _email_type;
            account_id = _account_id;
            booking_id = _booking_id;
            custom_tags = _custom_tag;
            email_from = new EmailAddress(KeyConstants.KeyConstants.DoNotReplyEmail, KeyConstants.KeyConstants.FishOnBooking);
            GetEmailTemplate();
            ReplaceEmailTags();
        }

        //GET EMAIL TEMPLATE (HEADER AND BODY) 
        private void GetEmailTemplate()
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Add("email_template_id", Convert.ToString((int)email_type));
                db.CommandText = "SELECT * FROM system_emails WHERE email_id=@email_template_id ";
                DataTable dt = db.ConvertQueryToDataTable();
                email_header = dt.Rows[0]["email_header"].ToString();
                email_body_html = dt.Rows[0]["email_body"].ToString();
                txt_msg = dt.Rows[0]["txt_msg"].ToString();
            }
            catch (System.Exception ex)
            {
                string ss = ex.ToString();
            }
        }

        //SET YOUR OWN EMAIL HEADER
        public void GetEmailHeader(string _email_header)
        {
            email_header = _email_header;
        }

        //SET YOUR OWN EMAIL BODY
        public void GetEmailBody(string _email_body)
        {
            email_body_html = _email_body;
        }


        //REPLACE EMAIL TAGS IF A BOOKING ID OR A ACCOUNT ID IS PROVIDED
        private void ReplaceEmailTags()
        {
            try
            {
                DAL db = new DAL();
                clsEmailTags email_tags = new clsEmailTags();

                if (account_id != "")
                {
                    db.parameters.Add("account_id", account_id);
                    db.CommandText = "SELECT * FROM v_users WHERE account_id=@account_id ";
                    DataTable dt_users = db.ConvertQueryToDataTable();
                    if (dt_users.Rows.Count > 0)
                    {
                        email_tags.customer_display_name = dt_users.Rows[0]["display_name"].ToString();
                        email_tags.customer_first_name = dt_users.Rows[0]["first_name"].ToString();
                        email_tags.customer_last_name = dt_users.Rows[0]["last_name"].ToString();
                        email_tags.customer_phone_number = DBLogic.formatPhoneNumber(dt_users.Rows[0]["phone_number"].ToString(), "");
                        email_tags.customer_email = dt_users.Rows[0]["user_email"].ToString();
                        email_tags.receive_txt_msg = DBLogic.DBBool(dt_users.Rows[0]["receive_text_msg"]);
                        email_to.Add(new EmailAddress { Email = email_tags.customer_email, Name = email_tags.customer_first_name + " " + email_tags.customer_last_name });
                        phone_number_to = email_tags.customer_phone_number;
                    }
                }


                if (booking_id != "")
                {
                    db.parameters.Add("booking_id", booking_id);
                    db.CommandText = "SELECT * FROM v_user_info_by_booking WHERE booking_id=@booking_id ";
                    DataTable dt_booking_details = db.ConvertQueryToDataTable();
                    try
                    {
                        if (dt_booking_details.Rows.Count > 0)
                        {
                            email_tags.reservation_no = dt_booking_details.Rows[0]["reservation_no"].ToString();
                            email_tags.event_name = dt_booking_details.Rows[0]["listing_name"].ToString();
                            email_tags.customer_display_name = dt_booking_details.Rows[0]["display_name"].ToString();
                            email_tags.customer_first_name = dt_booking_details.Rows[0]["first_name"].ToString();
                            email_tags.customer_last_name = dt_booking_details.Rows[0]["last_name"].ToString();
                            email_tags.customer_phone_number = DBLogic.formatPhoneNumber(dt_booking_details.Rows[0]["phone_number"].ToString(), "");
                            email_tags.customer_email = dt_booking_details.Rows[0]["user_email"].ToString();
                            email_tags.total_guests = dt_booking_details.Rows[0]["no_of_guests"].ToString();
                            //email_tags.amount_charged_today = "$" + dt_booking_details.Rows[0]["booking_fee"].ToString();
                            email_tags.agreed_fee = "$" + dt_booking_details.Rows[0]["agreed_fee"].ToString();
                            email_tags.booking_fee = "$" + dt_booking_details.Rows[0]["booking_fee"].ToString();
                            email_tags.event_address = dt_booking_details.Rows[0]["listing_address"].ToString();
                            email_tags.event_city = dt_booking_details.Rows[0]["listing_city"].ToString();
                            email_tags.event_state = dt_booking_details.Rows[0]["listing_state_abbr"].ToString();
                            email_tags.event_zip = dt_booking_details.Rows[0]["listing_zip"].ToString();
                            email_tags.event_location = string.Format("{0}, {1}, {2}, {3}", email_tags.event_address, email_tags.event_city, email_tags.event_state, email_tags.event_zip);
                            email_tags.event_date = DBLogic.LongDateString(dt_booking_details.Rows[0]["event_date"].ToString());
                            email_tags.event_start_time = DBLogic.FormatTimeAMPM(dt_booking_details.Rows[0]["start_time"].ToString());
                            email_tags.event_end_time = DBLogic.FormatTimeAMPM(dt_booking_details.Rows[0]["end_time"].ToString());
                            email_tags.event_duration = DBLogic.DBHoursDifference(dt_booking_details.Rows[0]["end_time"].ToString(), dt_booking_details.Rows[0]["start_time"].ToString());
                            email_tags.contact_person = dt_booking_details.Rows[0]["contact_first_name"].ToString() + " " + dt_booking_details.Rows[0]["contact_last_name"].ToString();
                            email_tags.contact_email = dt_booking_details.Rows[0]["contact_email"].ToString();
                            email_tags.contact_phone = DBLogic.formatPhoneNumber(dt_booking_details.Rows[0]["contact_phone"].ToString(), "");
                            email_tags.date_of_booking = DBLogic.LongDateString(dt_booking_details.Rows[0]["date_created"].ToString());
                            email_tags.receive_txt_msg = DBLogic.DBBool(dt_booking_details.Rows[0]["receive_text_msg"]);

                            //email_tags.cancelation_policy = DBLogic.DBString(dt_booking_details.Rows[0]["cancelation_policy"].ToString());
                            email_tags.canceled_date = DBLogic.LongDateString(dt_booking_details.Rows[0]["canceled_date"].ToString());
                            email_tags.canceled_reason = DBLogic.DBString(dt_booking_details.Rows[0]["canceled_reason"].ToString());
                            email_tags.canceled_by = DBLogic.DBString(dt_booking_details.Rows[0]["canceled_by"].ToString());

                            phone_number_to = email_tags.customer_phone_number;
                            _receive_txt_msg = email_tags.receive_txt_msg;

                            if (email_type == EmailType.booking_confirmation_customer || email_type == EmailType.booking_cancelation_customer)
                            {
                                email_to.Add(new EmailAddress { Email = email_tags.customer_email, Name = email_tags.customer_first_name + " " + email_tags.customer_last_name });
                            }
                            if (email_type == EmailType.booking_confirmation_owner || email_type == EmailType.booking_cancelation_owner)
                            {
                                email_to.Add(new EmailAddress { Email = email_tags.contact_email, Name = email_tags.contact_person });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.ToString();
                    }
                }


                if (custom_tags != null)
                {
                    foreach (clsCustomTags custom_tag in custom_tags)
                    {
                        email_body_html = email_body_html.Replace(custom_tag.column_name, custom_tag.column_value);
                        txt_msg = txt_msg.Replace(custom_tag.column_name, custom_tag.column_value);
                        email_header = email_header.Replace(custom_tag.column_name, custom_tag.column_value);
                    }
                }


                foreach (string column in emailTagReflection.column_names)
                {
                    email_body_html = email_body_html.Replace("{" + column.ToString() + "}", GetPropertyValue(email_tags, column));
                    txt_msg = txt_msg.Replace("{" + column.ToString() + "}", GetPropertyValue(email_tags, column));
                    email_header = email_header.Replace("{" + column.ToString() + "}", GetPropertyValue(email_tags, column));
                }
            }
            catch (Exception ex)
            {
                string sss = ex.ToString();
            }
        }


        public string SendEmail()
        {
            if (email_from.Email == string.Empty || email_to.Count <= 0 || email_body_html == string.Empty || email_header == string.Empty)
            {
                return KeyConstantsMsgs.please_fill_in_all_the_fields;
            }

            var client = new SendGridClient(_send_grid_key);
            var msg = new SendGridMessage()
            {
                From = email_from, //new EmailAddress( email_obj.email_from_title_name),
                Subject = email_header,
                PlainTextContent = email_body_text,
                HtmlContent = email_body_html
            };
            foreach (EmailAddress email_to in email_to)
            {
                msg.AddTo(new EmailAddress(email_to.Email, email_to.Name));
            }
            if (cc_email != null)
            {
                msg.AddCc(cc_email.Email, cc_email.Name);

            }
            client.SendEmailAsync(msg);

            SendTextMessage();
            return KeyConstantsMsgs.success;
        }


        public void SendTextMessage()
        {
            if(_receive_txt_msg == true)
            {
                clsApplicationWideSettings settings = new clsApplicationWideSettings();
                _twilio_account_sid = settings.GetSetting(10);
                _twilio_auth_token = settings.GetSetting(11);
                _twilio_phone_number = settings.GetSetting(12);

                try
                {
                    TwilioClient.Init(_twilio_account_sid, _twilio_auth_token);
                    var message1 = MessageResource.Create(
                        body: txt_msg,
                        from: new Twilio.Types.PhoneNumber(_twilio_phone_number),
                        to: new Twilio.Types.PhoneNumber("+1" + phone_number_to)
                    );
                }
                catch (Exception ex)
                {
                    string ss = ex.ToString();
                }
            }           
        }


        private string GetPropertyValue(object o, string p)
        {
            if (o.GetType().GetProperty(p).GetValue(o, null) == null)
            {
                return "null";
            }
            return o.GetType().GetProperty(p).GetValue(o, null).ToString();
        }



    }
}