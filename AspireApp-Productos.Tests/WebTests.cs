using Microsoft.Extensions.Logging;

namespace AspireApp_Productos.Tests;

public class WebTests
{
    private static readonly TimeSpan BuildTimeout = GetTimeout("TEST_BUILD_TIMEOUT_SECONDS", TimeSpan.FromMinutes(5));
    private static readonly TimeSpan RequestTimeout = GetTimeout("TEST_REQUEST_TIMEOUT_SECONDS", TimeSpan.FromSeconds(60));

    private static TimeSpan GetTimeout(string envVar, TimeSpan @default)
    {
        var s = Environment.GetEnvironmentVariable(envVar);
        if (int.TryParse(s, out var secs) && secs > 0) return TimeSpan.FromSeconds(secs);
        return @default;
    }

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource(BuildTimeout).Token;
        var requestCancellationToken = new CancellationTokenSource(RequestTimeout).Token;

        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireApp_Productos_AppHost>(cancellationToken);
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        await using var app = await appHost.BuildAsync(cancellationToken).WaitAsync(BuildTimeout, cancellationToken);
        await app.StartAsync(cancellationToken).WaitAsync(BuildTimeout, cancellationToken);

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        await app.ResourceNotifications.WaitForResourceHealthyAsync("webfrontend", cancellationToken).WaitAsync(BuildTimeout, cancellationToken);
        var response = await httpClient.GetAsync("/", requestCancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
