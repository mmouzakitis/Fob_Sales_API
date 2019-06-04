using FOB_Sales_API.Models;
using FOB_Sales_API.Models.Booking;
using FOB_Sales_API.Models.KeyConstants;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Jwt.Filters;

namespace FOBAdmin.Controllers.APIs
{

    //[Authorize(Roles = "Admin,Super Admin")]
    [JwtAuthentication]
    [EnableCors(origins: KeyConstantsMsgs.ADMIN_CORS_ORIGIN, headers: "*", methods: KeyConstantsMsgs.ADMIN_CORS_POST)]//https://www.fishonbooking.com
    [RoutePrefix(KeyConstants.website_route_prefix)]
    public class BookingsController : ApiController
    {

        [HttpPost]
        [Route("EmailBookingDetails")]
        public HttpResponseMessage EmailBookingDetails([FromBody]string booking_token)
        {
            clsBookingBLL bookings = new clsBookingBLL();
            bookings.EmailBookingDetails(booking_token);
            return GetHttpResponseType(bookings.status, bookings.message);
        }



      

        [HttpPost]
        [AllowAnonymous]
        [Route("GetPreBookingDetails")]//clsPreBookingSearch
        public clsBookingDetails GetPreBookingDetails([FromBody]clsPreBookingSearch PreBookingObj)//clPreBookObj
        {
            clsBookingBLL bookings = new clsBookingBLL();
            return bookings.LoadPreBookingDetails(PreBookingObj);
        }

        
        [HttpPost]
        [Route("AllBookingsByDate")]
        public List<clsBookingTotals> AllBookingsByDate([FromBody]clsBookingStatsSearch dateObj)
        {
            clsBookingBLL bookings = new clsBookingBLL();
            return bookings.AllBookingsByDate(dateObj);
        }

        [HttpPost]
        [Route("AllBookingsByMonth")]
        public List<clsBookingTotals> AllBookingsByMonth([FromBody]clsBookingStatsSearch dateObj)
        {
            clsBookingBLL bookings = new clsBookingBLL();
            return bookings.AllBookingsByMonth(dateObj);
        }
      

        [HttpPost]
		[Route("GetCompletedBookings")]
		public List<clsBooking> GetCompletedBookings([FromBody]string account_token)
		{
			clsBookingBLL bookings = new clsBookingBLL();
			return bookings.LoadBookingsByAccountId(account_token,KeyConstants.completed);
		}


		[HttpPost]
        [Route("GetActiveBookings")]
        public List<clsBooking> GetActiveBookings([FromBody]string account_token)
        {
            clsBookingBLL bookings = new clsBookingBLL();
            return bookings.LoadBookingsByAccountId(account_token,KeyConstants.not_completed);
        }

        [HttpPost]
        [Route("GetBookingDetails")]
        public clsBookingDetails GetBookingDetails([FromBody]string booking_token)
        {
            clsBookingBLL bookings = new clsBookingBLL();
            return bookings.LoadBookingDetails(booking_token);
        }

        [HttpPost]
        [Route("GetCalendarBookings")]
        public List<clsCalendarBooking> GetCalendarBookings([FromBody]clsSearchCalendar search_by_dates)
        {
            clsBookingBLL LoadDetails = new clsBookingBLL();
            return LoadDetails.GetCalendarBookings(search_by_dates);
        }


        [HttpPost]
        [Route("SearchListingBookings")]
        public List<clsBooking> SearchListingBookings([FromBody]clsSearchBookings search)
        {
            clsBookingBLL Bookings = new clsBookingBLL();
            return Bookings.LoadBookingsBySearch(search);
        }


        //[HttpPost]
        //[Route("GetBookingSlots")]
        //public List<clsBookingSlot> GetBookingSlots([FromBody]clsSearchSlot search)
        //{
        //    clsBookingSlotBLL Bookings = new clsBookingSlotBLL();
        //    return Bookings.LoadBookingSlots(search);
        //}


        [HttpPost]
        [Route("CancelBookingByUser")]
        public HttpResponseMessage CancelBookingByUser(clsCancelPurchase cancel_booking)
        {
            cancel_booking.lookup_id = KeyConstants.canceled_by_owner.ToString();
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }
            clsBookingBLL Booking = new clsBookingBLL();
            Booking.CancelBooking(cancel_booking);
            if (Booking.status == KeyConstantsMsgs.success)
            {
                return Request.CreateResponse(HttpStatusCode.OK,Booking.message);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Booking.message);
            }
            //return GetHttpResponseType(Booking.status, Booking.message);
        }


        [HttpPost]
        [Route("CancelBookingByCaptain")]
        public HttpResponseMessage CancelBookingByCaptain(clsCancelPurchase cancel_booking)
        {
            cancel_booking.lookup_id = KeyConstants.canceled_by_captain.ToString();
            if (ModelState.IsValid == false)
            {
                clsModelErrorCollection ErrorColl = new clsModelErrorCollection();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorColl.GetModelStateErrors(ModelState));
            }
            clsBookingBLL Booking = new clsBookingBLL();
            Booking.CancelBooking(cancel_booking);
            if (Booking.status == KeyConstantsMsgs.success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, Booking.message);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Booking.message);
            }
            //return GetHttpResponseType(Booking.status, Booking.message);
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
