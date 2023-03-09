namespace ActorModelExample.Orleans.Grains.States;

[GenerateSerializer, Immutable]
public record VenueState
{
    [Id(0)]
    public HashSet<Guid> LiveEvents { get; init; } = new();
}