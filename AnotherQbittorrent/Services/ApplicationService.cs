using AnotherQbittorrent.Utils;

namespace AnotherQbittorrent.Services;

public class ApplicationService(NetUtils netUtils)
{
    private const string BaseUrl = "/api/v2/app";

    public string GetApiVersion()
    {
        var result = netUtils.Get($"{BaseUrl}/webapiVersion");
        return result.Item2;
    }

    public string GetVersion()
    {
        var result = netUtils.Get($"{BaseUrl}/version");
        return result.Item2;
    }

    public string GetBuildInfo()
    {
        var result = netUtils.Get($"{BaseUrl}/buildInfo");
        return result.Item2;
    }

    public async Task<string> GetApiVersionAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/webapiVersion");
        return result.Item2;
    }

    public async Task<string> GetVersionAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/version");
        return result.Item2;
    }

    public async Task<string> GetBuildInfoAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/buildInfo");
        return result.Item2;
    }
}
