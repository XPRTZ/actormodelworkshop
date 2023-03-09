namespace ActorModelExample.Domain.Models;

public record Venue(string Name, string Location, IEnumerable<LiveEvent> LiveEvents);
