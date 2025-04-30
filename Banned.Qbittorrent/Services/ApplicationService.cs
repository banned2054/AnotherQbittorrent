using Banned.Qbittorrent.Models.Application;
using Banned.Qbittorrent.Utils;

namespace Banned.Qbittorrent.Services;

/// <summary>
/// 提供与 qBittorrent 应用程序相关的服务<br/>
/// Provides services related to qBittorrent application
/// </summary>
public class ApplicationService(NetUtils netUtils)
{
    private const string BaseUrl = "/api/v2/app";

    /// <summary>
    /// 获取 Web API 版本<br/>
    /// Get Web API version
    /// </summary>
    /// <returns>Web API 版本信息<br/>Web API version information</returns>
    public async Task<ApiVersion> GetApiVersionAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/webapiVersion");
        return new ApiVersion(result.Item2);
    }

    /// <summary>
    /// 获取 qBittorrent 版本<br/>
    /// Get qBittorrent version
    /// </summary>
    /// <returns>qBittorrent 版本号<br/>qBittorrent version number</returns>
    public async Task<string> GetVersionAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/version");
        return result.Item2;
    }

    /// <summary>
    /// 获取构建信息<br/>
    /// Get build information
    /// </summary>
    /// <returns>构建信息<br/>Build information</returns>
    public async Task<string> GetBuildInfoAsync()
    {
        var result = await netUtils.GetAsync($"{BaseUrl}/buildInfo");
        return result.Item2;
    }
}
