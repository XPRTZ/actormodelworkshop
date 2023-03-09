using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ActorModelExample.WebApp.Pages.Components;

public partial class SeatSelectionComponent
{
    [Parameter]
    public IEnumerable<int> ReserveSeats { get; set; } = Array.Empty<int>();

    [Parameter]
    public IEnumerable<int> BookedSeats { get; set; } = Array.Empty<int>();

    [Parameter]
    public EventCallback<int> OnReserveSeat { get; set; }

    private string GetSeatStyle(int seatNumber)
    {
        if (ReserveSeats.Contains(seatNumber))
        {
            return "selected";
        }

        if (BookedSeats.Contains(seatNumber))
        {
            return "reserved";
        }

        return "free";
    }

    private async Task ReserveSeat(MouseEventArgs _, int seatNumber)
    {
        if (!ReserveSeats.Contains(seatNumber) && !BookedSeats.Contains(seatNumber))
        {
            await OnReserveSeat.InvokeAsync(seatNumber);
        }
    }

}
