using ActorModelExample.Orleans.Abstractions.Grains;
using ActorModelExample.Orleans.Abstractions.Models;
using FluentAssertions;
using Orleans.TestingHost;

namespace ActorModelExample.Orleans.Grains.Tests;

[Collection(ClusterCollection.Name)]
public class VenueGrainTests
{
    private readonly TestCluster _cluster;

    public VenueGrainTests(ClusterFixture fixture)
    {
        _cluster = fixture.Cluster;
    }

    [Fact]
    public async Task AddLiveEventAsync_()
    {
        // arrange
        var venueGrain = _cluster.GrainFactory.GetGrain<IVenueGrain>(0);
        var liveEvent = new LiveEvent
        {
            Id = Guid.NewGuid(),
            Artist = "TestArtist",
            TotalSeats = 5
        };

        // act
        await venueGrain.AddLiveEventAsync(liveEvent);

        // assert
        var liveEvents = await venueGrain.GetLiveEventsAsync();
        liveEvents.Should().ContainEquivalentOf(liveEvent);
    }
}