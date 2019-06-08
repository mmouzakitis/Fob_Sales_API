using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using FOB_Sales_API.Models.Crypto;

namespace FOB_Sales_API.Models.Common
{
    public class DBLogic
    {
        public List<string> ConvertDateArrayToString(List<DateTime> list)
        {
            //24-05-2019" dd/mm/yyyy
            List<string> temp = new List<string>();
            foreach (DateTime item in list)
            {
                string ss = item.ToString("dd-MM-yyyy");
                temp.Add(ss);
            }
            return temp;
        }



        public List<DateTime> SortDateAscending(List<DateTime> list)
        {
            list.Sort((a, b) => a.CompareTo(b));
            return list;
        }

        
        public static string FormatPhoneForDatabase(string value)
        {
            return value.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ","").Replace(".","");
        }

        public static string ConvertToCurrency(double value)
        {
            return value.ToString("C", CultureInfo.CurrentCulture);
        }


        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }


        public static double AggreedFee(double base_price, double base_guests, double cost_per_extra_guest, int no_of_guests)
        {
            double cost = 0;
            if (no_of_guests <= base_guests)
            {
                cost = base_price;
            }
            if (no_of_guests > base_guests)
            {
                cost = base_price + (cost_per_extra_guest * (no_of_guests - base_guests));
            }
            return cost;
        }

        public static string ToProperCase(string str)
        {
           return Char.ToUpper(str[0]) + str.Remove(0, 1);
        }

        public static string ConvertAvailabilityPercentToText(string value)
        {
            int valueNumeric = Convert.ToInt32(value);
            string val = string.Empty;
            if (valueNumeric > 99)
            {
                val = "Fully Booked";
            }
            else if (valueNumeric > 80)
            {
                val = "Almost Booked";
            }
            else if (valueNumeric > 65)
            {
                val = "low Availability";
            }
            else if (valueNumeric > 40)
            {
                val = "Medium Availability";
            }
            else if (valueNumeric <= 40)
            {
                val = "High Availability";
            }
            return val;
        }

        public static string GetMonthByNo(int month)
        {
            switch (month)
            {
                case 1:
                    return "january";
                case 2:
                    return "february";
                case 3:
                    return "march";
                case 4:
                    return "april";
                case 5:
                    return "may";
                case 6:
                    return "june";
                case 7:
                    return "july";
                case 8:
                    return "august";
                case 9:
                    return "september";
                case 10:
                    return "october";
                case 11:
                    return "november";
                case 12:
                    return "december";
                default:
                    return "";
            }
        }


        public static string ConvertAvailabilityPercentToCSS(string value)
        {
            int valueNumeric = Convert.ToInt32(value);
            string val = string.Empty;
            if (valueNumeric > 99)
            {
                val = "danger";
            }
            else if (valueNumeric > 55)
            {
                val = "warning";
            }
            else if (valueNumeric <= 55)
            {
                val = "success";
            }
            return val;
        }


        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>string</returns>
        public static string LongDateString(string value)
        {
            DateTime date_value;

            if (DateTime.TryParse(value, out date_value))
            {
                return date_value.ToString("dddd, dd MMMM yyyy");
            }
            else
            {
                return value;
            }
        }

        public static string LongDateNoYearString(string value)
        {
            DateTime date_value;

            if (DateTime.TryParse(value, out date_value))
            {
                return date_value.ToString("dd MMMM");
            }
            else
            {
                return value;
            }
        }

        public static string ConvertToSingleString(List<string> ids)
        {
            string list = "";
            foreach (string id in ids)
            {
                list += "," + clsCrypto.Decrypt(id);
            }
            if (list.StartsWith(","))
            {
                list = list.Remove(0, 1);
            }
            return list;
        }


        public static string DBHoursDifference(string start_time, string end_time)
        {
            DateTime dt_start = DateTime.Parse("6/22/2009 " + start_time);
            DateTime dt_end = DateTime.Parse("6/22/2009 " + end_time);
            string hours = dt_start.Subtract(dt_end).Hours.ToString();
            return hours;
        }


        public static string DBShortTime(string value)
        {
            DateTime dt_end = DateTime.Parse("6/22/2009 " + value);
            return dt_end.ToString("h:mm tt");
        }


        public static string FormatTimeAMPM(string time_str)
        {
            DateTime dt = DateTime.Parse("6/22/2009 " + time_str);
            return dt.ToString("h:mm tt");
        }


        public static string FormatDateTimeForCalendar(string date_str, string time_str)
        {
            DateTime dateOnly = Convert.ToDateTime(date_str);
            DateTime timeOnly = Convert.ToDateTime(time_str);

            DateTime combined = dateOnly.Date.Add(timeOnly.TimeOfDay);
            return combined.ToLongDateString();
        }

        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>string</returns>
        public static string LongDateTimeString(string value)
        {
            DateTime date_value;

            if (DateTime.TryParse(value, out date_value))
            {
                return date_value.ToString("dddd, dd MMMM yyyy hh:mm");
            }
            else
            {
                return value;
            }
        }



