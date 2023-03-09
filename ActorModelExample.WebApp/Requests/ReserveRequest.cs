using ActorModelExample.Domain.Models;

namespace ActorModelExample.WebApp.Requests
{
    public class ReserveRequest
    {
        public LiveEvent LiveEvent { get; set; }
        public int Seat { get; set; }
        public Guid BookingId { get; set; }
    }
}
