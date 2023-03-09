namespace ActorModelExample.Orleans.Abstractions.Models;

[GenerateSerializer, Immutable]
public record LiveEvent
{
    [Id(0)]
    public Guid Id { get; init; }

    [Id(1)]
    public string Artist { get; init; } = null!;

    [Id(1)]
    public int TotalSeats { get; init; }
}
