using ActorModelExample.Domain.Models;

namespace ActorModelExample.WebApp.Requests
{
    public class ConfirmRequest
    {
        public LiveEvent LiveEvent { get; set; }
        public IEnumerable<int> Seats { get; set; }
        public Guid BookingId { get; set; }
    }
}
