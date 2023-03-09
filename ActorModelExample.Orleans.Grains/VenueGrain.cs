using ActorModelExample.Orleans.Abstractions.Grains;
using ActorModelExample.Orleans.Abstractions.Models;
using ActorModelExample.Orleans.Grains.States;
using ActorTestProject.Abstractions.Grains;
using Orleans.Providers;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class VenueGrain : Grain<VenueState>, IVenueGrain
{
    public async Task AddLiveEventAsync(LiveEvent liveEvent)
    {
        State.LiveEvents.Add(liveEvent.Id);
        await WriteStateAsync();

        var liveEventGrain = GrainFactory.GetGrain<ILiveEventGrain>(liveEvent.Id);
        await liveEventGrain.CreateAsync(liveEvent);
    }

    public async Task<IReadOnlyCollection<LiveEvent>> GetLiveEventsAsync()
    {
        var result = new List<LiveEvent>(State.LiveEvents.Count);
        foreach (var id in State.LiveEvents)
        {
            // this should be cached in real life
            var liveEvent = GrainFactory.GetGrain<ILiveEventGrain>(id);
            var details = await liveEvent.GetDetailsAsync();
            result.Add(details);
        }
        return result.AsReadOnly();
    }
}
