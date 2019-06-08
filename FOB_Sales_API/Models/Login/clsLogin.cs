using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Crypto;
using FOB_Sales_API.Models.KeyConstants;
using System.Data;
using WebApi.Jwt;
using FOB_Sales_API.Models.Common;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.Interfaces;
//using FOB_Sales_API.Models.Password;
//using FOB_Sales_API.Models.UserModels;
//using SendGrid.Helpers.Mail;

namespace FOB_Sales_API.Models.UserModels
{

    public class clsLogin : IFoBMessages
    {
        public string status { get; set; }
        public string message { get; set; }

        public enum user_type
        {
            angler =1,
            captain = 2,
            admin = 3
        }

 
        public clsLoginObj LoginSalesUser(clsLoginUser user)
        {
            clsLoginObj login_obj = new clsLoginObj();
            try
            {
                DAL db = new DAL();
                db.Parameters("user_email", user.user_email);
                db.CommandText = "SELECT disable_login FROM system_settings WHERE system_settings_id=1";
                bool disable_all_logins = DBLogic.DBBool(db.ExecuteScalar());
                if (disable_all_logins == true)
                {
                    login_obj.message = KeyConstantsMsgs.website_down_for_maintenance;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                db.CommandText = "SELECT account_id FROM user_accounts WHERE user_email=@user_email";

                 string account_id = DBLogic.DBString(db.ExecuteScalar());

                if (account_id == "" || account_id == null)
                {
                    login_obj.message = KeyConstantsMsgs.wrong_email_password;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                DataTable dt = GetLoginInfo(account_id);
                int Count = dt.Rows.Count;
                if (Count <= 0)
                {
                    login_obj.message = KeyConstantsMsgs.wrong_email_password;
                    return login_obj;
                }
                string account_id_not_encrypted = account_id;// dt.Rows[0]["account_id"].ToString();
                string database_password_hashed = dt.Rows[0]["user_password"].ToString();
                string salt = dt.Rows[0]["user_salt"].ToString();
                bool account_locked = DBLogic.DBBool(dt.Rows[0]["account_locked"]);
                bool account_disabled = DBLogic.DBBool(dt.Rows[0]["account_canceled"]);
                bool email_verified = DBLogic.DBBool(dt.Rows[0]["email_verified"]);
                string account_type = DBLogic.DBString(dt.Rows[0]["account_type_id"]);
                string first_name = DBLogic.DBString(dt.Rows[0]["first_name"]);
                string last_name = DBLogic.DBString(dt.Rows[0]["last_name"]);
                bool is_sales_associate = DBLogic.DBBool(dt.Rows[0]["sales_associate"]);
                if(account_type != "3")
                {
                    login_obj.message = KeyConstantsMsgs.no_login_rights;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                if (account_disabled == true)
                {
                    login_obj.message = KeyConstantsMsgs.account_disabled;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                if (account_locked == true)
                {
                    login_obj.message = KeyConstantsMsgs.account_locked;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                if (email_verified == false)
                {
                    login_obj.message = KeyConstantsMsgs.verify_email;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
                string user_password_hashed = clsCrypto.HashPassword(user.user_password);
                string user_password_hashed_and_salted = clsCrypto.HashPassword(string.Format("{0}{1}", user_password_hashed, salt));
                if (database_password_hashed == user_password_hashed_and_salted)
                {
                    db.parameters.Clear();
                    db.Parameters("account_id", account_id_not_encrypted);
                    db.CommandText = "sp_reset_failed_attempts";
                    db.ExecuteStoredProcedure();
                    login_obj.user_email = user.user_email;
                    login_obj.f_name = first_name;
                    login_obj.l_name = last_name;
                    login_obj.sales_associate = is_sales_associate;
                    login_obj.user_token = clsCrypto.Encrypt(account_id);
                    login_obj.access_token = JwtManager.GenerateToken(user.user_email);
                    login_obj.account_type = account_type;
                    login_obj.is_guest_account = false;
                    if (login_obj.account_type == "3")
                    {
                        login_obj.site_status = 439839320;
                    }else
                    {
                        login_obj.site_status = 0;
                    }
                    login_obj.message = KeyConstantsMsgs.success;
                    login_obj.status = KeyConstantsMsgs.success;
                    return login_obj;
                }
                else
                {
                    db.parameters.Clear();
                    db.Parameters("account_id", account_id_not_encrypted);
                    db.CommandText = "sp_update_failed_attempts";
                    db.ExecuteStoredProcedure();
                    login_obj.message = KeyConstantsMsgs.wrong_email_password;
                    login_obj.status = KeyConstantsMsgs.error;
                    return login_obj;
                }
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                login_obj.message = KeyConstantsMsgs.wrong_email_password;
                return login_obj;
            }
        }


        private DataTable GetLoginInfo(string account_id)
        {
            DAL db = new DAL();
            db.Parameters("account_id", account_id);
            db.CommandText = "SELECT account_id,user_password,user_salt,account_locked,account_canceled,email_verified,is_admin,account_type,account_type_id,first_name,last_name,sales_associate FROM v_user_accounts WHERE account_id=@account_id";
            return db.ConvertQueryToDataTable();
        }    

    }
}