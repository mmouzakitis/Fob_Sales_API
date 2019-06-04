using System;
using System.Collections.Generic;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.KeyConstants;
using System.Data;
using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.Interfaces;
using FOB_Sales_API.Models.ApplicationWideSettings;
using FOB_Sales_API.Models.ErrorLogger;
using FOB_Sales_API.Models.Booking;

namespace FOB_Sales_API.Models.Booking
{
    public class clsBookingBLL : IFoBMessages
    {
        public string status { get; set; }
        public string message { get; set; }
        public const string could_not_complete = "Could not complete booking";
        public const string cannot_cancel_past_booking = "Cannot cancel booking that is in the past";
        public const string could_not_complete_booking = "Could not complete booking, please try again or contact us through our Contact Us Page";
        public const string booking_error_booking_taken = "So sorry, it looks like someone managed to book this slot just before you did. Cannot complete booking";
        public const string booking_error_no_of_guests = "Please provide a number of guests 1 or greater";
        public const string booking_error_slot_doesnt_exist = "Can not complete booking, It looks like the captain changed the slot times, please close this window and book another time";
        public const string booking_error_date_is_blocked = "Can not complete booking, the date you have selected has just been booked by someone else. Please close this window and book another date.";
        public const string booking_error_date_in_the_past = "Can not complete booking, the date you are trying to book is in the past";
        public const string booking_error_data_not_valid = "So sorry, it looks like someone managed to book this slot just before you did. Cannot complete booking";

        public void EmailBookingDetails(string booking_id)
        {

            bool email_sent = false;

            bool booking_status = GetBookingStatus(booking_id);
            if (booking_status == true)
            {
                clsEmailBLL SendEmail = new clsEmailBLL(EmailType.booking_reminder, string.Empty, clsCrypto.Decrypt(booking_id), null);
                SendEmail.SendEmail();
                email_sent = true;
            }
            if (email_sent == true)
            {
                status = KeyConstantsMsgs.success;
                message = KeyConstantsMsgs.email_sent;
            }
            else
            {
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_sending_email;
            }

        }



        public bool GetBookingStatus(string booking_id)
        {
            DAL db = new DAL();
            db.Parameters("booking_id", clsCrypto.Decrypt(booking_id));
            db.CommandText = "SELECT [booking_active] FROM Listing_bookings WHERE booking_id=@booking_id ";
            bool status = DBLogic.DBBool(db.ExecuteScalar());
            return status;
        }



