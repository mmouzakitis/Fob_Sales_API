using FOB_Sales_API.DataAccessLayer;
using FOB_Sales_API.Models.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FOB_Sales_API.Models.Common
{
    public class clsSearhObj
    {
        public string business_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string account_type { get; set; }
        public bool? contacted { get; set; }
        public bool? from_marketing_list { get; set; }
        public string created_by_id { get; set; }
    }

    public class clsCommon
    {
       

        public string ConstructAccountWhereClause(clsSearhObj SearchParameters, DAL db)
        {
            string where_clause = string.Empty;
            bool add_AND = false;
            db.ClearParameters();


            if (string.IsNullOrEmpty(SearchParameters.email) == false)
            {
                db.Parameters("email", SearchParameters.email);
                where_clause = "user_email LIKE '%' + @email + '%' ";
                add_AND = true;
            }
            if (SearchParameters.from_marketing_list != null)
            {
                if (SearchParameters.from_marketing_list == true)
                {

                    if (add_AND == true)
                    {
                        where_clause = where_clause + " AND " + " marketing_list_id IS NOT NULL ";
                    }
                    else
                    {
                        where_clause = "marketing_list_id IS NOT NULL";
                    }
                    add_AND = true;
                }
            }
            if (string.IsNullOrEmpty(SearchParameters.first_name) == false)
            {
                db.Parameters("first_name", SearchParameters.first_name);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + "first_name LIKE @first_name + '%' ";
                }
                else
                {
                    where_clause = "first_name LIKE @first_name + '%' ";
                }
                add_AND = true;
            }

            if (string.IsNullOrEmpty(SearchParameters.last_name) == false)
            {
                db.Parameters("last_name", SearchParameters.last_name);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + " last_name LIKE @last_name + '%' ";
                }
                else
                {
                    where_clause = "last_name LIKE @last_name + '%' ";
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


        public string ConstructMarketingWhereClause(clsSearhObj SearchParameters, DAL db)
        {
            string where_clause = string.Empty;
            bool add_AND = false;
            db.ClearParameters();


            if (string.IsNullOrEmpty(SearchParameters.business_name) == false)
            {
                db.Parameters("business_name", SearchParameters.business_name);
                where_clause = "business_name LIKE '%' + @business_name + '%' ";
                add_AND = true;
            }
            
             if (string.IsNullOrEmpty(SearchParameters.created_by_id) == false)
            {
                db.Parameters("created_by_id", clsCrypto.Decrypt(SearchParameters.created_by_id));
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + " created_by_id=@created_by_id";
                }
                else
                {
                    where_clause = "created_by_id=@created_by_id ";
                }
                add_AND = true;
            }

            if (string.IsNullOrEmpty(SearchParameters.address) == false)
            {
                db.Parameters("address", SearchParameters.address);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + "business_address LIKE '%' + @address + '%' ";
                }
                else
                {
                    where_clause = "business_address LIKE '%' + @address + '%' ";
                }
                add_AND = true;
            }

            if (string.IsNullOrEmpty(SearchParameters.city) == false)
            {
                db.Parameters("city", SearchParameters.city);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + " business_city=@city";
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
                    where_clause = where_clause + " AND " + " business_state=@state";
                }
                else
                {
                    where_clause = "business_state=@state";
                }
                add_AND = true;
            }
            if (string.IsNullOrEmpty(SearchParameters.email) == false)
            {
                db.Parameters("email", SearchParameters.email);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + " business_email LIKE '%' + @email + '%' ";
                }
                else
                {
                    where_clause = "business_email LIKE '%' + @email + '%' ";
                }
                add_AND = true;
            }
            
            if (SearchParameters.contacted != null)
            {
                db.Parameters("contacted", SearchParameters.contacted.ToString());
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + " has_been_contacted=@contacted";
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


    }
}