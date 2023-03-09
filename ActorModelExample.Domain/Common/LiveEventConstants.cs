using System.Globalization;

namespace ActorModelExample.Domain.Common
{
    public static class LiveEventConstants
    {
        public const int EventRows = 10;
        public const int SeatsPerRow = 10;

        public static CultureInfo CultureInfo => new("nl-NL");

        public const string VenueLocation = "Amsterdam";
        public const string VenueName = "Carre";
    }
}
