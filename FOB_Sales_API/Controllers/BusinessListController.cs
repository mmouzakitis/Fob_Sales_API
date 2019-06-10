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
using FOB_Sales_API.Models.Business;
using FOB_Sales_API.Models.Listings;
using FOB_Sales_API.Models.Common;
//using FOB_API.Models.Listings;

namespace FOBAdmin.Controllers.APIs
{

    //[Authorize(Roles = "Admin,Super Admin")]
    [JwtAuthentication]
    [EnableCors(origins: KeyConstantsMsgs.ADMIN_CORS_ORIGIN, headers: "*", methods: KeyConstantsMsgs.ADMIN_CORS_POST)]//https://www.fishonbooking.com
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class BusinessListController : ApiController
    {


        [HttpPost]
        [Route("CreateNewBusinessRecord")]
        public HttpResponseMessage CreateNewBusinessRecord(clsBusinessListRecrod record)
        {
            clsBusinessListBLL NewRecord = new clsBusinessListBLL();
            NewRecord.CreateNewBusinessRecord(record);
            return GetHttpResponseType(NewRecord.status, NewRecord.message);
        }



        [HttpPost]
        [Route("EditBusinessRecord")]
        public HttpResponseMessage UpdateBusinessList(clsBusinessListRecrod record)
        {
            clsBusinessListBLL Update = new clsBusinessListBLL();
            Update.UpdateBusinessRecord(record);
            return GetHttpResponseType(Update.status, Update.message);
        }


        [HttpPost]
        [Route("LoadBusinessList")]
        public List<clsBusinessListRecrod> LoadBusinessList([FromBody]clsSearhObj search)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessList(search);
        }



        [HttpPost]
        [Route("LoadBusinessEmailTemplates")]
        public List<clsEmailTemplateList> LoadBusinessEmailTemplates()
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessEmailTemplates();
        }


        [HttpPost]
        [Route("LoadBusinessEmail")]
        public clsBusinessEmail LoadBusinessEmail([FromBody]clsMultipliIds search)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessEmail(search);
        }

        [HttpPost]
        [Route("LoadSingleBusinessRecord")]
        public clsBusinessListRecrod LoadSingleBusinessRecord([FromBody]clsId search)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadSingleBusinessRecord(search);
        }


        
        [HttpPost]
        [Route("LoadBusinessEmailsForSending")]
        public string LoadBusinessEmailsForSending([FromBody]clsId ids)
        {
            if (ids.id == string.Empty)
            {
                return "";
            }
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessEmailsForSending(ids);
        }


        [HttpPost]
        [Route("BusinessNameExists")]
        public bool? BusinessNameExists([FromBody]clsStr search)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.BusinessNameExists(search);
        }



        [HttpPost]
        [Route("LoadBusinessNotes")]
        public string LoadBusinessNotes([FromBody]clsId search)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessNotes(search);
        }

        
        [HttpPost]
        [Route("UpdateBusinessNotes")]
        public HttpResponseMessage UpdateBusinessNotes([FromBody]clsNotes notes)
        {
            clsBusinessListBLL Update = new clsBusinessListBLL();
            Update.UpdateBusinessNotes(notes);
            return GetHttpResponseType(Update.status, Update.message);
        }

        [HttpPost]
        [Route("LoadBusinessEmailLog")]
        public List<clsBusinessEmail> LoadBusinessEmailLog([FromBody]clsId IdObj)
        {
            clsBusinessListBLL List = new clsBusinessListBLL();
            return List.LoadBusinessEmailLog(IdObj);
        }


        [Route("SendBusinessEmail")]
        public HttpResponseMessage BookListingTile([FromBody]clsBusinessEmail emailObj)
        {
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }
            clsBusinessListBLL SendEmail = new clsBusinessListBLL();
            SendEmail.SendBusinessEmail(emailObj);
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


