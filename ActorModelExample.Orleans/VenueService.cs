using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.Orleans.Abstractions.Grains;
using ActorTestProject.Abstractions.Grains;

namespace ActorModelExample.Orleans;

public class VenueService : IVenueService
{
    private readonly IGrainFactory _grainFactory;

    public VenueService(IGrainFactory grainFactory)
    {
        _grainFactory = grainFactory;
    }

    public Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var shoppingCartGrain = GetShoppingCartGrain(bookingId);
        return shoppingCartGrain.RemoveSeatAsync(new Abstractions.Models.Seat
        {
            LiveEventId = liveEvent.Id,
            SeatNumber = seatNumber
        });
    }

    public Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats)
    {
        var shoppingCartGrain = GetShoppingCartGrain(bookingId);
        return shoppingCartGrain.CheckoutAsync();
    }

    public async Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId)
    {
        var result = new Dictionary<SeatStatus, List<int>>()
        {
            { SeatStatus.Available, new() },
            { SeatStatus.Reserved, new() },
            { SeatStatus.Booked, new() }
        };

        // this implementation sucks performance wise but it works
        var totalSeats = LiveEventConstants.EventRows * LiveEventConstants.SeatsPerRow;
        for (var seatNr = 1; seatNr <= totalSeats; seatNr++)
        {
            var seatGrain = _grainFactory.GetGrain<ISeatGrain>(seatNr, liveEvent.Id.ToString());
            var status = await seatGrain.GetBookingStatusAsync(bookingId);
            result[status].Add(seatNr);
        }
        return result;
    }

    public async Task RegisterVenueAsync(Venue venue)
    {
        // we only have 1 venue for now
        var venueGrain = _grainFactory.GetGrain<IVenueGrain>(0);
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

    public Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var shoppingCartGrain = GetShoppingCartGrain(bookingId);
        return shoppingCartGrain.AddSeatAsync(new Abstractions.Models.Seat
        {
            LiveEventId = liveEvent.Id,
            SeatNumber = seatNumber
        });
    }

    private IShoppingCartGrain GetShoppingCartGrain(Guid bookingId)
    {
        return _grainFactory.GetGrain<IShoppingCartGrain>(bookingId);
    }
}