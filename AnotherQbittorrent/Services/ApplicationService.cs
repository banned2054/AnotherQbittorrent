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

    public async Task<string> AsyncGetApiVersion()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/webapiVersion");
        return result.Item2;
    }

    public async Task<string> AsyncGetVersion()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/version");
        return result.Item2;
    }

    public async Task<string> AsyncGetBuildInfo()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/buildInfo");
        return result.Item2;
    }
}
