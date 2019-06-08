using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FOB_Sales_API.Models.Common;


namespace FOB_Sales_API.Models.UserModels
{
    public class clsUserEmail
    {

        public string account_token { get; set; } = "";

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "108:Email address cannot be longer than 50 characters.")]
        [EmailAddress(ErrorMessage = "108:A valid Email is required")]
        public string user_email { get; set; } = "";
    }

    public class clsUserToken
    {
        public string user_token { get; set; }
    }
    public class clsChangePasswordByUser
    {
        public string account_token { get; set; } = "";
        public string old_password { get; set; } = "";
        public string new_password { get; set; } = "";
        public string retype_password { get; set; } = "";
    }


    public class clsDisplayName
    {
        public string account_token { get; set; } = "";
        [StringLength(20, ErrorMessage = "122:Display name cannot be longer than 20 characters.")]
        public string display_name { get; set; } = "";
    }


    public class clsUserInfo : clsUserEmail
    {
        [Required]
        public string id { get; set; }


        [StringLength(20, ErrorMessage = "122:Display name cannot be longer than 20 characters.")]
        public string d_name { get; set; }

        [Required(ErrorMessage = "110:First name is required")]
        [StringLength(25, ErrorMessage = "110:First name cannot be longer than 25 characters.")]
        public string f_name { get; set; }

        [Required(ErrorMessage = "111:Last name is required")]
        [StringLength(25, ErrorMessage = "111:Last name cannot be longer than 25 characters.")]
        public string l_name { get; set; }

        [StringLength(10, ErrorMessage = "109:Phone number cannot be longer than 10 numbers.")]
        public string c_number { get; set; }

        public string address { get; set; }
        public string city { get; set; }
        public string state_abbr { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string country_id { get; set; }
        public string gender { get; set; }
        public int gender_id { get; set; }
        public string overall_rating { get; set; }
        public string listing_reg_completed { get; set; }
        public string profile_pic { get; set; }
        public bool profile_pic_exists { get; set; }
        public string account_type { get; set; }
        public int account_type_id { get; set; }
        public int account_locked { get; set; }
        public int account_active { get; set; }
        public string edited_by_id { get; set; }
       
    }


    public class clsUserInfoUpdateByUser
    {
        public string account_token { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string c_number { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string state_id { get; set; }
        public string gender_id { get; set; }
    }

    public class clsEmail
    {
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "109:User email cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "113:A valid email is required")]
        [EmailAddress(ErrorMessage = "113:A valid email is required")]
        public string user_email { get; set; }
    }


    public class clsSendResetPassword
    {
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "109:User email cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "113:A valid email is required")]
        [EmailAddress(ErrorMessage = "113:A valid email is required")]
        public string u_email { get; set; }

        [Required(ErrorMessage = "200:Re Captcha is required")]
        public string captcha { get; set; }
    }


    public class clsRegisterAsGuest : clsEmail
    {
        [Required(ErrorMessage = "110:First name is required")]
        [StringLength(20, ErrorMessage = "110:First name cannot be longer than 20 characters")]
        public string f_name { get; set; }

        [Required(ErrorMessage = "111:Last name is required")]
        [StringLength(20, ErrorMessage = "111:Last name cannot be longer than 20 characters")]
        public string l_name { get; set; }

        [Required(ErrorMessage = "200:Re Captcha is required")]
        public string captcha { get; set; }
    }


    public class clsRegister : clsEmail
    {
        [Required(ErrorMessage = "110:First name is required")]
        [StringLength(20, ErrorMessage = "110:First name cannot be longer than 20 characters.")]
        public string f_name { get; set; }

        [Required(ErrorMessage = "111:Last name is required")]
        [StringLength(20, ErrorMessage = "111:Last name cannot be longer than 20 characters.")]
        public string l_name { get; set; }
       
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password Must be between 8 and 30 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string user_password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required")]
        [DataType(DataType.Password)]
        [Compare("user_password")]
        public string user_retype_password { get; set; }

        [Required(ErrorMessage = "200:Re Captcha is required")]
        public string captcha { get; set; }

        public bool is_sales_associate { get; set; }
        public bool is_marketing_account { get; set; }

    }


    public class clsUserMessage
    {

        public string user_token { get; set; } = string.Empty;

        [Required(ErrorMessage = "")]
        [StringLength(40, ErrorMessage = "115:Please provide a name")]
        public string u_name { get; set; }

        [StringLength(50, ErrorMessage = "113:Please provide an email")]
        public string u_email { get; set; }

        [Required(ErrorMessage = "116:Please provide a subject")]
        [StringLength(50, ErrorMessage = "116:Maximum length is 50 characters")]
        public string u_subject { get; set; }

        [Required(ErrorMessage = "117:Please provide a message")]
        [StringLength(500, ErrorMessage = "117:Maximum length is 500 characters")]
        public string u_message { get; set; }

        [StringLength(13, ErrorMessage = "109:Phone number cannot be longer than 13 numbers.")]
        public string u_phone { get; set; } = string.Empty;
       
        //[Required(ErrorMessage = "250:You did not pass recaptcha verification, please try again")]
        public string captcha { get; set; }
    }


    public class clsLoginUser
    {
        [Required(ErrorMessage = "113:Valid Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "113:Email cannot be longer than 50 characters")]
        public string user_email { get; set; }

        [Required(ErrorMessage = "114:Password is required")]
        public string user_password { get; set; }

        [Required(ErrorMessage = "200:Re Captcha is required")]
        public string captcha { get; set; }
    }


    public class clsLoginUserSecondary
    {
        [Required(ErrorMessage = "113:Valid Email is required")]
        [DataType(DataType.EmailAddress)]
        public string user_email { get; set; }

        [Required(ErrorMessage = "113:Valid Email is required")]
        [DataType(DataType.EmailAddress)]
        public string user_email_secondary { get; set; }

        [Required(ErrorMessage = "114:Verification code is required")]
        public string verification_code { get; set; }
    }


    public class clQuickLogin
    {
        public string quick_token { get; set; }
        public string captcha { get; set; }
    }

    public class clsLoginObj
    {
        public string user_email { get; set; }
        public string access_token { get; set; }
        public string user_token { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public bool sales_associate { get; set; }
        public bool is_marketing { get; set; }
        public int site_status { get; set; }
        public string account_type { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public bool is_guest_account { get; set; }
    }


    public class clsResetPassword
    {
        [Required]
        public string u_token { get; set; }

        [Required(ErrorMessage = "114:Password is required")]
        [StringLength(255, ErrorMessage = "114:Must be between 9 and 50 characters", MinimumLength = 9)]
        public string user_password { get; set; }

        [Required(ErrorMessage = "115:Retyped password is required")]
        [StringLength(255, ErrorMessage = "115:Must be between 9 and 50 characters", MinimumLength = 9)]
        [Compare("user_password")]
        public string user_retype_password { get; set; }
    }
}