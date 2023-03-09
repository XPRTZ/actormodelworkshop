using ActorModelExample.AkkaNet.Services;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActorModelExample.AkkaNet.Hosting;

public static class HostingExtensions
{
    public static IServiceCollection AddAkkaNetService(this IServiceCollection services)
    {
        services.AddSingleton<IActorBridge, AkkaService>();
        services.AddHostedService(sp => (AkkaService)sp.GetRequiredService<IActorBridge>());
        services.AddScoped<IVenueService, AkkaVenueService>();
        return services;
    }

    public static IHost LoadVenue(this IHost app, Venue venue)
    {
        using var scope = app.Services.CreateScope();

        var venueService = scope.ServiceProvider.GetRequiredService<IVenueService>();
        venueService.RegisterVenueAsync(venue).GetAwaiter().GetResult();

        return app;
    }
}
