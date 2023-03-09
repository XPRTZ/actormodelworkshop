using ActorModelExample.Orleans.Abstractions.Grains;
using ActorModelExample.Orleans.Abstractions.Models;
using Orleans.Providers;
using System.Collections.Generic;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class VenueGrain : Grain<List<LiveEvent>>, IVenueGrain
{
    public async Task InitLiveEventAsync(LiveEvent liveEvent)
    {
        State.Add(liveEvent);
        await WriteStateAsync();

        for (int i = 1; i <= liveEvent.TotalSeats; i++)
        {
            var seatGrain = GrainFactory.GetGrain<ISeatGrain>(liveEvent.Id.ToString()+"-"+i);
            await seatGrain.InitSeatAsync(i, liveEvent.Id);
        }
    }

    public Task<IReadOnlyCollection<LiveEvent>> GetLiveEventsAsync()
    {
        IReadOnlyCollection<LiveEvent> liveEvents = State;
        return Task.FromResult(liveEvents);
    }
}
