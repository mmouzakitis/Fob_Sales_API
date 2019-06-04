using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ModelBinding;

namespace FOB_Sales_API.Models
{
    public class clsModelErrorCollection
    {
        public List<string> error_desc { get; set; }
        public List<string> error_no { get; set; }

        public clsModelErrorCollection()
        {
            error_desc = new List<string>();
            error_no = new List<string>();
        }


        public clsModelErrorCollection GetModelStateErrors(ModelStateDictionary ModelState)
        {
            clsModelErrorCollection error_desc = new clsModelErrorCollection();
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    //error_desc.error_desc.Add("<br>" + error.ErrorMessage.ToString());  //here 'error.ErrorMessage' will have required error message//DoSomethingWith(error.ErrorMessage.ToString());
                    error_desc.error_desc.Add(error.ErrorMessage);  //here 'error.ErrorMessage' will have required error message//DoSomethingWith(error.ErrorMessage.ToString());
                }
            }
            return error_desc;
        }

        public clsModelErrorCollection GetErrorCollection(ModelStateDictionary ModelState)
        {
            return GetModelStateErrors(ModelState);
        }
    }
}