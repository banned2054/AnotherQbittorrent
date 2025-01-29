using AnotherQbittorrent.Utils;

namespace AnotherQbittorrent.Services;

public class ApplicationService(NetUtils netUtils)
{
    private const string BaseUrl = "/api/v2/app";

    public string GetApiVersion()
    {
        var result = netUtils.Fetch($"{BaseUrl}/webapiVersion");
        return result;
    }

    public string GetVersion()
    {
        var result = netUtils.Fetch($"{BaseUrl}/version");
        return result;
    }

    public string GetBuildInfo()
    {
        var result = netUtils.Fetch($"{BaseUrl}/buildInfo");
        return result;
    }

    public async Task<string> AsyncGetApiVersion()
    {
        var result = await netUtils.AsyncFetch($"{BaseUrl}/webapiVersion");
        return result;
    }

    public async Task<string> AsyncGetVersion()
    {
        var result = await netUtils.AsyncFetch($"{BaseUrl}/version");
        return result;
    }

    public async Task<string> AsyncGetBuildInfo()
    {
        var result = await netUtils.AsyncFetch($"{BaseUrl}/buildInfo");
        return result;
    }
}