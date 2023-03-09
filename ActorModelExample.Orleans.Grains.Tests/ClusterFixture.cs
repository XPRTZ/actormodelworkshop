using Orleans.TestingHost;

namespace ActorModelExample.Orleans.Grains.Tests;

public class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; init; }

    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<SiloConfigurator>();
        Cluster = builder.Build();
        Cluster.Deploy();
    }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }
}

public class SiloConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder hostBuilder)
    {
        hostBuilder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("ticket-system");
    }
}