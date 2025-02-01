using AnotherQbittorrent.Models.Enums;
using AnotherQbittorrent.Models.Requests;
using AnotherQbittorrent.Models.Torrent;
using AnotherQbittorrent.Utils;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace AnotherQbittorrent.Services;

public class TorrentService(NetUtils netUtils)
{
    private const string BaseUrl = "/api/v2/torrents";

    public List<TorrentInfo>? GetTorrentInfos(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                              string?           category = null,
                                              string?           tag      = null,
                                              string?           sort     = null,
                                              bool              reverse  = false,
                                              int               limit    = 0,
                                              int               offset   = 0,
                                              List<string>?     hashList = null)
    {
        var requestPara = new TorrentListRequests
        {
            Filter   = filter,
            Category = category,
            Tag      = tag,
            Sort     = sort,
            Reverse  = reverse,
            Limit    = limit,
            Offset   = offset,
            HashList = hashList
        }.ToString();

        if (requestPara != string.Empty)
        {
            requestPara = "?" + requestPara;
        }

        var response = netUtils.Get($"{BaseUrl}/info{requestPara}");
        return StringToTorrentInfoList(response.Item2);
    }

    public async Task<List<TorrentInfo>?> AsyncGetTorrentInfos(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                                               string?           category = null,
                                                               string?           tag      = null,
                                                               string?           sort     = null,
                                                               bool              reverse  = false,
                                                               int               limit    = 0,
                                                               int               offset   = 0,
                                                               List<string>?     hashList = null)
    {
        var requestPara = new TorrentListRequests
        {
            Filter   = filter,
            Category = category,
            Tag      = tag,
            Sort     = sort,
            Reverse  = reverse,
            Limit    = limit,
            Offset   = offset,
            HashList = hashList
        }.ToString();

        if (requestPara != string.Empty)
        {
            requestPara = "?" + requestPara;
        }

        var response = await netUtils.GetAsync($"{BaseUrl}/info{requestPara}");
        return StringToTorrentInfoList(response.Item2);
    }

    public static List<TorrentInfo>? StringToTorrentInfoList(string jsonString)
    {
        try
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TorrentInfoConverter());

