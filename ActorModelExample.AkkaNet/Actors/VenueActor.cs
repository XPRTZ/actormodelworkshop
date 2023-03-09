using ActorModelExample.Domain.Models;
using Akka.Actor;

namespace ActorModelExample.AkkaNet.Actors;

public class VenueActor : UntypedActor
{
    public VenueActor(Venue venue)
    {
    }

    public static Props Properties(Venue venue) =>
        Props.Create(() => new VenueActor(venue));

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            default:
                Unhandled(message);
                break;
        }
    }
}