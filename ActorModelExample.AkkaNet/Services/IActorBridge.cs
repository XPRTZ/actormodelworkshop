namespace ActorModelExample.AkkaNet.Services;

public interface IActorBridge
{
    Task<object> Ask(object message);
}

