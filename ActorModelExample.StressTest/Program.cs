using ActorModelExample.StressTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static int _numberOfConcurrentConnections = 10;

    public static async Task Main(String[] args)
    {
        SetNumberOfConcurrentConnectionsFromArgs(args);

        var builder = CreateHostBuilder(args);          
        
        var host = builder.Build();
        await host.StartAsync();

        Console.WriteLine("Enter to Quit...");
        Console.ReadLine();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)        
        .ConfigureServices((hostContext, services) =>
        { 
            services.AddHostedService(sp => new StresserService(_numberOfConcurrentConnections));
        });

    private static void SetNumberOfConcurrentConnectionsFromArgs(string[] args)
    {
        if(args.Length != 1)
        {
            PrintDefaultArgumentMessage();
        }
        else
        {
            if (int.TryParse(args[0], out var nrOfConn))
            {
                _numberOfConcurrentConnections = nrOfConn;
                Console.WriteLine($"StressTest will use {nrOfConn} concurrent connections.");
            }
            else
            {
                PrintDefaultArgumentMessage();
            }
        }
    }

    private static void PrintDefaultArgumentMessage()
    {
        Console.WriteLine("StressTest will use (default) 10 concurrent connections.");
        Console.WriteLine("To change use StressTest.exe [numberOfCouncurrentConnections]");
    }
}