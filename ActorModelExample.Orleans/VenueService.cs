using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.Orleans.Abstractions.Grains;

namespace ActorModelExample.Orleans;

public class VenueService : IVenueService
{
    private readonly IGrainFactory _grainFactory;

    public VenueService(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public async Task RegisterVenueAsync(Venue venue)
    {
        // we only have 1 venue for now
        var venueId = 0;
        var venueGrain = _grainFactory.GetGrain<IVenueGrain>(venueId);
        foreach (var liveEvent in venue.LiveEvents)
        {
            await venueGrain.AddLiveEventAsync(new Abstractions.Models.LiveEvent
            {
                Id = liveEvent.Id,
                Artist = liveEvent.Artist,
                TotalSeats = LiveEventConstants.EventRows * LiveEventConstants.SeatsPerRow
            });
        }
    }

    public Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        throw new NotImplementedException();
    }

    public Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId)
    {
        throw new NotImplementedException();
    }  

    public Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        throw new NotImplementedException();
    }
}