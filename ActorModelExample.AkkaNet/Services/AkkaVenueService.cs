using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;

namespace ActorModelExample.AkkaNet.Services;

public class AkkaVenueService : IVenueService
{
    private readonly IActorBridge _bridge;

    // remove venue
    public AkkaVenueService(IActorBridge bridge)
    {
        _bridge = bridge;
    }

    public Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        // todo: _ = await _bridge.Ask(request);
        throw new NotImplementedException();
    }

    public Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats)
    {
        // todo: _ = await _bridge.Ask(request);
        throw new NotImplementedException();
    }

    public Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId)
    {
        // todo: _ = await _bridge.Ask(request);
        throw new NotImplementedException();
    }

    public Task RegisterVenueAsync(Venue venue)
    {
        // not possible, because background service is not running.
        // And so no actors are available.
        return Task.CompletedTask;
    }

    public Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        // todo: _ = await _bridge.Ask(request);
        throw new NotImplementedException();
    }
}
