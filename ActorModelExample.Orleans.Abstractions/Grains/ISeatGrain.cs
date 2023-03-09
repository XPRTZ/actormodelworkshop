using ActorModelExample.Domain.Models;
using ActorModelExample.Orleans.Abstractions.Models;
namespace ActorTestProject.Abstractions.Grains;

public interface ISeatGrain : IGrainWithIntegerCompoundKey
{
    Task<SeatStatus> GetBookingStatusAsync(Guid bookingId);
    Task<bool> TryReserveAsync(Guid bookingId);
    Task CancelReservationAsync(Guid bookingId);
    Task BookAsync(Guid bookingId);
    Task CreateSeatAsync(Seat seat);
}
