using FOB_Sales_API.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.Models.ApplicationWideSettings
{
    public class clsApplicationWideSettings
    {
        public string GetSetting(int id)
        {
            try
            {
                DAL db = new DAL();
                db.parameters.Add("id",id.ToString());
                db.CommandText = "SELECT setting_value FROM application_wide_settings WHERE setting_id=@id ";
                return (string)db.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string tt = ex.ToString();
                return null;
            }
        }

    }
}