        public clsCompleteBookingByCaptain CompleteBookingByCaptain(clsCompleteBookingByCaptain booking)
        {
            clsBookingDetails record = new clsBookingDetails();
            try
            {
                DAL db = new DAL();
                db.Parameters("slot_id", clsCrypto.Decrypt(booking.s_token));
                db.Parameters("listing_id", clsCrypto.Decrypt(booking.l_token));
                db.Parameters("event_date", booking.event_date);
                db.Parameters("no_of_guests", booking.no_of_guests);
                db.Parameters("final_price", booking.price);
                db.Parameters("first_name", booking.f_name.ToString());
                db.Parameters("last_name", booking.l_name.ToString());
                db.Parameters("email", booking.email.ToString());
                db.Parameters("phone_number", booking.phone_no.ToString());
                db.CommandText = "sp_create_booking_by_captain";
                booking.booking_confirmation_no = db.ExecuteStoredProcedure();
                if (booking.booking_confirmation_no == "-100")
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = could_not_complete;
                }
                if (booking.booking_confirmation_no == "-35")//slot doesn't exist anymore
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_slot_doesnt_exist;
                    return booking;
                }
                else if (booking.booking_confirmation_no == "-10")//selected date is blocked
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_date_is_blocked;
                    return booking;
                }
                else if (booking.booking_confirmation_no == "-20")//cannot book a date in the past
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_date_in_the_past;
                    return booking;
                }
                else if (booking.booking_confirmation_no == "-25")//data entered is not valid
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_data_not_valid;
                    return booking;
                }
                else if (booking.booking_confirmation_no == "-30")//booking already exists
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_booking_taken;
                    return booking;
                }
                else if (booking.booking_confirmation_no == "-45")//booking already exists
                {
                    booking.booking_succeeded = false;
                    booking.booking_message = booking_error_no_of_guests;
                    return booking;
                }
                else
                {
                    booking.booking_succeeded = true;
                    booking.booking_message = KeyConstantsMsgs.record_created_successfully;
                    //CalculateAvailability(booking.booking_confirmation_no);
                    return booking;
                }
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                booking.booking_confirmation_no = "-1";
                booking.booking_succeeded = false;
                booking.booking_message = could_not_complete_booking;
                return booking;
            }
        }


        //public void SendAzureQueueMessageTest()
        //{
        //    string id = "1258791";
        //    clsAzureMessaging.SendAzureMessage(clsCrypto.Decrypt(id));
        //}


        //THIS FUNCTION IS USED WHEN A USER IS IN THE PROCESS OF BOOKING AND WE NEED TO DISPLAY THE INFORMATION TO HIM.clsPreBookingSearch
        public clsBookingDetails LoadPreBookingDetails(clsPreBookingSearch booking_details)//clPreBookObj
        {
            clsBookingDetails record = new clsBookingDetails();
            try
            {
                DAL db = new DAL();
                db.Parameters("slot_id", clsCrypto.Decrypt(booking_details.s_id));
                db.CommandText = "SELECT * FROM v_prebooking_details WHERE slot_id=@slot_id AND listing_active='true' ";
                DataTable dt = db.ConvertQueryToDataTable();
                if (dt.Rows.Count == 1)
                {

                    record.s_id = clsCrypto.Encrypt(dt.Rows[0]["slot_id"].ToString());
                    record.l_id = clsCrypto.Encrypt(dt.Rows[0]["listing_id"].ToString());
                    record.start_time = DBLogic.FormatTimeAMPM(DBLogic.DBString(dt.Rows[0]["start_time"]));
                    record.end_time = DBLogic.FormatTimeAMPM(DBLogic.DBString(dt.Rows[0]["end_time"]));
                    record.listing_name = DBLogic.DBString(dt.Rows[0]["listing_name"]);
                    record.listing_location = DBLogic.DBString(dt.Rows[0]["listing_address"]) + ", " + DBLogic.DBString(dt.Rows[0]["listing_city"]) + ", " + DBLogic.DBString(dt.Rows[0]["listing_state_abbr"]);
                    record.boat_length = DBLogic.DBString(dt.Rows[0]["boat_length"]);
                    record.max_capacity = DBLogic.DBInteger(dt.Rows[0]["max_capacity"]);
                    record.event_date = booking_details.event_date;
                    record.event_date_long = DBLogic.LongDateString(record.event_date);
                    record.trip_duration = GetDuration(record.start_time, record.end_time);
                    record.no_of_guests = booking_details.no_of_guests;
                    record.base_price = DBLogic.DBDouble(dt.Rows[0]["slot_price"]);
                    record.base_price = GetSeasonalPrice(record.s_id, record.event_date, record.base_price);
                    record.promo_transactions_remaining = DBLogic.DBInteger(dt.Rows[0]["promo_transactions_remaining"]);
                    record.booking_fee_rate = DBLogic.DBInteger(dt.Rows[0]["booking_fee_rate"]);
                    if (record.promo_transactions_remaining > 0)
                    {
                        clsApplicationWideSettings app_settings = new clsApplicationWideSettings();
                        record.booking_fee_rate = Int32.Parse(app_settings.GetSetting(6));
                    }
                    if (record.booking_fee_rate == 0)
                    {
                        record.booking_fee_rate = KeyConstants.KeyConstants.default_fee_rate;
                    }
                    record.booking_fee = record.base_price * (record.booking_fee_rate * 0.01); //DBLogic.AggreedFee(record.base_price, record.base_guests, record.cost_per_extra_guest, pre_booking.no_of_guests) * (record.booking_fee_rate * 0.01);
                    // record.total_booking_cost = 0;// record.agreed_fee + record.booking_fee;
                    record.base_price_currency = DBLogic.ConvertToCurrency(record.base_price).Replace("$", "");
                    record.booking_fee_currency = DBLogic.ConvertToCurrency(record.booking_fee).Replace("$", "");
                    //record.total_booking_cost_currency = DBLogic.ConvertToCurrency(record.total_booking_cost).Replace("$", "");
                    record.agreed_fee = record.base_price - record.booking_fee;
                    record.agreed_fee_currency = DBLogic.ConvertToCurrency(record.agreed_fee).Replace("$", "");

                    record.c_name = DBLogic.DBString(dt.Rows[0]["contact_first_name"]) + ' ' + DBLogic.DBString(dt.Rows[0]["contact_last_name"]);
                    record.c_email = DBLogic.DBString(dt.Rows[0]["contact_email"]);
                    record.c_phone = DBLogic.formatPhoneNumber(dt.Rows[0]["contact_phone"].ToString(), "");
                }

                return record;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }



        public void SendEmailNotificationBookingCompleted(string booking_id)
        {
            try
            {
                clsEmailBLL SendEmailToCustomer = new clsEmailBLL(EmailType.booking_confirmation_customer, string.Empty, booking_id, null);
                SendEmailToCustomer.SendEmail();
            }
            catch (Exception ex)
            {
                clsErrors error = new clsErrors(-36, ex.ToString(), booking_id, KeyConstants.KeyConstants.booking);
                string ss = ex.ToString();
            }

            try
            {
                clsEmailBLL SendEmailToOwner = new clsEmailBLL(EmailType.booking_confirmation_owner, string.Empty, booking_id, null);
                SendEmailToOwner.SendEmail();
            }
            catch (Exception ex)
            {
                clsErrors error = new clsErrors(-37, ex.ToString(), booking_id, KeyConstants.KeyConstants.booking);
                string ss = ex.ToString();
            }
        }

        public void UpdateBookingWithTransactionId(clsCompletedBooking booking_obj)
        {
            try
            {
                DAL db = new DAL();
                db.Parameters("confirmation_no", booking_obj.booking_confirmation_no.ToString());
                db.Parameters("credit_trans_id", booking_obj.credit_card_transaction_id);
                db.Parameters("authorization_code", booking_obj.authorization_code);
                db.Parameters("listing_id", clsCrypto.Decrypt(booking_obj.l_token));
                db.Parameters("event_date", booking_obj.event_date);
                db.Parameters("slot_id", clsCrypto.Decrypt(booking_obj.s_token));
                db.Parameters("first_name_on_card", booking_obj.first_name_on_card);
                db.Parameters("last_name_on_card", booking_obj.last_name_on_card);
                db.Parameters("time_zone_minutes", booking_obj.time_offset_greenwich.ToString());
                db.CommandText = "sp_update_booking_with_credit_trans_id";
                var val = db.ExecuteStoredProcedure();
            }
            catch (Exception ex)
            {
                string ss = string.Empty;
                clsErrors error = new clsErrors(-50, ex.ToString(), booking_obj.l_token, KeyConstants.KeyConstants.listing);
            }
        }

      
        public decimal GetBookingAmountCharged(int confirmation_no)
        {
            DAL db = new DAL();
            db.Parameters("confirmation_no", confirmation_no.ToString());
            db.CommandText = "SELECT booking_fee FROM listing_bookings WHERE booking_id=@confirmation_no";
            decimal amount = decimal.Parse(db.ExecuteScalar().ToString());
            return amount;
        }

        public void BlockDateFromCalendar(string booking_id)
        {
            bool block_date = false;
            try
            {
                DAL db = new DAL();
                db.Parameters("booking_id", booking_id);
                db.CommandText = "SELECT listing_id FROM v_booking_ids WHERE booking_id=@booking_id";
                string listing_id = (string)db.ExecuteScalar();
                db.CommandText = "SELECT event_date FROM v_booking_ids WHERE booking_id=@booking_id";
                string event_date = (string)db.ExecuteScalar();

                db.Parameters("listing_id", listing_id);
                db.Parameters("event_date", event_date);
                db.CommandText = "SELECT start_time,end_time FROM listing_slots WHERE listing_id=@listing_id";
                DataTable dtSlots = db.ConvertQueryToDataTable();
                db.CommandText = "SELECT start_time,end_time FROM listing_bookings WHERE listing_id=@listing_id AND event_date=@event_date ";
                DataTable dtBookedTimes = db.ConvertQueryToDataTable();
                int no_of_slots = dtSlots.Rows.Count;
                int index = 0;
                foreach (DataRow rowBooked in dtBookedTimes.Rows)
                {
                    foreach (DataRow rowSlots in dtSlots.Rows)
                    {
                        bool is_available = IsBookingAvailable(rowSlots["start_time"].ToString(), rowSlots["end_time"].ToString(), rowBooked["start_time"].ToString(), rowBooked["end_time"].ToString());
                        if (is_available == false)
                        {
                            index += 1;
                        }
                    }
                }
                //if index equals or is more then the number of slots that means there is no more room for bookins because dates overlap or
                //are booked
                if (index >= no_of_slots)
                {
                    block_date = true;
                }
                db.CommandText = "sp_block_date_if_date_is_fully_booked";
                string ss = db.ExecuteStoredProcedure();
                bool tt = block_date;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
            }
        }


        public void BlockDateFromCalendar(string listing_id, string event_date)
        {
            bool block_date = false;
            try
            {
                DAL db = new DAL();
                db.Parameters("listing_id", clsCrypto.Decrypt(listing_id));
                db.Parameters("event_date", event_date);
                db.CommandText = "SELECT start_time,end_time FROM listing_slots WHERE listing_id=@listing_id";
                DataTable dtSlots = db.ConvertQueryToDataTable();
                db.CommandText = "SELECT start_time,end_time FROM listing_bookings WHERE listing_id=@listing_id AND event_date=@event_date ";
                DataTable dtBookedTimes = db.ConvertQueryToDataTable();
                int slot_count = dtSlots.Rows.Count;
                int internal_count = 0;
                foreach (DataRow rowBooked in dtBookedTimes.Rows)
                {
                    foreach (DataRow rowSlots in dtSlots.Rows)
                    {
                        bool is_available = IsBookingAvailable(rowSlots["start_time"].ToString(), rowSlots["end_time"].ToString(), rowBooked["start_time"].ToString(), rowBooked["end_time"].ToString());
                        if (is_available == false)
                        {
                            internal_count += 1;
                        }
                    }
                }
                if (internal_count >= slot_count)
                {
                    block_date = true;
                }
                db.CommandText = "sp_block_date_if_date_is_fully_booked";
                string ss = db.ExecuteStoredProcedure();
                bool tt = block_date;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                clsErrors error = new clsErrors(-41, ex.ToString(), listing_id, KeyConstants.KeyConstants.listing);
            }
        }

       

        public double GetSeasonalPrice(string slot_id, string event_date, double base_price)
        {
            DateTime startDate = DateTime.Parse(event_date);
            startDate = new DateTime(2000, startDate.Month, startDate.Day);
            try
            {
                DAL db = new DAL();
                db.Parameters("slot_id", clsCrypto.Decrypt(slot_id));
                db.Parameters("event_date", event_date);
                db.CommandText = "SELECT price FROM listing_slots_seasonal WHERE slot_id=@slot_id AND @event_date>=start_date AND @event_date<=end_date ";
                string price = DBLogic.DBString(db.ExecuteScalar());
                if (price == string.Empty)
                {
                    return base_price;
                }
                else
                {
                    return Convert.ToDouble(price);
                }
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                clsErrors error = new clsErrors(-42, ex.ToString(), slot_id, KeyConstants.KeyConstants.slot);
                return base_price;
            }
        }



        public clsBookingDetails LoadBookingDetails(string booking_id)
        {
            clsBookingDetails record = new clsBookingDetails();
            try
            {
                DAL db = new DAL();
                db.Parameters("booking_id", clsCrypto.Decrypt(booking_id));
                db.CommandText = "SELECT * FROM v_booking_details WHERE booking_id=@booking_id ";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    record.b_id = clsCrypto.Encrypt(row["booking_id"].ToString());
                    record.l_id = clsCrypto.Encrypt(row["listing_id"].ToString());
                    record.acc_id = clsCrypto.Encrypt(row["account_id"].ToString());

                    record.customers_name = DBLogic.DBString(row["first_name"]) + ' ' + DBLogic.DBString(row["last_name"]);
                    record.listing_name = DBLogic.DBString(row["listing_name"]);
                    record.customer_email = DBLogic.DBString(row["email"]);
                    record.customer_phone = DBLogic.DBString(row["phone_no"]);

                    record.listing_location = DBLogic.DBString(row["listing_address"]) + ", " + DBLogic.DBString(row["listing_city"]) + ", " + DBLogic.DBString(row["state"]);
                    record.event_active = DBLogic.DBBool(row["booking_active"]);
                    record.canceled_date = DBLogic.DBString(row["canceled_date"]);
                    //record.cancellation_cut_off_days = 0;// DBLogic.DBInteger(row["cancellation_cut_off_days"]);
                    record.can_cancel = true;
                    record.date_of_booking = DBLogic.LongDateString(DBLogic.DBString(row["date_created"]));
                    if (record.event_active == true)
                    {
                        record.booking_status_text = "Upcoming";
                        if (DBLogic.DBDate(record.date_of_booking) < DateTime.Now)
                        {
                            record.booking_status_text = "Completed";
                            record.can_cancel = false;
                        }
                    }
                    else
                    {
                        record.can_cancel = false;
                        record.booking_status_text = "Canceled";
                    }
                    record.boat_length = DBLogic.DBString(row["boat_length"]);
                    record.max_capacity = DBLogic.DBInteger(row["max_capacity"]);
                    record.category = DBLogic.DBString(row["listing_category"]);
                    record.event_date = DBLogic.DBString(row["event_date"]);
                    if (DBLogic.DBDate(row["event_date"].ToString()) < DateTime.Today)
                    {
                        record.booking_status_text = "Completed";
                        record.past_booking = true;
                    }
                    else
                    {
                        record.booking_status_text = "Upcoming";
                    }
                    record.event_date_long = DBLogic.LongDateString(DBLogic.DBString(row["event_date"]));
                    record.start_time = DBLogic.FormatTimeAMPM(DBLogic.DBString(row["start_time"]));
                    record.end_time = DBLogic.FormatTimeAMPM(DBLogic.DBString(row["end_time"]));
                    record.trip_duration = GetDuration(record.start_time, record.end_time);
                    record.no_of_guests = DBLogic.DBInteger(row["no_of_guests"]);
                    record.agreed_fee = DBLogic.DBDouble(row["agreed_fee"]);
                    record.booking_fee = DBLogic.DBDouble(row["booking_fee"]);
                    //record.base_price = DBLogic.DBDouble(row["base_price"]);
                    //record.cost_per_extra_guest_currency = DBLogic.ConvertToCurrency(record.cost_per_extra_guest).Replace("$", "");
                    // record.agreed_fee = DBLogic.AggreedFee(record.base_price, record.base_guests, record.cost_per_extra_guest, pre_booking.no_of_guests) * 0.97;
                    //record.booking_fee = DBLogic.AggreedFee(record.base_price, record.base_guests, record.cost_per_extra_guest, pre_booking.no_of_guests) * 0.03;
                    //record.total_booking_cost = record.agreed_fee + record.booking_fee;
                    record.agreed_fee_currency = DBLogic.ConvertToCurrency(record.agreed_fee).Replace("$", "");
                    record.booking_fee_currency = DBLogic.ConvertToCurrency(record.booking_fee).Replace("$", "");
                    //record.total_booking_cost_currency = DBLogic.ConvertToCurrency(record.total_booking_cost).Replace("$", "");

                    record.c_name = DBLogic.DBString(row["contact_first_name"]) + ' ' + DBLogic.DBString(row["contact_last_name"]);
                    record.c_email = DBLogic.DBString(row["contact_email"]);
                    record.c_phone = DBLogic.formatPhoneNumber(row["contact_phone"].ToString(), "");
                    record.c_id = clsCrypto.Encrypt(row["contact_id"].ToString());
                    record.canceled_date = DBLogic.LongDateString(DBLogic.DBString(row["canceled_date"]));
                    record.canceled_reason = DBLogic.DBString(row["canceled_reason"]);
                    record.canceled_by_lookup = DBLogic.DBString(row["canceled_by_lookup"]);
                    record.canceled_by_lookup_id = DBLogic.DBInteger(row["canceled_by_lookup_id"]);
                    //record.cancelation_cut_off_date = DBLogic.LongDateString(CancelationCutOffDate(record.event_date, record.cancellation_cut_off_days));
                    //record.entitled_to_refund = EntitledToRefund(record.canceled_date, record.cancelation_cut_off_date, record.canceled_by_lookup_id);
                    // records.Add(record);
                }
                return record;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                clsErrors error = new clsErrors(-44, ex.ToString(), booking_id, KeyConstants.KeyConstants.booking);
                return null;
            }
        }



        public List<clsCalendarBooking> GetCalendarBookings(clsSearchCalendar search_calendar)
        {
            List<clsCalendarBooking> records = new List<clsCalendarBooking>();
            DAL db = new DAL();
            try
            {
                db.Parameters("listing_id", clsCrypto.Decrypt(search_calendar.l_id));
                db.Parameters("start_date", search_calendar.start_date);
                db.Parameters("end_date", search_calendar.end_date);
                db.CommandText = "SELECT booking_id,first_name,last_name,event_date,start_time,end_time,booking_active FROM v_bookings_for_calendar WHERE listing_id=@listing_id AND event_date >=@start_date AND event_date<=@end_date ";
                DataTable dt = db.ConvertQueryToDataTable();

                db.CommandText = "SELECT blocked_dates_id,event_date,title FROM v_blocked_dates WHERE listing_id=@listing_id and blocked_type= 1 ";
                DataTable dt_blocked_dates = db.ConvertQueryToDataTable();

                foreach (DataRow row in dt.Rows)
                {
                    clsCalendarBooking record = new clsCalendarBooking();
                    record.id = clsCrypto.Encrypt(row["booking_id"].ToString());
                    record.is_blocked_by_captain = false;
                    record.title = DBLogic.DBString(row["first_name"].ToString()) + ' ' + DBLogic.DBString(row["last_name"].ToString());
                    record.event_active = DBLogic.DBBool(row["booking_active"]);
                    if (record.event_active == false)
                    {
                        record.backgroundColor = "#ff0000";
                    }
                    else
                    {
                        record.backgroundColor = "#ff0000";
                    }
                    record.start = FormatDateTimeForCalendar(row["event_date"].ToString(), row["start_time"].ToString());
                    record.end = FormatDateTimeForCalendar(row["event_date"].ToString(), row["end_time"].ToString());
                    records.Add(record);
                }

                foreach (DataRow row in dt_blocked_dates.Rows)
                {
                    clsCalendarBooking record = new clsCalendarBooking();
                    record.id = clsCrypto.Encrypt(row["blocked_dates_id"].ToString());
                    record.is_blocked_by_captain = true;
                    record.title = DBLogic.DBString(row["title"].ToString());
                    record.event_active = false;
                    if (record.event_active == false)
                    {
                        record.backgroundColor = "#ff0000";
                    }
                    else
                    {
                        record.backgroundColor = "#ff0000";
                    }
                    record.start = DBLogic.DBString(row["event_date"].ToString());
                    record.end = DBLogic.DBString(row["event_date"].ToString());
                    records.Add(record);
                }

                return records;
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                clsErrors error = new clsErrors(-45, ex.ToString(), search_calendar.l_id, KeyConstants.KeyConstants.listing);
                return null;
            }
        }


        public List<clsBookingTotals> AllBookingsByDate(clsBookingStatsSearch SearchObj)
        {
            List<clsBookingTotals> records = new List<clsBookingTotals>();
            try
            {
                DAL db = new DAL();
                db.Parameters("event_date", SearchObj.event_date);
                db.CommandText = "SELECT listing_name,listing_city,listing_state_abbr,event_date,no_of_guests FROM v_booking_details_small WHERE event_date=@event_date AND booking_active='true' ORDER BY event_date DESC";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsBookingTotals record = new clsBookingTotals();
                    record.count = 1;
                    record.event_date = DBLogic.LongDateString(DBLogic.DBString(row["event_date"]));
                    record.listing_name = DBLogic.DBString(row["listing_name"]);
                    record.listing_city = DBLogic.DBString(row["listing_city"]);
                    record.listing_state = DBLogic.DBString(row["listing_state_abbr"]);
                    record.listing_guests = DBLogic.DBString(row["no_of_guests"]);
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


        public List<clsBookingTotals> AllBookingsByMonth(clsBookingStatsSearch SearchObj)
        {
            List<clsBookingTotals> records = new List<clsBookingTotals>();
            try
            {
                string start_date = SearchObj.GetFirstDayOfMonth();
                string end_date = SearchObj.GetLastDayOfMonth();
                string event_date = SearchObj.event_date;
                DAL db = new DAL();
                db.Parameters("start_date", start_date);
                db.Parameters("end_date", end_date);
                db.CommandText = "SELECT event_date, COUNT(*) AS count FROM listing_bookings WHERE event_date>=@start_date AND event_date<=@end_date AND booking_active='true' GROUP BY event_date ORDER BY event_date DESC";
                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsBookingTotals record = new clsBookingTotals();
                    record.count = DBLogic.DBInteger(DBLogic.DBString(row["count"]));
                    record.event_date = DBLogic.ShortDateString(row["event_date"].ToString());
                    record.event_date_long = DBLogic.LongDateString(DBLogic.DBString(row["event_date"]));
                    record.year = SearchObj.year;
                    record.month = SearchObj.month;
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



        public List<clsBooking> LoadBookingsByAccountId(string account_id, string booking_type)
        {
            List<clsBooking> records = new List<clsBooking>();
            try
            {
                DAL db = new DAL();
                db.Parameters("account_id", clsCrypto.Decrypt(account_id));

                if (booking_type == KeyConstants.KeyConstants.completed)
                {
                    db.CommandText = "SELECT * FROM v_bookings WHERE account_id=@account_id AND event_date < getdate() ORDER BY event_date DESC";
                }
                if (booking_type == KeyConstants.KeyConstants.not_completed)
                {
                    db.CommandText = "SELECT * FROM v_bookings WHERE account_id=@account_id AND event_date >= getdate() ORDER BY event_date DESC";
                }

                DataTable dt = db.ConvertQueryToDataTable();
                foreach (DataRow row in dt.Rows)
                {
                    clsBooking record = new clsBooking();
                    //record.error_message = string.Empty;
                    record.b_id = clsCrypto.Encrypt(row["booking_id"].ToString());
                    record.acc_id = clsCrypto.Encrypt(row["account_id"].ToString());
                    record.l_id = clsCrypto.Encrypt(row["listing_id"].ToString());
                    record.date_of_booking = DBLogic.LongDateString(DBLogic.DBString(row["date_created"]));
                    record.event_date = DBLogic.LongDateString(DBLogic.DBString(row["event_date"]));
                    record.start_time = DBLogic.DBShortTime(DBLogic.DBString(row["start_time"]));
                    record.end_time = DBLogic.DBShortTime(DBLogic.DBString(row["end_time"]));
                    record.no_of_guests = DBLogic.DBInteger(row["no_of_guests"]);
                    record.agreed_fee = DBLogic.DBDouble(row["agreed_fee"]);
                    record.agreed_fee_currency = DBLogic.ConvertToCurrency(record.agreed_fee).Replace("$", "");
                    record.booking_fee = DBLogic.DBDouble(row["booking_fee"]);
                    record.booking_fee_currency = DBLogic.ConvertToCurrency(record.booking_fee).Replace("$", "");
                    record.max_capacity = DBLogic.DBDouble(row["max_capacity"]);
                    record.listing_name = DBLogic.DBString(row["listing_name"]);
                    record.location = DBLogic.DBString(row["listing_address"]) + ' ' + DBLogic.DBString(row["listing_city"]) + ' ' + DBLogic.DBString(row["listing_state_abbr"]);
                    record.past_booking = false;
                    if (DBLogic.DBDate(row["event_date"].ToString()) < DateTime.Today)
                    {
                        record.booking_status_text = "Completed";
                        record.past_booking = true;
                    }
                    else
                    {
                        record.booking_status_text = "Upcoming";
                    }

                    if (DBLogic.DBBool(row["booking_active"]) == false)
                    {
                        record.booking_status_text = "Canceled";
                    }
                    // record.booking_status_text = DBLogic.ConvertBoolToYesNo(DBLogic.DBBool(row["booking_active"]));

                    record.booking_active = DBLogic.DBBool(row["booking_active"]);
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


        public List<clsBooking> LoadBookingsBySearch(clsSearchBookings search)
        {
            try
            {
                List<clsBooking> booking_slots = new List<clsBooking>();
                DAL db = new DAL();
                string where = string.Empty;
                if (search.event_date.Length > 0)
                {
                    db.Parameters("event_date", search.event_date);
                    where = "event_date=@event_date ";
                }
                if (search.last_name.Length > 0)
                {
                    db.Parameters("last_name", search.last_name);
                    if (where.Length > 0)
                    {
                        where = where + " OR ";
                    }
                    where = where + " last_name LIKE @last_name + '%'";
                }
                if (search.reservation_no.Length >= 2)
                {
                    db.Parameters("reservation_no", search.reservation_no);
                    if (where.Length > 0)
                    {
                        where = where + " OR ";
                    }
                    where = where + " booking_id=@reservation_no";
                }
                if (where.Length < 10)
                {
                    return null;
                }
                db.CommandText = "SELECT * FROM v_bookings WHERE " + where;
                DataTable dt = db.ConvertQueryToDataTable();

                int count = 0;
                foreach (DataRow rowBookings in dt.Rows)
                {
                    count = +1;
                    clsBooking record = new clsBooking();
                    record.index = count;
                    record.b_id = clsCrypto.Encrypt(rowBookings["booking_id"].ToString());
                    record.acc_id = clsCrypto.Encrypt(rowBookings["account_id"].ToString());
                    record.reservation_no = rowBookings["booking_id"].ToString();
                    record.date_of_booking = DBLogic.LongDateString(rowBookings["date_created"].ToString());
                    record.event_date = DBLogic.LongDateString(rowBookings["event_date"].ToString());
                    record.start_time = DBLogic.FormatTimeAMPM(rowBookings["start_time"].ToString());
                    record.end_time = DBLogic.FormatTimeAMPM(rowBookings["end_time"].ToString());
                    record.customers_name = rowBookings["first_name"].ToString() + ' ' + rowBookings["last_name"].ToString();
                    record.listing_name = rowBookings["listing_name"].ToString();
                    record.booking_active = DBLogic.DBBool(rowBookings["booking_active"]);
                    record.booking_status_text = DBLogic.ConvertBoolToActiveCanceled(DBLogic.DBBool(rowBookings["booking_active"]));
                    booking_slots.Add(record);
                }
                return booking_slots;
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }


        public void CancelBooking(clsCancelPurchase booking)
        {
            try
            {
                DAL db = new DAL();
                string booking_id = clsCrypto.Decrypt(booking.b_token);
                db.Parameters("booking_id", booking_id);
                db.Parameters("canceled_by_id", clsCrypto.Decrypt(booking.a_token));
                db.Parameters("canceled_by_lookup_id", booking.lookup_id);
                db.Parameters("cancel_reason", booking.cancel_reason);
                db.CommandText = "sp_cancel_booking";
                string result = db.ExecuteStoredProcedure();
                if (result == "-10")
                {
                    status = KeyConstantsMsgs.error;
                    message = cannot_cancel_past_booking;
                    return;
                }
                else
                if (result == "10")
                {
                    //CalculateAvailability(booking_id);
                    status = KeyConstantsMsgs.success;
                    message = KeyConstantsMsgs.booking_canceled;
                    try
                    {
                        clsEmailBLL SendEmailToCustomer = new clsEmailBLL(EmailType.booking_cancelation_customer, string.Empty, booking_id, null);
                        SendEmailToCustomer.SendEmail();
                    }
                    catch (Exception ex)
                    {
                        string tt = ex.ToString();
                    }

                    try
                    {
                        clsEmailBLL SendEmailToOwner = new clsEmailBLL(EmailType.booking_cancelation_owner, string.Empty, booking_id, null);
                        SendEmailToOwner.SendEmail();
                    }
                    catch (Exception ex)
                    {
                        string tt = ex.ToString();
                    }
                }
                else if (result == "-500")//cancelled by captain
                {
                    //CalculateAvailability(booking_id);
                    status = KeyConstantsMsgs.success;
                    message = KeyConstantsMsgs.booking_canceled;
                }
                else
                {
                    status = KeyConstantsMsgs.error;
                    message = KeyConstantsMsgs.error_canceling_booking;
                }
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                status = KeyConstantsMsgs.error;
                message = KeyConstantsMsgs.error_canceling_booking;

            }
        }





        private string FormatDateTimeForCalendar(string date_str, string time_str)
        {
            DateTime dateOnly = Convert.ToDateTime(date_str);
            DateTime timeOnly = Convert.ToDateTime(time_str);

            DateTime combined = dateOnly.Date.Add(timeOnly.TimeOfDay);
            return combined.ToLongDateString();
        }


        //public string EntitledToRefund(string canceled_date, string cut_off_date, int canceled_by_lookup_id)
        //{
        //    if (canceled_date == string.Empty)
        //    {
        //        return "";
        //    }
        //    if (canceled_by_lookup_id == KeyConstants.KeyConstants.canceled_by_owner)
        //    {
        //        return "Yes";
        //    }
        //    DateTime cancel_date = Convert.ToDateTime(canceled_date);
        //    DateTime cut_date = Convert.ToDateTime(cut_off_date);
        //    if (cancel_date > cut_date)
        //    {
        //        return "No";
        //    }
        //    else
        //    {
        //        return "Yes";
        //    }
        //}


        public string CancelationCutOffDate(string event_date, int cut_off_days)
        {
            DateTime dateOnly = Convert.ToDateTime(event_date);
            return dateOnly.AddDays(cut_off_days).ToString();
        }

        public string GetDuration(string start_time, string end_time)
        {
            string random_date = "6/22/2009 ";
            DateTime dt_start = DateTime.Parse(random_date + start_time);
            DateTime dt_end = DateTime.Parse(random_date + end_time);
            var diff = dt_end.Subtract(dt_start);
            return diff.TotalHours.ToString();

        }

        public bool IsBookingAvailable(string start_time, string end_time, string booked_start_timeA, string booked_end_timeA)
        {
            string random_date = "6/22/2009 ";
            DateTime dt_start1 = DateTime.Parse(random_date + start_time);
            DateTime dt_end1 = DateTime.Parse(random_date + end_time);
            DateTime booked_start = DateTime.Parse(random_date + booked_start_timeA);
            DateTime booked_end = DateTime.Parse(random_date + booked_end_timeA);
            if (dt_start1.Ticks >= booked_start.Ticks && dt_start1.Ticks <= booked_end.Ticks || dt_end1.Ticks >= booked_start.Ticks && dt_end1.Ticks <= booked_end.Ticks)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


}

