using FOB_Sales_API.Models.KeyConstants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FOB_Sales_API.DataAccessLayer
{
    //mike was here
    public class DAL
    {
        public string conn_string { get; set; }
        private DataTable GetDatatable { get; set; }
        public string CommandText { get; set; }
        public Dictionary<string, string> parameters = new Dictionary<string, string>();

        public DAL()
        {
            conn_string = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public DAL(string conn_name)
        {
            conn_string = ConfigurationManager.ConnectionStrings[conn_name].ConnectionString;
        }

        public void ClearParameters()
        {
            parameters.Clear();
        }

        //public void AddParameter(string name, SqlParameter value)
        //{
        //    procParameters.Add(name, value);
        //    //SqlDbType tt =  SqlDbType.Date;
        //}
        //SqlDbType tt =  SqlDbType.Date;
        public void Parameters(string name, string value)
        {
            if (name.StartsWith("@")==false)
            {
                name = "@" + name;
            }
            parameters.Add(name, value);
        }

        public DataTable ConvertQueryToDataTable()
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(CommandText, conn_string))
            {
                foreach (var procParameter in parameters)
                {
                    adapter.SelectCommand.Parameters.AddWithValue(procParameter.Key, procParameter.Value);
                }
                GetDatatable = new DataTable();
                adapter.Fill(GetDatatable);
            }

            return GetDatatable;
        }

        //execute scalar
        public object ExecuteScalar()
        {
            using (var conn = new SqlConnection(conn_string))
            {
                using (var cmd = new SqlCommand(CommandText, conn))
                {
                    foreach (var procParameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(procParameter.Key, procParameter.Value);
                    }
                    conn.Open();
                    object value = cmd.ExecuteScalar();
                    conn.Close();
                    return  value;
                }
            }
        }

        //execute non query
        public void ExecuteNonQuery()
        {
            //execute scalar
            using (var conn = new SqlConnection(conn_string))
            {
                using (var cmd = new SqlCommand(CommandText, conn))
                {
                    foreach (var procParameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(procParameter.Key, procParameter.Value);
                       // command.Parameters.Add(new SqlParameter(procParameter.Key, SqlDbType.Int)).Value = procParameter.Value;
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public string ExecuteStoredProcedure()
        {
            //stored procedure
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                   
                    cmd.CommandText = CommandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    // cmd.Parameters.Add("@account_id", SqlDbType.Int).Value = account_id;
                    bool has_output_parameter = false;
                    string output_parameter = "";
                    foreach (var procParameter in parameters)
                    {
                        if (procParameter.Value == KeyConstants.parameter_out)
                        {
                            has_output_parameter = true;
                            cmd.Parameters.Add(procParameter.Key, SqlDbType.VarChar, 500);
                            cmd.Parameters[procParameter.Key].Direction = ParameterDirection.Output;
                            output_parameter = procParameter.Key;
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(procParameter.Key, procParameter.Value);
                        }
                    }

                    var result = "";
                    if (has_output_parameter == false)
                    {
                        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        result = returnParameter.Value.ToString();
                    }
                    else
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        result = (string)cmd.Parameters[output_parameter].Value;
                    }
                    
                    conn.Close();
                    return result.ToString();
                }
            }

        }
    }
}