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
using FOB_Sales_API.Models.Common;
//using FOB_API.Models.Listings;

namespace FOBAdmin.Controllers.APIs
{

    //[Authorize(Roles = "Admin,Super Admin")]
    [JwtAuthentication]
    [EnableCors(origins: KeyConstantsMsgs.ADMIN_CORS_ORIGIN, headers: "*", methods: KeyConstantsMsgs.ADMIN_CORS_POST)]//https://www.fishonbooking.com
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class MarketingListController : ApiController
    {


        [HttpPost]
        [Route("CreateNewMarketingRecord")]
        public HttpResponseMessage CreateNewMarketingRecord(clsMarketingListRecrod record)
        {
            clsMarketingListBLL NewRecord = new clsMarketingListBLL();
            NewRecord.CreateNewMarketingRecord(record);
            return GetHttpResponseType(NewRecord.status, NewRecord.message);
        }



        [HttpPost]
        [Route("EditMarketingRecord")]
        public HttpResponseMessage UpdateMarketingList(clsMarketingListRecrod record)
        {
            clsMarketingListBLL Update = new clsMarketingListBLL();
            Update.UpdateMarketingRecord(record);
            return GetHttpResponseType(Update.status, Update.message);
        }


        [HttpPost]
        [Route("LoadMarketingList")]
        public List<clsMarketingListRecrod> LoadMarketingList([FromBody]clsSearhObj search)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadMarketingList(search);
        }



        [HttpPost]
        [Route("LoadMarketingEmailTemplates")]
        public List<clsEmailTemplateList> LoadMarketingEmailTemplates()
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadMarketingEmailTemplates();
        }


        [HttpPost]
        [Route("LoadMarketingEmail")]
        public clsMarketingEmail LoadMarketingEmail([FromBody]clsMultipliIds search)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadMarketingEmail(search);
        }

        [HttpPost]
        [Route("LoadSingleMarketingRecord")]
        public clsMarketingListRecrod LoadSingleMarketingRecord([FromBody]clsId search)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadSingleMarketingRecord(search);
        }


        
        [HttpPost]
        [Route("BusinessNameExists")]
        public bool? BusinessNameExists([FromBody]clsStr search)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.BusinessNameExists(search);
        }



        [HttpPost]
        [Route("LoadMarketingNotes")]
        public string LoadMarketingNotes([FromBody]clsId search)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadMarketingNotes(search);
        }

        
        [HttpPost]
        [Route("UpdateMarketingNotes")]
        public HttpResponseMessage UpdateMarketingNotes([FromBody]clsNotes notes)
        {
            clsMarketingListBLL Update = new clsMarketingListBLL();
            Update.UpdateMarketingNotes(notes);
            return GetHttpResponseType(Update.status, Update.message);
        }

        [HttpPost]
        [Route("LoadMarketingEmailLog")]
        public List<clsMarketingEmail> LoadMarketingEmailLog([FromBody]clsId IdObj)
        {
            clsMarketingListBLL List = new clsMarketingListBLL();
            return List.LoadMarketingEmailLog(IdObj);
        }


        [Route("SendMarketingEmail")]
        public HttpResponseMessage BookListingTile([FromBody]clsMarketingEmail emailObj)
        {
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }
            clsMarketingListBLL SendEmail = new clsMarketingListBLL();
            SendEmail.SendMarketingEmail(emailObj);
            return GetHttpResponseType(SendEmail.status, SendEmail.message);
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


