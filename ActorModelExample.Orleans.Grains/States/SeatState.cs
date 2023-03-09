using ActorModelExample.Domain.Models;

namespace ActorModelExample.Orleans.Grains.States;

[GenerateSerializer, Immutable]
public record SeatState
{
    [Id(0)]
    public Guid LiveEventGuid { get; init; }

    [Id(1)]
    public Guid BookingId { get; init; }

    [Id(2)]
    public int SeatNumber { get; init; }

    [Id(3)]
    public SeatStatus BookingStatus { get; init; }

    [Id(4)]
    public bool IsCreated { get; init; }
}