            var torrentInfos = JsonSerializer.Deserialize<List<TorrentInfo>>(jsonString, options);
            return torrentInfos;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON 解析失败: {ex.Message}");
            return null;
        }
    }

    public void DeleteTorrent(string hash, bool deleteFile = false)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
            { "deleteFiles", deleteFile.ToString().ToLower() }
        };

        netUtils.Post($"{BaseUrl}/delete", parameters);
    }

    public void DeleteTorrent(List<string> hashList, bool deleteFile = false)
    {
        var hash = string.Join('|', hashList.ToArray());
        DeleteTorrent(hash, deleteFile);
    }

    public async Task AsyncDeleteTorrent(string hash, bool deleteFile = false)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
            { "deleteFiles", deleteFile.ToString().ToLower() }
        };

        await netUtils.PostAsync($"{BaseUrl}/delete", parameters);
    }

    public async Task AsyncDeleteTorrent(List<string> hashList, bool deleteFile = false)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncDeleteTorrent(hash, deleteFile);
    }

    public void ResumeTorrent(string hash)
    {
        netUtils.Get($"{BaseUrl}/resume?hashes={hash}");
    }

    public void ResumeTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ResumeTorrent(hash);
    }

    public async Task AsyncResumeTorrent(string hash)
    {
        await netUtils.GetAsync($"{BaseUrl}/resume?hashes={hash}");
    }

    public async Task AsyncResumeTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncResumeTorrent(hash);
    }

    public void ReannounceTorrent(string hash)
    {
        netUtils.Get($"{BaseUrl}/reannounce?hashes={hash}");
    }

    public void ReannounceTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ReannounceTorrent(hash);
    }

    public async Task AsyncReannounceTorrent(string hash)
    {
        await netUtils.GetAsync($"{BaseUrl}/reannounce?hashes={hash}");
    }

    public async Task AsyncReannounceTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncReannounceTorrent(hash);
    }

    public void RecheckTorrent(string hash)
    {
        netUtils.Get($"{BaseUrl}/recheck?hashes={hash}");
    }

    public void RecheckTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        RecheckTorrent(hash);
    }

    public async Task AsyncRecheckTorrent(string hash)
    {
        await netUtils.GetAsync($"{BaseUrl}/recheck?hashes={hash}");
    }

    public async Task AsyncRecheckTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncRecheckTorrent(hash);
    }

    public void PauseTorrent(string hash)
    {
        netUtils.Get($"{BaseUrl}/pause?hashes={hash}");
    }

    public void PauseTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        PauseTorrent(hash);
    }

    public async Task AsyncPauseTorrent(string hash)
    {
        await netUtils.GetAsync($"{BaseUrl}/pause?hashes={hash}");
    }

    public async Task AsyncPauseTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncPauseTorrent(hash);
    }

    public List<TrackerInfo> GetTrackerList(string hash)
    {
        var response = netUtils.Get($"{BaseUrl}/trackers?hash={hash}");
        if (response.Item1 == HttpStatusCode.NotFound)
        {
            return new();
        }

        return new();
    }

    /// <summary>
    /// 添加种子文件或 URL
    /// </summary>
    public async Task<string> AddTorrentAsync(
        List<string>? filePaths          = null,
        List<string>? urls               = null,
        string?       savePath           = "/download",
        string?       category           = null,
        string?       tags               = null,
        bool?         skipChecking       = null,
        bool?         paused             = null,
        bool?         rootFolder         = null,
        string?       rename             = null,
        int?          upLimit            = null,
        int?          dlLimit            = null,
        float?        ratioLimit         = null,
        int?          seedingTimeLimit   = null,
        bool?         autoTmm            = null,
        bool?         sequentialDownload = null,
        bool?         firstLastPiecePrio = null)
    {
        var parameters = new Dictionary<string, string>();

        if (urls is { Count: > 0 })
        {
            parameters["urls"] = string.Join("\n", urls);
        }

        if (!string.IsNullOrEmpty(savePath)) parameters["savepath"] = savePath;
        if (!string.IsNullOrEmpty(category)) parameters["category"] = category;
        if (!string.IsNullOrEmpty(tags)) parameters["tags"] = tags;
        if (skipChecking.HasValue) parameters["skip_checking"] = skipChecking.Value.ToString().ToLower();
        if (paused.HasValue) parameters["paused"] = paused.Value.ToString().ToLower();
        if (rootFolder.HasValue) parameters["root_folder"] = rootFolder.Value.ToString().ToLower();
        if (!string.IsNullOrEmpty(rename)) parameters["rename"] = rename;
        if (upLimit.HasValue) parameters["upLimit"] = upLimit.Value.ToString();
        if (dlLimit.HasValue) parameters["dlLimit"] = dlLimit.Value.ToString();
        if (ratioLimit.HasValue) parameters["ratioLimit"] = ratioLimit.Value.ToString(CultureInfo.InvariantCulture);
        if (seedingTimeLimit.HasValue) parameters["seedingTimeLimit"] = seedingTimeLimit.Value.ToString();
        if (autoTmm.HasValue) parameters["autoTMM"] = autoTmm.Value.ToString().ToLower();
        if (sequentialDownload.HasValue)
            parameters["sequentialDownload"] = sequentialDownload.Value.ToString().ToLower();
        if (firstLastPiecePrio.HasValue)
            parameters["firstLastPiecePrio"] = firstLastPiecePrio.Value.ToString().ToLower();

        if (filePaths is { Count: > 0 })
        {
            var result = await netUtils.PostWithFilesAsync($"{BaseUrl}/add", parameters, filePaths);
            return result.Item2;
        }
        else if (urls is { Count: > 0 })
        {
            var result = await netUtils.PostAsync($"{BaseUrl}/add", parameters);
            return result.Item2;
        }
        else
        {
            return "No torrent file or URL provided.";
        }
    }
}
