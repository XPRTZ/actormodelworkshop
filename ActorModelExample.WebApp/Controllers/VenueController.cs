using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.WebApp.Requests;
//using ActorModelExample.Orleans.Abstractions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActorModelExample.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private IVenueService _venueService;
        private Venue _venue;

        public VenueController(IVenueService venueService, Venue venue)
        {
            _venueService = venueService;
            _venue = venue;
        }

        [HttpGet("LiveEvents")]
        public ActionResult<LiveEvent[]> GetLiveEvents()
        {
            return Ok(_venue.LiveEvents.ToArray());
        }

        [HttpGet("NumberOfSeats")]
        public ActionResult<int> GetNumberOfSeats()
        {
            return LiveEventConstants.EventRows * LiveEventConstants.SeatsPerRow;
        }

        [HttpPost("ReserveSeat")]
        public async Task<ActionResult> ReserveSeat(ReserveRequest reserveRequest)
        {
            try
            {
                await _venueService.ReserveSeatAsync(reserveRequest.LiveEvent, reserveRequest.BookingId, reserveRequest.Seat);
                return Ok();
            }
            catch
            {
                return BadRequest("Bad...");
            }
        }

        [HttpPost("Confirm")]
        public async Task<ActionResult> ConfirmSeat(ConfirmRequest confirmRequest)
        {
            try
            {
                await _venueService.ConfirmBookingAsync(confirmRequest.LiveEvent, confirmRequest.BookingId, $"Stresser_{Guid.NewGuid()}", confirmRequest.Seats);
                return Ok();
            }
            catch
            {
                return BadRequest("Bad...");
            }
        }

        [HttpGet("GetSeatStatus/{eventId}/{bookingId}")]
        public async Task<ActionResult<Dictionary<SeatStatus, List<int>>>> GetSeatStatus(Guid eventId, Guid bookingId)
        {
            try
            {
                var liveEvent = _venue.LiveEvents.FirstOrDefault(le => le.Id == eventId);
                return Ok(await _venueService.GetSeatInfoAsync(liveEvent, bookingId));
            }
            catch
            {
                return BadRequest("Bad...");
            }
        }
    }
    
}
