// Code Owner: Leon Paintner
namespace Trainee_Tracker.E2ETests;

public class ConnectivityTest
{
    private readonly HttpClient _client = new ();
    private const string BaseUrl = "http://localhost:5089";

    [Fact]
    public async void ServerReturnsSuccessStatus()
    {
        var response = await _client.GetAsync(BaseUrl);

        response.EnsureSuccessStatusCode();
    }
}