using FOB_Sales_API.Controllers;
using FOB_Sales_API.Models;
using FOB_Sales_API.Models.Emails;
using FOB_Sales_API.Models.HtmlSanitizer1;
using FOB_Sales_API.Models.KeyConstants;
using FOB_Sales_API.Models.UserModels;
using Ganss.XSS;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using WebApi.Jwt;


namespace jwtTokens.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "POST")]//https://www.fishonbooking.com//https://fobadmin.azurewebsites.net/
    [AllowAnonymous]
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class TokenController : ApiController
    {     

        [AllowAnonymous]
        [HttpPost]
        [Route("loginSales")]
        public HttpResponseMessage login([FromBody]clsLoginUser user_info)
        {
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }

            //Check to see if the captcha is valid
            bool captcha_status = clsCaptcha.ValidateRecaptcha(user_info.captcha);
            if (captcha_status == false)
            {
                return GetHttpResponseType(KeyConstantsMsgs.error, KeyConstantsMsgs.captcha_failed);
            }
            else
            {
                clsAccounts Account = new clsAccounts();
                clsLoginObj loginObj = new clsLoginObj();
                loginObj = Account.LoginSalesUser(user_info);
                if (loginObj.status == KeyConstantsMsgs.success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, loginObj);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, loginObj.message);//loginObj.message
                }
            }
        }

    
        //[HttpPost]
        //[Route("GetUserInfoSmall")]
        //public clsUserInfoSmall GetUserInfoSmall([FromBody]string user_token)
        //{
        //    clsContactUsBLL messages = new clsContactUsBLL();
        //    return  messages.GetUserInfoSmall(user_token);
        //    //return GetHttpResponseType(messages.status, messages.message);
        //}

       
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
