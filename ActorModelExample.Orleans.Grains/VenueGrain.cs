using ActorModelExample.Orleans.Abstractions.Grains;
using ActorModelExample.Orleans.Abstractions.Models;
using Orleans.Providers;
using System.Collections.Generic;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class VenueGrain : Grain<HashSet<Guid>>, IVenueGrain
{
    public async Task AddLiveEventAsync(LiveEvent liveEvent)
    {
        State.Add(liveEvent.Id);
        await WriteStateAsync();

        // TODO rest of the method
    }

    public Task<IReadOnlyCollection<LiveEvent>> GetLiveEventsAsync()
    {
        // TODO implement me
        IReadOnlyCollection<LiveEvent> liveEvents = Array.Empty<LiveEvent>();
        return Task.FromResult(liveEvents);
    }
}
