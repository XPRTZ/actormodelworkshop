using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.WebApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;

namespace ActorModelExample.WebApp.Pages;

public partial class Reservation
{
    private BookingModel _booking = new();

    [Inject]
    public ProtectedSessionStorage Session { get; set; } = null!;

    [Inject]
    public IVenueService Service { get; set; } = null!;

    [Inject]
    public Venue Venue { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public string ParamId { get; set; } = string.Empty;

    [Parameter]
    public string ParamBookingId { get; set; } = string.Empty;

    public LiveEvent LiveEvent { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        var bookingId = new Guid(ParamBookingId);
        var id = new Guid(ParamId);
        if (!Venue.LiveEvents.Any(e => e.Id == id))
        {
            NavigationManager.NavigateTo(PageConstants.Index);
            return;
        }

        LiveEvent = Venue.LiveEvents.First(e => e.Id == id);
        _booking.Id = bookingId;

        await UpdateSeatInfo();
    }

    private IEnumerable<int> ReserveSeats { get; set; } = Array.Empty<int>();

    private IEnumerable<int> BookedSeats { get; set; } = Array.Empty<int>();

    private async Task HandleValidSubmitAsync()
    {
        if (!ReserveSeats.Any())
        {
            return;
        }

        await Service.ConfirmBookingAsync(LiveEvent, _booking.Id, _booking.Name, ReserveSeats);

        NavigationManager.NavigateTo(PageConstants.Index);
    }

    private async Task CancelReservationAsync(MouseEventArgs _, int seatNumber)
    {
        await Service.CancelSeatReservationAsync(LiveEvent, _booking.Id, seatNumber);

        await UpdateSeatInfo();
    }

    private async Task ReserveSeatAsync(int seatNumber)
    {
        await Service.ReserveSeatAsync(LiveEvent, _booking.Id, seatNumber);

        await UpdateSeatInfo();
    }

    private async Task UpdateSeatInfo()
    {
        var seatInfo = await Service.GetSeatInfoAsync(LiveEvent, _booking.Id);
        ReserveSeats = GetSeatByStatus(seatInfo, SeatStatus.Reserved);
        BookedSeats = GetSeatByStatus(seatInfo, SeatStatus.Booked);
    }

    private static IEnumerable<int> GetSeatByStatus(IDictionary<SeatStatus, List<int>> seatInfo, SeatStatus status)
    {
        if (seatInfo.TryGetValue(status, out var value))
        {
            return value;
        }

        return Array.Empty<int>();
    }

}
