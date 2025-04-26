using Banned.Qbittorrent.Models.Application;
using Banned.Qbittorrent.Utils;

namespace Banned.Qbittorrent.Services;

public class ApplicationService(NetUtils netUtils)
{
    private const string BaseUrl = "/api/v2/app";

    public async Task<ApiVersion> GetApiVersionAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/webapiVersion");
        return new ApiVersion(result.Item2);
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
