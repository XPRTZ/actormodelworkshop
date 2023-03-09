using ActorModelExample.Orleans.Abstractions.Models;
using ActorTestProject.Abstractions.Grains;
using Orleans.Providers;
using Orleans.Runtime;

namespace ActorModelExample.Orleans.Grains;

[StorageProvider(ProviderName = "ticket-system")]
public class ShoppingCartGrain : Grain<List<Seat>>, IShoppingCartGrain, IRemindable
{
    public async Task AddSeatAsync(Seat seat)
    {
        var seatGrain = GetSeatGrain(seat);
        var isReserved = await seatGrain.TryReserveAsync(this.GetPrimaryKey());
        if (!isReserved)
        {
            throw new InvalidOperationException("Seat reservation failed.");
        }

        State.Add(seat);
        await WriteStateAsync();

        var reminder = await this.GetReminder("Reservation");
        if (reminder is null)
        {
            await this.RegisterOrUpdateReminder("Reservation", TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60));
        }
    }
    public async Task RemoveSeatAsync(Seat seat)
    {
        await RemoveAsync(seat);
        await WriteStateAsync();
    }

    public async Task ClearCartAsync()
    {
        foreach (var seat in State.ToArray())
        {
            await RemoveAsync(seat);
        }
        await WriteStateAsync();

        await this.UnregisterReminder(await this.GetReminder("Reservation"));
    }

    public Task<IReadOnlyCollection<Seat>> GetAllItemsAsync()
    {
        IReadOnlyCollection<Seat> cartItems = State.AsReadOnly();
        return Task.FromResult(cartItems);
    }

    public async Task CheckoutAsync()
    {
        foreach (var seat in State.ToArray())
        {
            var seatGrain = GetSeatGrain(seat);
            await seatGrain.BookAsync(this.GetPrimaryKey());
        }
        State.Clear();

        await WriteStateAsync();

        await this.UnregisterReminder(await this.GetReminder("Reservation"));
    }
    async Task IRemindable.ReceiveReminder(string reminderName, TickStatus status)
    {
        await ClearCartAsync();
    }

    private async Task RemoveAsync(Seat seat)
    {
        var seatGrain = GetSeatGrain(seat);
        await seatGrain.CancelReservationAsync(this.GetPrimaryKey());

        State.Remove(seat);
    }

    private ISeatGrain GetSeatGrain(Seat seat)
    {
        return GrainFactory.GetGrain<ISeatGrain>(seat.SeatNumber, seat.LiveEventId.ToString());
    }
}