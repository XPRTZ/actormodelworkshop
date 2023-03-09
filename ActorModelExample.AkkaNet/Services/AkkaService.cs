using ActorModelExample.AkkaNet.Actors;
using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ActorModelExample.AkkaNet.Services
{
    internal class AkkaService : IHostedService, IActorBridge
    {
        private readonly ILogger<AkkaService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private ActorSystem? _actorSystem;
        private IActorRef? _actorRef;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public AkkaService(ILogger<AkkaService> logger, IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _applicationLifetime = appLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var bootstrap = BootstrapSetup.Create();
            var diSetup = DependencyResolverSetup.Create(_serviceProvider);

            var actorSystemSetup = bootstrap.And(diSetup);
            _actorSystem = ActorSystem.Create("akka-universe", actorSystemSetup);

            var resolver = DependencyResolver.For(_actorSystem);
            var props = resolver.Props<VenueActor>();

            _actorRef = _actorSystem.ActorOf(props, "venue");
            _ = _actorSystem.WhenTerminated.ContinueWith(tr =>
            {
                _applicationLifetime.StopApplication();
            }, cancellationToken);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        public async Task<object> Ask(object message)
        {
            if (_actorRef == null)
            {
                _logger.LogWarning("No VenueActorRef found");
                throw new ApplicationException("Initialization of Akka.Net failed, no VenueActorRef found");
            }

            return await _actorRef.Ask(message);
        }
    }
}
