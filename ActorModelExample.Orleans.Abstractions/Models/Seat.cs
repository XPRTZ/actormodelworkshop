namespace ActorModelExample.Orleans.Abstractions.Models;

[GenerateSerializer, Immutable]
public record Seat
{
    [Id(0)]
    public Guid LiveEventId { get; init; }

    [Id(1)]
    public int SeatNumber { get; init; }
}
