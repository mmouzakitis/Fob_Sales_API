using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FOB_Sales_API.Models.KeyConstants;
using WebApi.Jwt.Filters;
using FOB_Sales_API.Models;
using FOB_Sales_API.Models.Marketing;
using FOB_Sales_API.Models.Listings;
using FOB_Sales_API.Models.UserAccounts;
using FOB_Sales_API.Models.Common;
//using FOB_API.Models.Listings;

namespace FOBAdmin.Controllers.APIs
{

    //[Authorize(Roles = "Admin,Super Admin")]
    [JwtAuthentication]
    [EnableCors(origins: KeyConstantsMsgs.ADMIN_CORS_ORIGIN, headers: "*", methods: KeyConstantsMsgs.ADMIN_CORS_POST)]//https://www.fishonbooking.com
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class UserAccountsController : ApiController
    {

        
        [HttpPost]
        [Route("BindMarketingRecords")]
        public void BindMarketingRecords()
        {
            clsUserAccount SalesEmployees = new clsUserAccount();
             SalesEmployees.BindMarketingRecords();
        }

        [HttpPost]
        [Route("LoadSalesEmplName")]
        public List<clsSalesEmployees> LoadSalesEmplName()
        {
            clsUserAccount SalesEmployees = new clsUserAccount();
            return SalesEmployees.LoadSalesEmplName();
        }

        [HttpPost]
        [Route("TotalAccountsCount")]
        public List<clsAccountsGrouped> TotalAccountsCount()
        {
            clsUserAccount NewRecord = new clsUserAccount();
            return NewRecord.TotalAccountsCount();
        }


        [HttpPost]
        [Route("LoadUserAccounts")]
        public List<clsUserAccount> LoadUserAccounts(clsSearhObj search)
        {
            clsUserAccount NewRecord = new clsUserAccount();
            return NewRecord.LoadUserAccounts(search);
        }

        

        [HttpPost]
        [Route("LoadAccountListings")]
        public List<clsListingDetails> LoadListingDetails(clsId id)
        {
            clsUserAccount NewRecord = new clsUserAccount();
            return NewRecord.LoadListingDetails(id);
        }

        [HttpPost]
        [Route("LoadAccountBookings")]
        public List<clsBookingDetails> LoadBookingDetails(clsId id)
        {
            clsUserAccount NewRecord = new clsUserAccount();
            return NewRecord.LoadBookingDetails(id);
        }


        public HttpResponseMessage GetHttpResponseType(string status, string message)
        {
            if (status == KeyConstantsMsgs.success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
        }


    }
}


