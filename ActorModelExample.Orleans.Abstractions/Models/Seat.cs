using ActorModelExample.Domain.Models;

namespace ActorModelExample.Orleans.Abstractions.Models;

[GenerateSerializer, Immutable]
public class Seat
{
    [Id(0)]
    public Guid Id { get; init; }
    [Id(1)]
    public int SeatNumber { get; init; }
    [Id(2)]
    public SeatStatus SeatStatus { get; set; }
    [Id(3)]
    public Guid LiveEventId { get; init; }
    [Id(4)]
    public Guid BookingId { get; set; }

}