        public static string ShortDateWithDashes(string value)
        {
            DateTime date_value;
            if (DateTime.TryParse(value, out date_value))
            {
                return date_value.ToString("yyyy-MM-dd");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>string</returns>
        public static string ShortDateString(string value)
        {
            if (value.Length > 5)
            {
                if (value.Contains("T"))
                {
                    return value.Split('T')[0];
                }
                else
                {
                    return value.Split(' ')[0];
                }
            }
            else
            {
                
                return value;
            }
        }


        public static DateTime ShortDate(string value)
        {
            DateTime date_value;
            if (DateTime.TryParse(value, out date_value))
            {
                return date_value;
            }
            return new DateTime();
            //else
            //{
            //    return null;
            //}
        }


        public static string ShortDateWithDashesString(string value)
        {
            DateTime date_value;
            if (DateTime.TryParse(value, out date_value))
            {
                string SS = date_value.ToString("dd/MM/yyyy");
                return SS.Replace("/", "-");
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>string</returns>
        public static string ConvertBoolToYesNo(bool value)
        {
            if (value == true)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>string</returns>
        public static int ConvertBoolToYesNoIds(bool value)
        {
            if (value == true)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }


        /// <summary>
        /// Converts a boolean value to a yes or no id
        /// </summary>
        /// <param name="value">boolean value</param>
        /// <returns>string</returns>
        public static string ConvertBoolToActiveCanceled(bool value)
        {
            if (value == true)
            {
                return "Active";
            }
            else
            {
                return "Canceled";
            }
        }
        /// <summary>
        /// Converts a yes no id to a boolean value
        /// </summary>
        /// <param name="value">integer value</param>
        /// <returns>string</returns>
        public static bool convertYesNoIdsToBool(int value)
        {
            if (value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }


        /// <summary>
        /// Convert database object to string
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>string</returns>
        public static string UnFormatPhone(object reader)
        {
            if (reader == null)
            {
                return string.Empty;
            }
            return reader == DBNull.Value ? string.Empty : reader.ToString().Replace("-", "").Replace("(", "").Replace(")", "");
        }


        ///// <summary>
        ///// Convert database object to string
        ///// </summary>
        ///// <param name="reader">Database Reader Object</param>
        ///// <returns>string</returns>
        //public static string FormatPhone(object reader)
        //{

        //    if (reader == null)
        //    {
        //        return string.Empty;
        //    }
        //    return reader == DBNull.Value ? string.Empty : String.Format("{0:(###) ###-####}", reader.ToString());
        //}

        public static string formatPhoneNumber(string phoneNum, string phoneFormat)
        {

            if (phoneFormat == "")
            {
                // If phone format is empty, code will use default format (###) ###-####
                phoneFormat = "###-###-####";
            }

            // First, remove everything except of numbers
            Regex regexObj = new Regex(@"[^\d]");
            phoneNum = regexObj.Replace(phoneNum, "");

            // Second, format numbers to phone string 
            if (phoneNum.Length > 0)
            {
                phoneNum = Convert.ToInt64(phoneNum).ToString(phoneFormat);
            }

            return phoneNum;
        }

        /// <summary>
        /// Convert database object to string
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>string</returns>
        public static string DBString(object reader)
        {
            if (reader == null)
            {
                return string.Empty;
            }
            return reader == DBNull.Value ? string.Empty : reader.ToString();
        }


        /// <summary>
        /// Convert database object to decimal
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>decimal</returns>
        public static decimal DBDecimal(object reader)
        {
            return reader == DBNull.Value ? 0 : Convert.ToDecimal(reader);
        }

        /// <summary>
        /// Convert database object to float
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>decimal</returns>
        public static float DBFloat(object reader)
        {
            return reader == DBNull.Value ? 0 : Convert.ToSingle(reader);
        }

        /// <summary>
        /// Convert database object to float
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>decimal</returns>
        public static double DBDouble(object reader)
        {
            return reader == DBNull.Value ? 0 : Convert.ToDouble(reader);
        }



        public static string DBStatusText(bool status)
        {
            if (status == true)
            {
                return "As Scheduled";
            }
            else
            {
                return "Canceled";
            }
        }


        /// <summary>
        /// Convert database object to integer
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>integer</returns>
        public static int DBInteger(object reader)
        {
            if (reader == null)
            {
                return 0;
            }
            else if (reader == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(reader);
            }

        }

        /// <summary>
        /// Convert database object to string
        /// </summary>
        /// <param name="reader">Database Reader Object</param>
        /// <returns>string</returns>
        public static DateTime DBDate(object reader)
        {
            if (reader == DBNull.Value)
            {
                return DateTime.Now;
            }
            else
            {
                string ss = reader.ToString();
                DateTime dDate;
                DateTime.TryParse(ss, out dDate);
                String.Format("{0:d/MM/yyyy}", dDate);
                return dDate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static bool DBBool(object reader)
        {
            return reader == DBNull.Value ? false : (bool)reader;
        }

        /// <summary>
        /// Convert string to a database null value
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>DBNull.Value</returns>
        public static object DBNullValue(object value)
        {
            return value == null ? DBNull.Value : (object)value;
        }

        public static int DBNullToZero(object value)
        {
            return value == null ? 0 : (int)value;
        }


        public static bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}