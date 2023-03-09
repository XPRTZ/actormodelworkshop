using Orleans.Providers;
using ActorModelExample.Domain.Models;
using ActorModelExample.Orleans.Abstractions.Models;

namespace ActorModelExample.Orleans.Grains;

public interface ISeatGrain : IGrainWithStringKey
{
    Task InitSeatAsync(int seatId, Guid liveEventId);

    Task<Seat> GetSeatAsync();

    Task ReserveSeatAsync(Guid bookingId);

    Task RemoveSeatAsync(Guid bookingId);

    Task ConfirmSeatAsync(Guid bookingId);
}

[StorageProvider(ProviderName = "ticket-system")]
public class SeatGrain : Grain<Seat>, ISeatGrain
{
    public async Task InitSeatAsync(int seatNumber, Guid liveEventId)
    {
        State = new Seat
        {
            Id = Guid.NewGuid(),
            SeatNumber = seatNumber,
            SeatStatus = SeatStatus.Available,
            LiveEventId = liveEventId,
        };
        await WriteStateAsync();
    }

    public Task<Seat> GetSeatAsync()
    {
        return Task.FromResult(State);
    }

    public async Task ReserveSeatAsync(Guid bookingId)
    {
        if (State.SeatStatus == SeatStatus.Available)
        {
            State.SeatStatus = SeatStatus.Reserved;
            State.BookingId = bookingId;
            await WriteStateAsync();
        }
        else if (State.SeatStatus == SeatStatus.Reserved && State.BookingId == bookingId)
        {
            State.SeatStatus = SeatStatus.Available;
            State.BookingId = default;
            await WriteStateAsync();
        }
    }

    public async Task RemoveSeatAsync(Guid bookingId)
    {
        if (State.SeatStatus == SeatStatus.Reserved && State.BookingId == bookingId)
        {
            State.SeatStatus = SeatStatus.Available;
            State.BookingId = default;
            await WriteStateAsync();
        }
    }

    public async Task ConfirmSeatAsync(Guid bookingId)
    {
        if (State.SeatStatus == SeatStatus.Reserved && State.BookingId == bookingId)
        {
            State.SeatStatus = SeatStatus.Booked;
            await WriteStateAsync();
        }
    }
}
