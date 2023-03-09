using ActorModelExample.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActorModelExample.Orleans.Server;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddOrleans(this WebApplicationBuilder builder)
    {
        builder.Host.UseOrleans(siloBuilder =>
        {
            siloBuilder.UseDashboard(options => { });
            siloBuilder.UseLocalhostClustering();
            siloBuilder.AddMemoryGrainStorage("ticket-system");
            siloBuilder.UseInMemoryReminderService();
            siloBuilder.AddStartupTask<SeedLiveEventsTask>();
        });

        builder.Services.AddTransient<IVenueService, VenueService>();

        return builder;
    }


}
