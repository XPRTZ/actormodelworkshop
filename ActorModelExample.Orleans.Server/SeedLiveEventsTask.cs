using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using Orleans.Runtime;

namespace ActorModelExample.Orleans.Server;

internal class SeedLiveEventsTask : IStartupTask
{
    private readonly IVenueService _venueService;
    private readonly Venue _venue;

    public SeedLiveEventsTask(IVenueService venueService, Venue venue)
    {
        _venueService = venueService;
        _venue = venue;
    }

    public Task Execute(CancellationToken cancellationToken)
    {
        return _venueService.RegisterVenueAsync(_venue);
    }
}