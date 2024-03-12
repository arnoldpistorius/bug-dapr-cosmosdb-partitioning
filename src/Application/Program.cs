using Application;
using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddLogging(x => x.AddConsole());
// services.AddDaprSidekick(configure => {
//     configure.Sidecar = new() {
//         AppId = "demo",
//         ResourcesDirectory = "dapr-components",
//         DaprHttpPort = 3500,
//         DaprGrpcPort = 50001
//     };
// });

services.AddActors(configure => {
    configure.RemindersStoragePartitions = 3;

    configure.Actors.RegisterActor<MyActorWithReminder>(nameof(MyActorWithReminder));
});
services.AddDaprClient();
services.AddTransient<ActorProxyFactory>();

var app = builder.Build();

app.MapGet("/{id}/start", async (string id, [FromServices] ActorProxyFactory actorFactory) => {
    var actor = actorFactory.CreateActorProxy<IMyActorWithReminder>(new ActorId(id), nameof(MyActorWithReminder));
    await actor.InitializeActor();
    return "Actor initialized";
});

app.MapGet("/{id}/stop", async (string id, [FromServices] ActorProxyFactory actorFactory) => {
    var actor = actorFactory.CreateActorProxy<IMyActorWithReminder>(new ActorId(id), nameof(MyActorWithReminder));
    await actor.StopActor();
    return "Actor stopped";
});

app.MapActorsHandlers();

await app.RunAsync();