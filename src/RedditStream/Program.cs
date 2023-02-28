// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditStream.Services.Clients;
using RedditStream.Services.Configuration;
using RedditStream.Services;
using System.Net.Http.Headers;
using System.Text;
using Serilog;
using RedditStream.Services.Interfaces;
using RedditStream.Repositories;
using RedditStream.Repositories.Interfaces;
using RedditStream.Repositories.Data;
using Microsoft.EntityFrameworkCore;

var builder = new ConfigurationBuilder();

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Build())
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("log.txt", shared: true, rollingInterval: RollingInterval.Day)
           .CreateLogger();


Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        services.AddOptions<RedditOptions>().Bind(configuration.GetSection("Reddit"));

        services.AddDbContext<RedditStreamContext>( options =>
        { 
            options.UseSqlite(configuration.GetConnectionString("RedditStream"));
        });
     
        services.AddScoped<IQueueService, QueueService>();
        services.AddScoped<IQueueClientService, QueueClientService>();
        services.AddScoped<ILiveThreadMessageService, LiveThreadMessageService>();
        services.AddScoped<ILiveThreadMessageRepository, LiveThreadMessageRepository>();
      
        services.AddTransient<RedditAuthenticationHandler>();
        services.AddHttpClient<IRedditClientAuthentication, RedditClientAuthentication>(x =>
        {
            var baseUrl = configuration.GetValue<string>("Reddit:Api:Authentication:BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException(nameof(RedditOptions.Api.Authentication.BaseUrl));
            }
            x.BaseAddress = new Uri(baseUrl);
        });
        services.AddHttpClient<IRedditClient, RedditClient>(x =>
        {
            var baseUrl = configuration.GetValue<string>("Reddit:Api:BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException(nameof(RedditOptions.Api.BaseUrl));
            }
            x.BaseAddress = new Uri(baseUrl);
        })
        .AddHttpMessageHandler<RedditAuthenticationHandler>();

    })
    .UseSerilog()
    .ConfigureServices(services => services.AddMemoryCache())
    .Build();

var svc = ActivatorUtilities.CreateInstance<RedditService>(host.Services);
await svc.Run();

Console.ReadLine();