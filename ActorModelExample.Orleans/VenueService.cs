using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.Orleans.Abstractions.Grains;
using ActorModelExample.Orleans.Grains;

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
            await venueGrain.InitLiveEventAsync(new Abstractions.Models.LiveEvent
            {
                Id = liveEvent.Id,
                Artist = liveEvent.Artist,
                TotalSeats = LiveEventConstants.EventRows * LiveEventConstants.SeatsPerRow
            });
        }
    }

    public async Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var grain = _grainFactory.GetGrain<ISeatGrain>(liveEvent.Id.ToString() + "-" + seatNumber);
        await grain.RemoveSeatAsync(bookingId);
    }

    public async Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats)
    {
        foreach (var confirmedSeat in confirmedSeats)
        {
            var grain = _grainFactory.GetGrain<ISeatGrain>(liveEvent.Id.ToString() + "-" + confirmedSeat);
            await grain.ConfirmSeatAsync(bookingId);
        }
    }

    public async Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId)
    {
        var result = new Dictionary<SeatStatus, List<int>>();

        result.Add(SeatStatus.Available, new List<int>());
        result.Add(SeatStatus.Booked, new List<int>());
        result.Add(SeatStatus.Reserved, new List<int>());

        var venueGrain = _grainFactory.GetGrain<IVenueGrain>(0);
        var liveEvents = await venueGrain.GetLiveEventsAsync();
        var le = liveEvents.FirstOrDefault(x => x.Id == liveEvent.Id);
        for(int i = 1; i <= le.TotalSeats; i++)
        {
            var seatGrain = _grainFactory.GetGrain<ISeatGrain>(liveEvent.Id.ToString() + "-" + i);

            var seat = await seatGrain.GetSeatAsync();

            result[seat.SeatStatus].Add(seat.SeatNumber);
        }

        return result;
    }  

    public async Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var grain = _grainFactory.GetGrain<ISeatGrain>(liveEvent.Id.ToString() + "-" + seatNumber);
        await grain.ReserveSeatAsync(bookingId);
    }
}
