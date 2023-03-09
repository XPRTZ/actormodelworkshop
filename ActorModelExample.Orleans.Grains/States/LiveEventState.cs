namespace ActorModelExample.Orleans.Grains.States;

[GenerateSerializer, Immutable]
public record LiveEventState
{
    [Id(0)]
    public Guid Id { get; init; }

    [Id(1)]
    public string Name { get; init; } = null!;

    [Id(2)]
    public int TotalSeats { get; init; }

    [Id(3)]
    public int AvailableSeats { get; init; }

    [Id(4)]
    public bool IsCreated { get; init; }
}
