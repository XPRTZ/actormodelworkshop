using ActorModelExample.Domain.Models;
using ActorModelExample.Orleans.Abstractions.Models;
using ActorModelExample.Orleans.Grains.States;
using ActorTestProject.Abstractions.Grains;
using Orleans.Providers;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class SeatGrain : Grain<SeatState>, ISeatGrain
{
    public Task CreateSeatAsync(Seat seat)
    {
        if (State.IsCreated)
        {
            throw new InvalidOperationException("Seat has already been created.");
        }

        var seatDetails = new SeatState
        {
            BookingStatus = SeatStatus.Available,
            LiveEventGuid = seat.LiveEventId,
            SeatNumber = seat.SeatNumber,
            IsCreated = true
        };
        return UpdateStateAsync(seatDetails);
    }

    public Task<SeatStatus> GetBookingStatusAsync(Guid bookingId)
    {
        var state = GetState();
        return state.BookingStatus == SeatStatus.Reserved && bookingId != state.BookingId
            ? Task.FromResult(SeatStatus.Booked)
            : Task.FromResult(state.BookingStatus);
    }

    public async Task BookAsync(Guid bookingId)
    {
        var seat = GetState();
        if (seat.BookingStatus != SeatStatus.Reserved)
        {
            throw new InvalidOperationException("Unable to book seat, only seats that are already reserved can be booked.");
        }
        if (seat.BookingId != bookingId)
        {
            throw new InvalidOperationException("Unable to book seat, only seats that are reserved for the current booking can be booked.");
        }
        await UpdateStateAsync(seat with { BookingStatus = SeatStatus.Booked });

        var liveEventGrain = GrainFactory.GetGrain<ILiveEventGrain>(seat.LiveEventGuid);
        await liveEventGrain.BookSeatAsync();
    }

    public async Task CancelReservationAsync(Guid bookingId)
    {
        var seat = GetState();
        if (seat.BookingStatus != SeatStatus.Reserved)
        {
            throw new InvalidOperationException("Unable to cancel seat reservation, only seats that are reserved can be cancelled.");
        }
        if (seat.BookingId != bookingId)
        {
            throw new InvalidOperationException("Unable to cancel seat reservation, only seats that are reserved for the current booking can be cancelled.");
        }
        await UpdateStateAsync(seat with { BookingStatus = SeatStatus.Available });
    }

    public async Task<bool> TryReserveAsync(Guid bookingId)
    {
        var seat = GetState();
        if (seat.BookingStatus != SeatStatus.Available)
        {
            return false;

        }
        await UpdateStateAsync(seat with
        {
            BookingId = bookingId,
            BookingStatus = SeatStatus.Reserved
        });
        return true;
    }

    private SeatState GetState()
    {
        return State.IsCreated
            ? State
            : throw new InvalidOperationException("Seat has not been initialized.");
    }

    private Task UpdateStateAsync(SeatState seat)
    {
        State = seat;
        return WriteStateAsync();
    }
}
