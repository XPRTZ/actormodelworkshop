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
            siloBuilder.UseLocalhostClustering();
            siloBuilder.AddMemoryGrainStorage("ticket-system");
            siloBuilder.AddStartupTask<SeedLiveEventsTask>();

            // add Orleans dashboard on http://localhost:8080
            siloBuilder.UseDashboard(options => { });
        });

        builder.Services.AddTransient<IVenueService, VenueService>();

        return builder;
    }


}
