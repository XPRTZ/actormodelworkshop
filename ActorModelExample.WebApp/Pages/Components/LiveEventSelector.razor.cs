using ActorModelExample.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ActorModelExample.WebApp.Pages.Components;

public partial class LiveEventSelector
{
    [Parameter]
    public Venue Venue { get; set; } = null!;

    [Parameter]
    public EventCallback<Guid> OnLiveEventSelected { get; set; }

    private async Task SelectLiveEvent(MouseEventArgs _, Guid id)
    {
        await OnLiveEventSelected.InvokeAsync(id);
    }
}
