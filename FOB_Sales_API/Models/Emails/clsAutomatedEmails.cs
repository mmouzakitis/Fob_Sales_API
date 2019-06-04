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
using FOB_Sales_API.Models.Emails;

namespace FOB_Sales_API.Models.AutomatedEmails
{
    public class clsAutomatedEmails
    {
        public void UpCommingBookings()
        {
            DAL db = new DAL();
            //db.parameters.Add("email_template_id", email_type.ToString());
            db.CommandText = "SELECT * FROM automated_emails_settings ";
            DataTable dt_parameters = db.ConvertQueryToDataTable();
            //clsEmailBLL sendEmail = new clsEmailBLL();

            //    email.email_to.Add(GetEmailTo(account_id));
            //    GetEmailTemplate(email_type);
            //    ReplaceEmailTags(account_id, listing_id,booking_id,custom_value);
            //    SendEmail(email);

            foreach (DataRow row in dt_parameters.Rows)
            {
                int email_id = DBLogic.DBInteger(row["email_id"]);
                int days = DBLogic.DBInteger(row["reminder_in_dates"]);

                //clsEmailBLL EmailBll = new clsEmailBLL();
                //clsEmailTemplate EmailTemplate = new clsEmailTemplate(email_type);mouzakitis
               
                db.parameters.Clear();
                DateTime event_date = DateTime.Now.AddDays(days);
                db.parameters.Add("event_date", event_date.ToShortDateString().ToString());
                db.CommandText = "SELECT * FROM v_booking_reminders WHERE booking_canceled='false' AND event_date=@event_date";
                DataTable dt_bookings = db.ConvertQueryToDataTable();
                foreach (DataRow row_bookings in dt_bookings.Rows)
                {
                    //EmailBll.ReplaceEmailTags(EmailTemplate, account_id, listing_id, booking_id, days);mouzakitis
                   // sendEmail.SendEmail(EmailTemplate);mouzakitis
                }

            }
        }
    }
}