using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.DummyImpl.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ActorModelExample.DummyImpl.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDummy(this IServiceCollection services, Venue venue)
    {
        var service = new VenueService();
        service.RegisterVenueAsync(venue).GetAwaiter().GetResult(); // eww
        services.AddSingleton<IVenueService>(service);

        return services;
    }
}
