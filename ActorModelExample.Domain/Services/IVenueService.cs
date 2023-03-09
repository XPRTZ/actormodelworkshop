using ActorModelExample.Domain.Models;

namespace ActorModelExample.Domain.Services;

public interface IVenueService
{
    Task RegisterVenueAsync(Venue venue);
    Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId);

    Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber);
    Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber);
    Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats);
}