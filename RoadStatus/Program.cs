using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadStatus.ApiService;

var roadApiService = SetupRoadApiService();

if(args.Length == 0)
{
    Console.WriteLine("Please provide a road code");
    Environment.Exit(1);
}

var code = args[0];

var roadData = await roadApiService.GetRoadStatusByCode(code);

Console.WriteLine(roadData.StatusMessage);

Environment.Exit(roadData.IsSuccessReponse == false ? 1 : 0);

IRoadApiService SetupRoadApiService()
{
    var builder = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", true, true);

    var config = builder.Build();

    var apiKey = config["API:Key"];
    var appId = config["API:AppId"];

    var serviceCollection = new ServiceCollection();
    serviceCollection.AddTransient<IRoadApiService>(_ => new RoadApiService(apiKey, appId));
    var serviceProvider = serviceCollection.BuildServiceProvider();

    return serviceProvider.GetRequiredService<IRoadApiService>();
}