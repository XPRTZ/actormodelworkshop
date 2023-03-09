using ActorModelExample.Orleans.Abstractions.Models;

namespace ActorModelExample.Orleans.Abstractions.Grains;

public interface IVenueGrain : IGrainWithIntegerKey
{
    Task InitLiveEventAsync(LiveEvent liveEvent);
    Task<IReadOnlyCollection<LiveEvent>> GetLiveEventsAsync();
}
