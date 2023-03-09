using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;

namespace ActorModelExample.DummyImpl.Services;

public class VenueService : IVenueService
{
    private readonly Dictionary<LiveEvent, Dictionary<SeatStatus, List<int>>> _repository;

    public VenueService()
    {
        _repository = new Dictionary<LiveEvent, Dictionary<SeatStatus, List<int>>>();
    }

    public Task RegisterVenueAsync(Venue venue)
    {
        var totalSeats = LiveEventConstants.EventRows * LiveEventConstants.SeatsPerRow;
        foreach (var liveEvent in venue.LiveEvents)
        {
            var reservedSeats = new List<int>();
            var selectedSeats = new List<int>();
            var freeSeats = new List<int>();

            for (int seatNumber = 1; seatNumber <= totalSeats; seatNumber++)
            {
                freeSeats.Add(seatNumber);
            }

            var seatInfo = new Dictionary<SeatStatus, List<int>>
            {
                { SeatStatus.Available, freeSeats },
                { SeatStatus.Reserved, selectedSeats },
                { SeatStatus.Booked, reservedSeats }
            };

            _repository.Add(liveEvent, seatInfo);
        }

        return Task.CompletedTask;
    }

    public Task<Dictionary<SeatStatus, List<int>>> GetSeatInfoAsync(LiveEvent liveEvent, Guid bookingId)
    {
        var eventSeatReservation = _repository[liveEvent];
        return Task.FromResult(eventSeatReservation);
    }

    public Task ReserveSeatAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var eventSeatReservation = _repository[liveEvent];
        var freeSeats = eventSeatReservation[SeatStatus.Available];
        var selectedSeats = eventSeatReservation[SeatStatus.Reserved];

        freeSeats.Remove(seatNumber);
        selectedSeats.Add(seatNumber);

        return Task.CompletedTask;
    }

    public Task CancelSeatReservationAsync(LiveEvent liveEvent, Guid bookingId, int seatNumber)
    {
        var eventSeatReservation = _repository[liveEvent];
        var freeSeats = eventSeatReservation[SeatStatus.Available];
        var selectedSeats = eventSeatReservation[SeatStatus.Reserved];

        freeSeats.Add(seatNumber);
        selectedSeats.Remove(seatNumber);

        return Task.CompletedTask;
    }

    public Task ConfirmBookingAsync(LiveEvent liveEvent, Guid bookingId, string name, IEnumerable<int> confirmedSeats)
    {
        var eventSeatReservation = _repository[liveEvent];
        var selectedSeats = eventSeatReservation[SeatStatus.Reserved];
        var reservedSeats = eventSeatReservation[SeatStatus.Booked];

        var list = confirmedSeats.ToList();
        foreach (var seat in list)
        {
            selectedSeats.Remove(seat);
        }

        reservedSeats.AddRange(list);

        return Task.CompletedTask;
    }
}
