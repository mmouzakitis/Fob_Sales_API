using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FOB_Sales_API.Models.KeyConstants;
using FOB_Sales_API.Models.Listings;
using WebApi.Jwt.Filters;
using FOB_Sales_API.Models;
using FOB_Sales_API.Models.Coupons;

namespace FOBAdmin.Controllers.APIs
{

    //[Authorize(Roles = "Admin,Super Admin")]
    [JwtAuthentication]
    [EnableCors(origins: KeyConstantsMsgs.ADMIN_CORS_ORIGIN, headers: "*", methods: KeyConstantsMsgs.ADMIN_CORS_POST)]//https://www.fishonbooking.com
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class PromotionController : ApiController
    {
        

        [HttpPost]
        [Route("VerifyCoupon")]
        public bool CouponIsValid([FromBody]clsApplyCoupon coupon)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.CouponIsValid(coupon.coupon);
        }

        [HttpPost]
        [Route("LoadUsedCouponDetails")]
        public List<clsUsedCouponDetails> LoadUsedCouponDetails([FromBody]string listing_id)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.LoadUsedCouponDetails(listing_id);
        }
        
        [HttpPost]
        [Route("LoadPromoCodeHolders")]
        public List<clsPromoCodeHolders> LoadPromoCodeHolders()
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.LoadPromoCodeHolders();
        }

        [HttpPost]
        [Route("LoadMyPromoCodes")]
        public List<clsMyCoupon> LoadMyPromoCodes([FromBody]string account_id)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.LoadMyPromoCodes(account_id);
        }

        [HttpPost]
        [Route("LoadUserPromoCodes")]
        public List<clsMyCoupon> LoadUserPromoCodes([FromBody]string account_id)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.LoadMyPromoCodes(account_id);
        }

        [HttpPost]
        [Route("CommissionDetailsByDate")]
        public List<clsCommissionDetails> CommissionDetailsByDate([FromBody] clsSerchCommissionByDate promo_id)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.CommissionDetailsByDate(promo_id);
        }

        [HttpPost]
        [Route("CommissionByDate")]
        public List<clsCommissionAggregate> CommissionByDate([FromBody]clsSearchAggCoupons aggregate_search)
        {
            clsCouponBLL coupons = new clsCouponBLL();
            return coupons.CommissionByDate(aggregate_search);
        }

        [HttpPost]
        [Route("ApplyCoupon")]
        public HttpResponseMessage ApplyCoupon([FromBody]clsApplyCoupon coupons_obj)
        {
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }
            clsCouponBLL coupons = new clsCouponBLL();
            coupons.ApplyCoupon(coupons_obj);
            return GetHttpResponseType(coupons.status, coupons.message);
        }


        [HttpPost]
        [Route("GetPromotionalTally")]
        public clsCouponTally GetPromotionalTally([FromBody]clsListToken coupons_obj)
        {
            clsCouponBLL coupon = new clsCouponBLL();
            return coupon.GetPromotionalTally(coupons_obj.l_token);
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



        //[HttpPost]
        //[Route("GetAvailableCoupons")]
        //public List<clsCoupon> GetAvailableCoupons([FromBody]clsAccToken get_coupon)
        //{
        //    clsCouponBLL coupon = new clsCouponBLL();
        //    return coupon.GetAvailableCoupons(get_coupon.acc_token);
        //}


        //[HttpPost]
        //[Route("EmailCouponTo")]
        //public HttpResponseMessage EmailCouponTo([FromBody]clsEmailCoupon coupon_obj)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
        //    }
        //    clsCouponBLL coupons = new clsCouponBLL();
        //    coupons.EmailCouponTo(coupon_obj);
        //    return GetHttpResponseType(coupons.status, coupons.message);
        //}

    }
}


