using ActorModelExample.Orleans.Abstractions.Models;

namespace ActorTestProject.Abstractions.Grains;

public interface ILiveEventGrain : IGrainWithGuidKey
{
    Task CreateAsync(LiveEvent liveEvent);
    Task<bool> IsSoldOutAsync();
    Task BookSeatAsync();
    Task<LiveEvent> GetDetailsAsync();
}
