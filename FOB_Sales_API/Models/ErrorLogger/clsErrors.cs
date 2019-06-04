using FOB_Sales_API.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.ErrorLogger
{
    public class clsErrors
    {
        public int _error_number { get; set; }
        public string _error_description { get; set; }
        public string _account_id { get; set; }
        public string _booking_type { get; set; }

        public clsErrors(int error_number,string description, string account_id,string booking_type)
        {
            _booking_type = booking_type;
            _error_description = description;
            _error_number = error_number;
            _account_id = account_id;
            try
            {
                DAL db = new DAL();
                db.Parameters("application", "Main Applications");
                db.Parameters("error_no", _error_number.ToString());
                db.Parameters("error_description", _error_description);
                db.Parameters("date_created", DateTime.Now.ToString());
                db.CommandText = "sp_log_errors";
                db.ExecuteStoredProcedure();
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
            }
        }

    }
}