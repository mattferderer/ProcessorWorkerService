using ProcessorWorkerService;
using ProcessorWorkerService.Configuration.Models;
using ProcessorWorkerService.Services.Consumers;
using ProcessorWorkerService.Services.DataStores;
using ProcessorWorkerService.Services.HttpClients;
using ProcessorWorkerService.Services.Producers;
using System.Threading.Channels;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<TwitterApiConfig>(context.Configuration.GetSection(nameof(TwitterApiConfig)));
        services.AddHostedService<Worker>();
        services.AddSingleton<ISocialMediaClient, TwitterClient>();
        var channel = Channel.CreateUnbounded<string>();
        services.AddTransient<ISocialMediaProducer, StreamProducer>(service => new StreamProducer(service.GetService<ISocialMediaClient>(), channel, service.GetService<ILogger>()));
        services.AddTransient<ISocialMediaConsumer, TwitterConsumer>(service => new TwitterConsumer(channel, service.GetService<ILogger>()));
        services.AddTransient<ISocialMediaStore, SimpleMemoryStore>();
    })
    .Build();

host.Run();
