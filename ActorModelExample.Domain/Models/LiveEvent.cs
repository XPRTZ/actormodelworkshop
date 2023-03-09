namespace ActorModelExample.Domain.Models;

public record LiveEvent(Guid Id, DateTime Date, string Artist, string Picture, int Price);
