using ActorModelExample.Orleans.Abstractions.Models;

namespace ActorTestProject.Abstractions.Grains;

public interface IShoppingCartGrain : IGrainWithGuidKey
{
    Task<IReadOnlyCollection<Seat>> GetAllItemsAsync();
    Task AddSeatAsync(Seat seat);
    Task RemoveSeatAsync(Seat seat);
    Task ClearCartAsync();
    Task CheckoutAsync();
}
