using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FOB_Sales_API.Models.Common
{
    public class clsAddressParser
    {

        //public string city { get; set; } = "";
        //public string state_abbr { get; set; } = "";
        //public string zip_code { get; set; } = "";
        //public string country { get; set; } = "";
        public bool has_city { get; set; }
        public bool has_state { get; set; }
        public bool has_zip { get; set; }
        public bool has_country { get; set; }

        public string parameter_value { get; set; }
        public string column_to_search { get; set; }

        public void ParseAddress(string address)
        {
            has_city = false;
            has_state = false;
            has_zip = false;
            has_country = false;

            address = address.Trim();
            if (address.EndsWith(","))
            {
                address = address.Remove(address.Length - 1, 1);
            }
            if (address.StartsWith(","))
            {
                address = address.Remove(0, 1);
            }
            address = address.Trim();

            string[] words = address.Split(',');

            //check to see if the string has a city 
            //foreach (string word in words)
            //{
            string param = GetState(words[0].Trim());
            if (param.Length > 1)
            {
                has_state = true;
                parameter_value = param;
                return;
            }
            else
            {
                param = words[0].Trim();
                has_city = true;
                parameter_value = param;
                return;
            }


            //string param2 = GetCountry(word.Trim());
            //string param3 = GetZip(word.Trim());
            //if (param1 == string.Empty && param2 == string.Empty && param3 == string.Empty)
            //{
            //    has_city = true;
            //    city = word;
            //}
            // }

            ////check to see if the string has a state 
            //foreach (string word in words)
            //{
            //    if (state_abbr.Length > 0)
            //    {
            //        has_state = true;
            //        break;
            //    }
            //    state_abbr = GetState(word.Trim());
            //}

            ////check to see if the string has a country 
            //foreach (string word in words)
            //{
            //    if (country.Length > 0)
            //    {
            //        has_country = true;
            //        break;
            //    }
            //    country = GetCountry(word.Trim());
            //}

            ////check to see if the string has a zip code 
            //foreach (string word in words)
            //{
            //    if (zip_code.Length > 0)
            //    {
            //        has_zip = true;
            //        break;
            //    }
            //    zip_code = GetZip(word.Trim());
            //}

            //selected_parameter = string.Empty;
            //if (has_city == true)
            //{
            //    city = verify_city_is_correct(city);
            //    selected_parameter = city;
            //    column_to_search = "listing_city";
            //    return;
            //}
            //if (has_zip == true)
            //{
            //    selected_parameter = zip_code;
            //    column_to_search = "listing_state_abbr";
            //    return;
            //}
            //if (has_state == true)
            //{
            //    selected_parameter = state_abbr;
            //    column_to_search = "listing_zip";
            //    return;
            //}
        }

    //If for some reason the user enters city state without a comma for example myrtle beach SC or Destin FL we need to parse out the state from the string.
    private string verify_city_is_correct(string city)
    {
            string[] multi_name  = city.Split(' ');
            if(multi_name.Length == 1)
            {
                return city;
            }
            else if (multi_name.Length == 2)
            {
                string state;
                
                string matches_state = states.TryGetValue(multi_name[1], out state) ? state : string.Empty;
                if (matches_state.Length > 0)
                {
                    return multi_name[0];
                }
                return city;
            }
            else if (multi_name.Length == 3)
            {
                string state;
                /* error handler is to return an empty string rather than throwing an exception */
                string matches_state = states.TryGetValue(multi_name[2], out state) ? state : string.Empty;
                if (matches_state.Length > 0)
                {
                    return multi_name[0] + " " + multi_name[1];
                }
                return city;
            }
            return city;
        }
    private static readonly IDictionary<string, string> states = new Dictionary<string, string>
    {
    { "AL", "AL" },
    { "AK", "AK" },
    { "AZ", "AZ" },
    { "AR", "AR" },
    { "CA", "CA" },
    { "CO", "CO" },
    { "CT", "CT" },
    { "DE", "DE" },
    { "DC", "DC" },
    { "FL", "FL" },
    { "GA", "GA" },
    { "HI", "HI" },
    { "ID", "ID" },
    { "IL", "IL" },
    { "IN", "IN" },
    { "IA", "IA" },
    { "KS", "KS" },
    { "KY", "KY" },
    { "LA", "LA" },
    { "ME", "ME" },
    { "MD", "MD" },
    { "MA", "MA" },
    { "MI", "MI" },
    { "MN", "MN" },
    { "MS", "MS" },
    { "MO", "MO" },
    { "MT", "MT" },
    { "NE", "NE" },
    { "NV", "NV" },
    { "NH", "NH" },
    { "NJ", "NJ" },
    { "NM", "NM" },
    { "NY", "NY" },
    { "NC", "NC" },
    { "ND", "ND" },
    { "OH", "OH" },
    { "OK", "OK" },
    { "OR", "OR" },
    { "PA", "PA" },
    { "RI", "RI" },
    { "SC", "SC" },
    { "SD", "SD" },
    { "TN", "TN" },
    { "TX", "TX" },
    { "UT", "UT" },
    { "VT", "VT" },
    { "VA", "VA" },
    { "WA", "WA" },
    { "WV", "WV" },
    { "WI", "WI" },
    { "WY", "WY" },
    { "ALABAMA", "AL" },
    { "ALASKA", "AK" },
    { "ARIZONA","AZ" },
    { "ARKANSAS" ,"AR"},
    { "CALIFORNIA","CA" },
    { "COLORADO","CO" },
    { "CONNECTICUT","CT" },
    { "DELAWARE","DE" },
    { "DISTRICT of COLUMBIA","DC" },
    { "FLORIDA", "FL" },
    { "GEORGIA","GA" },
    { "HAWAII", "HI" },
    { "IDAHO", "ID" },
    { "ILLINOIS", "IL" },
    { "INDIANA", "IN" },
    { "IOWA", "IA" },
    { "KANSAS", "KS" },
    { "KENTUCKY", "KY" },
    { "LOUISIANA" , "LA"},
    { "MAINE", "ME" },
    { "MARYLAND" , "MD" },
    { "MASSCHUSETTS", "MA" },
    { "MICHIGAN", "MI" },
    { "MINNESOTA", "MN" },
    { "MISSISSIPPI", "MS"},
    { "MISSOURI", "MO"  },
    { "MONTANA", "MT"},
    { "NEBRASKA","NE" },
    { "NEVADA","NV"},
    { "NEW HAMPSHIRE", "NH" },
    { "NEW JERSEY" , "NJ" },
    { "NEW MEXICO", "NM" },
    { "NEW YORK", "NY" },
    { "NORTH CAROLINA", "NC"},
    { "NORTH DAKOTA", "ND" },
    { "OHIO", "OH" },
    { "OKLAHOMA", "OK" },
    { "OREGON", "OR" },
    { "PENNSYLVANIA", "PA"},
    { "RHODE ISLAND","RI"},
    { "SOUTH CAROLINA" , "SC" },
    { "SOUTH DAKOTA", "SD"},
    { "TENNESSEE","TN" },
    { "TEXAS", "TX" },
    { "UTAH", "UT" },
    { "VERMONT" , "VT"},
    { "VIRGINIA", "VA" },
    { "WASHINGTON", "WA" },
    { "WEST VIRGINIA", "WV" },
    { "WISSCONSIN" , "WI" },
    { "WYOMING", "WY" },
};

        public static string GetState(string parameter)
        {
            parameter = parameter.ToUpper();
            string state;
            /* error handler is to return an empty string rather than throwing an exception */
            return states.TryGetValue(parameter, out state) ? state : string.Empty;
        }

        public static string GetCountry(string parameter)
        {
            if (parameter.ToUpper() == "USA")
            {
                return parameter;
            }
            else
            {
                return string.Empty;
            }
        }


        public static string GetZip(string zipCode)
        {
            var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
            if ((!Regex.Match(zipCode, _usZipRegEx).Success))
            {
                return string.Empty;
            }
            return zipCode;
        }
    }


}