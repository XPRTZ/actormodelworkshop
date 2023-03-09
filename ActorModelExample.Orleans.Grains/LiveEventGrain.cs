using ActorModelExample.Orleans.Abstractions.Models;
using ActorModelExample.Orleans.Grains.States;
using ActorTestProject.Abstractions.Grains;
using Orleans.Providers;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class LiveEventGrain : Grain<LiveEventState>, ILiveEventGrain
{
    public async Task CreateAsync(LiveEvent liveEvent)
    {
        if (State.IsCreated)
        {
            throw new InvalidOperationException("Seat has already been created.");
        }

        var id = this.GetPrimaryKey();

        for (var seatNr = 1; seatNr <= liveEvent.TotalSeats; seatNr++)
        {
            var seatGrain = GrainFactory.GetGrain<ISeatGrain>(seatNr, id.ToString());
            await seatGrain.CreateSeatAsync(new Seat { LiveEventId = id, SeatNumber = seatNr });
        }

        await UpdateStateAsync(new LiveEventState
        {
            Id = id,
            Name = liveEvent.Artist,
            TotalSeats = liveEvent.TotalSeats,
            AvailableSeats = liveEvent.TotalSeats,
            IsCreated = true
        });
    }

    public Task BookSeatAsync()
    {
        if (State.AvailableSeats == 0)
        {
            throw new InvalidOperationException("This should never happen, more seats booked than available.");
        }
        var availableSeats = State.AvailableSeats - 1;
        return UpdateStateAsync(State with { AvailableSeats = availableSeats });
    }

    public Task<bool> IsSoldOutAsync()
    {
        return Task.FromResult(State.AvailableSeats == 0);
    }

    private Task UpdateStateAsync(LiveEventState liveEvent)
    {
        State = liveEvent;
        return WriteStateAsync();
    }

    public Task<LiveEvent> GetDetailsAsync()
    {
        return Task.FromResult(new LiveEvent
        {
            Id = State.Id,
            Artist = State.Name,
            TotalSeats = State.TotalSeats,
        });
    }
}
