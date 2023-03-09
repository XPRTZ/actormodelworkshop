using ActorModelExample.Domain.Models;
using ActorModelExample.WebApp.Common;
using Bogus;

namespace ActorModelExample.WebApp.Util;

public static class DataGenerator
{
    public static Venue GenerateVenue()
    {
        var faker = new Faker("nl");

        var liveEventList = new List<LiveEvent>();
        for (int index = 0; index < 5; index++)
        {
            var liveEvent = new LiveEvent(
                Guid.NewGuid(),
                faker.Date.Between(DateTime.Now, DateTime.Now.AddYears(1)),
                faker.Name.FullName(),
                faker.Image.PicsumUrl(),
                faker.Random.Int(25, 150)
                );

            liveEventList.Add(liveEvent);
        }

        return new Venue(
            LiveEventConstants.VenueName,
            LiveEventConstants.VenueLocation,
            liveEventList);
    }
}