using FOB_Sales_API.DataAccessLayer;
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
        public bool? contacted { get; set; }
    }

    public class clsCommon
    {
       

        public string ConstructAccountWhereClause(clsSearhObj SearchParameters, DAL db)
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

            if (string.IsNullOrEmpty(SearchParameters.address) == false)
            {
                db.Parameters("address", SearchParameters.address);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_address LIEK '%' + @address + '%' ";
                }
                add_AND = true;
            }

            if (string.IsNullOrEmpty(SearchParameters.city) == false)
            {
                db.Parameters("city", SearchParameters.city);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_city=@city";
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

            if (string.IsNullOrEmpty(SearchParameters.address) == false)
            {
                db.Parameters("address", SearchParameters.address);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
                }
                else
                {
                    where_clause = "business_address LIEK '%' + @address + '%' ";
                }
                add_AND = true;
            }

            if (string.IsNullOrEmpty(SearchParameters.city) == false)
            {
                db.Parameters("city", SearchParameters.city);
                if (add_AND == true)
                {
                    where_clause = where_clause + " AND " + where_clause;
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