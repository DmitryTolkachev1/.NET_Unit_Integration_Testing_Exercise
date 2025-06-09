using System.Text;
using System.Text.Json;
using DotNet.Testcontainers.Images;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;
using WebAPI;

namespace TestProject1;
public class UsersControllerTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private HttpClient _client;

    public UsersControllerTests()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("test-db")
            .WithImage("postgres:15")
            .WithDockerEndpoint("unix:///run/podman/podman.sock")
            .WithCleanUp(true)
            .WithName("integration-tests-postgres")
            .Build();
    } 

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var settings = new Dictionary<string, string>
                    {
                        ["ConnectionStrings:DefaultConnection"] = _postgresContainer.GetConnectionString()
                    };

                    config.AddInMemoryCollection(settings);
                });
            });

        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    [Fact]
    public async Task CreateUser_ThenGetUser_ShouldReturnCorrectData()
    {
        // Arrange
        var user = new { name = "Test", email = "test@mail.com" };
        var json = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

        // Act
        var createResponse = await _client.PostAsync("/api/users", json);
        createResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/api/users/1");
        var content = await getResponse.Content.ReadAsStringAsync();

        // Assert
        content.Should().Contain("Test");
        content.Should().Contain("test@mail.com");
    }
}