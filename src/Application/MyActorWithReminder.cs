using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace Application;

public class MyActorWithReminder : Actor, IMyActorWithReminder
{
    private readonly ILogger<MyActorWithReminder> logger;

    public MyActorWithReminder(ActorHost host, ILogger<MyActorWithReminder> logger) : base(host)
    {
        this.logger = logger;
    }

    public async Task InitializeActor()
    {
        using var scope = CreateLoggingScope();

        logger.LogDebug("Registering reminder...");
        await RegisterReminderAsync("some-reminder-name", null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
        logger.LogDebug("Reminder registered.");
    }

    public async Task StopActor()
    {
        using var scope = CreateLoggingScope();

        logger.LogDebug("Unregistering reminder...");
        await UnregisterReminderAsync("some-reminder-name");
        logger.LogDebug("Reminder unregistered.");
    }

    IDisposable? CreateLoggingScope() {
        return logger.BeginScope("ActorId: {actorId}", Id.GetId());
    }
}

public interface IMyActorWithReminder : IActor
{
    Task InitializeActor();

    Task StopActor();
}
