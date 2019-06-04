using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.Common
{
    public enum EmailType
    {
        email_verification = 1,
        reset_password = 2,
        booking_confirmation_customer = 3,
        booking_confirmation_owner = 4,
        booking_cancelation_customer = 5,
        booking_cancelation_owner = 6,
        booking_reminder = 7,
        booking_review = 8,
        quick_link = 9,
        password_has_changed = 10,
        new_user_welcome = 11,
        coupon_promotion = 12,
        txt_msg_reminder =13,
        notify_captain_of_review=14,
        new_sales_associate = 15
    }
}