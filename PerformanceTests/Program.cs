using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NBomber.CSharp;
using NBomber.Http.CSharp;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting NBomber Performance Test...");
        PerformanceTest();
        Console.WriteLine("Test completed.");
    }

    private static void PerformanceTest()
    {
        var client = new HttpClient();

        var scenario = Scenario.Create("users_get_scenario", async context =>
        {
            var request = Http.CreateRequest("GET", "https://localhost:7168/api/users");
            var response = await Http.Send(client, request);
            return response;
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: 10,
                              interval: TimeSpan.FromSeconds(1),
                              during: TimeSpan.FromSeconds(30))
        );

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}