using ActorModelExample.AkkaNet.Actors;
using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Bogus;

namespace ActorModelExample.AkkaNet.Tests.Actors;

public class VenueActorShould : TestKit
{
    private readonly Faker _faker;
    private readonly Venue _venue;

    public VenueActorShould()
    {
        _faker = new Faker("nl");

        var liveEventList = new List<LiveEvent>();
        var liveEvent = new LiveEvent(
            Guid.NewGuid(),
            _faker.Date.Between(DateTime.Now, DateTime.Now.AddYears(1)),
            _faker.Person.FullName,
            _faker.Image.PicsumUrl(),
            _faker.Random.Int(25, 150)
            );

        liveEventList.Add(liveEvent);

        _venue = new Venue(
            LiveEventConstants.VenueName,
            LiveEventConstants.VenueLocation,
            liveEventList);
    }

    private IActorRef CreateActorRef(Venue venue)
    {
        return Sys.ActorOf(VenueActor.Properties(venue));
    }

}
