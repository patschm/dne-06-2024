using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

// Used Nuget packages:
// Microsoft.Extensions.Configuration
// Microsoft.Extensions.Configuration.Ini
// Microsoft.Extensions.Configuration.Json
// Microsoft.Extensions.Configuration.Xml
// Microsoft.Extensions.DependencyInjection
// Microsoft.Extensions.Hosting
// Microsoft.Extensions.Logging
// Microsoft.Extensions.Logging.Console

namespace InfraStructure;

internal class Program
{
    static void Main(string[] args)
    {
        //EnvironmentSettings();
        //Configuration();
        //Secrets();
        //Logging();
        //DependencyInjection();
        AllInOne();
    }

    private static void EnvironmentSettings()
    {
        // Predefined settings
        Console.WriteLine($"Base Directory: {Environment.CurrentDirectory}");
        Console.WriteLine($"User login name: {Environment.UserName}");
        Console.WriteLine($"Machine name: {Environment.MachineName}");
        Console.WriteLine($"Installed OS: {Environment.OSVersion}");
        Console.WriteLine($".NET version: {Environment.Version}");

        // By convention:
        // DOTNET variables always starts with DOTNET_<NAME>
        // ASPNET variables always starts with ASPNETCORE_<NAME>

        // Set them in Properties/launchSettings.json
        Console.WriteLine(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"));
        // Or command line:
        // set DOTNET_NAME=Jan
        // dotnet run
        // Optionally add: --no-launch-profile or --launch-profile EnvironmentSample"
        Console.WriteLine(Environment.GetEnvironmentVariable("DOTNET_NAME"));
        
    }
    private static void Configuration()
    {
        // (I)ConfigurationBuilder from package Microsoft.Extensions.Configuration;
        IConfigurationBuilder bld = new ConfigurationBuilder();
       
        // AddJsonFile from package Microsoft.Extensions.Configuration.Json;
        bld.SetBasePath(Environment.CurrentDirectory);
        bld.AddJsonFile("appsettings.json", optional:true, reloadOnChange:true);
        bld.AddJsonFile($"appsettings.Staging.json", optional: true, reloadOnChange: false);
        // bld.AddXmlFile("config.xml");
        // bld.AddIniFile("startup.ini");
        IConfiguration config = bld.Build();
        config.GetReloadToken().RegisterChangeCallback(obj => Console.WriteLine("Gewijzigd"), null);
        var section = config.GetSection("MyConfiguration");
        var conf = new MyConfiguration();
        
        do
        {
            //var conf = section.Get<MyConfiguration>();
            section.Bind(conf);
            Console.WriteLine(conf.Name);
            Thread.Sleep(5000);
        }
        while (true);

        // Read single entry
        //var nameSection = config.GetSection("MyConfiguration:Name");
        //Console.WriteLine(nameSection.Value);
        //// Read collection. Use extensions from package Microsoft.Extensions.Configuration.Binder
        //var hobbiesSection = config.GetSection("MyConfiguration:Hobbies");
        //var data1 = hobbiesSection.Get<string[]>();
        //Console.WriteLine(string.Join(',', data1!));
        //// Read complex object
        //var addressSection = config.GetSection("MyConfiguration:Address");
        //System.Console.WriteLine("Test Value: " + addressSection.Value);
        //var address = addressSection.Get<Address>();
        //Console.WriteLine($"{address?.StreetName} {address?.Number}");
        //// Alternatively:
        //var address2 = new Address();
        //addressSection.Bind(address2);
        //Console.WriteLine($"{address2?.StreetName} {address2?.Number}");

    }
    private static void Secrets()
    {
        IConfigurationBuilder bld = new ConfigurationBuilder();
        bld.SetBasePath(Environment.CurrentDirectory);
        bld.AddJsonFile("appsettings.json", optional:true, reloadOnChange:false);
        bld.AddUserSecrets<Program>();
        IConfiguration config = bld.Build();

        // Set and read a secret at command line:
        // If not done: dotnet user-secrets init
        // dotnet user-secrets set "Big:Secret" "Password" (optionally add for which project: --project "D:\Net Essentials\Demos\Module 2\InfraStructure)"
        var secret = config.GetSection("Big:Secret").Value;
        // or
        secret = config["Big:Secret"];
        Console.WriteLine($"The big secret is: {secret}");

        // more commands:
        // dotnet user-secrets list
        // dotnet user-secrets remove "Big:Secret"
    }
    private static void Logging()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Environment.CurrentDirectory);
        builder.AddJsonFile("appsettings.json", optional:false, reloadOnChange:true);
        var config2 = builder.Build();

        var factory = LoggerFactory.Create(config => {
            // In code:
            //config.AddFilter((cat, lvl) =>
            //{
            //    return cat == typeof(LogVictim).FullName && lvl >= LogLevel.Debug;
            //});
            // In config:
            config.AddConfiguration(config2.GetSection("Logging"));

            config.ClearProviders();
            // From package: Microsoft.Extensions.Logging.Console
            config.AddConsole();
            //config.AddEventLog();
        });

        ILogger<LogVictim> logger = factory.CreateLogger<LogVictim>();
        var obj = new LogVictim(logger);
        obj.DoSomeStuff();
    }
    private static void DependencyInjection()
    {
        var factory = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
        var builder = factory.CreateBuilder(services);

        builder.AddHostedService<ConsoleHost>();
        builder.AddTransient<ICounter, Counter>();
        //builder.AddScoped<ICounter, Counter>();
        //builder.AddSingleton<ICounter, Counter>();

        var provider = builder.BuildServiceProvider();

        //provider.
        
       // var service = provider.GetRequiredService<ConsoleHost>();
        //service.StartAsync(CancellationToken.None).Wait();

        //var ctr=provider.GetRequiredService<ICounter>();
        //ctr.Increment();
        //ctr.Show();
        //ctr = provider.GetRequiredService<ICounter>();
        //ctr.Increment();
        //ctr.Show();
        //ctr = provider.GetRequiredService<ICounter>();
        //ctr.Increment();
        //ctr.Show();
        //return;

        //Console.WriteLine("==== Run 1 ====");
        //using (var scope = provider.CreateScope())
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        var _counter = scope.ServiceProvider.GetRequiredService<ICounter>();
        //        _counter.Increment();
        //        _counter.Show();
        //    }
        //}

        //Console.WriteLine("==== Run 2 ====");
        //using (var scope = provider.CreateScope())
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        var _counter = scope.ServiceProvider.GetRequiredService<ICounter>();
        //        _counter.Increment();
        //        _counter.Show();
        //    }
        //}
    }
    private static void AllInOne()
    {
        // Host puts all the goodies in one. 
        // Use Host for all non-web applications.
        // WebHost (net 5.0 or lower) is the one needed for aspnet webapi/mvc
        // WebApplication (net 6 or higher) is the one needed for aspnet webapi/mvc

        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(conf =>
            {
                //conf.AddJsonFile("appsettings.json"); // Not needed. Is Default
            })
            .ConfigureServices(scol =>
            {
                scol.AddHostedService<ConsoleHost>();
                scol.AddHostedService<ConsoleHost2>();  
                scol.AddScoped<ICounter, Counter>();
                scol.AddTransient<LogVictim>();
            })
            .ConfigureLogging(log =>
            {        
               //log.ClearProviders();
                //log.AddConsole(); // Not needed. Is default
            })
            .Build();

        //var logvictim = host.Services.GetRequiredService<LogVictim>();
        //logvictim.DoSomeStuff();

        host.Start();

       
    }
}