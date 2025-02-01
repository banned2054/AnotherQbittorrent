using AnotherQbittorrent.Models.Enums;
using AnotherQbittorrent.Models.Requests;
using AnotherQbittorrent.Models.Torrent;
using AnotherQbittorrent.Utils;
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
        var requestPara = new GetTorrentInfoListRequest
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

    public List<TorrentInfo>? GetTorrentInfos(GetTorrentInfoListRequest request)
    {
        var requestPara = request.ToString();

        if (requestPara != string.Empty)
        {
            requestPara = "?" + requestPara;
        }

        var response = netUtils.Get($"{BaseUrl}/info{requestPara}");
        return StringToTorrentInfoList(response.Item2);
    }


    public async Task<List<TorrentInfo>?> GetTorrentInfosAsync(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                                               string?           category = null,
                                                               string?           tag      = null,
                                                               string?           sort     = null,
                                                               bool              reverse  = false,
                                                               int               limit    = 0,
                                                               int               offset   = 0,
                                                               List<string>?     hashList = null)
    {
        var requestPara = new GetTorrentInfoListRequest
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

    public async Task<List<TorrentInfo>?> GetTorrentInfosAsync(GetTorrentInfoListRequest request)
    {
        var requestPara = request.ToString();

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

    public async Task DeleteTorrentAsync(string hash, bool deleteFile = false)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
            { "deleteFiles", deleteFile.ToString().ToLower() }
        };

        await netUtils.PostAsync($"{BaseUrl}/delete", parameters);
    }

    public async Task DeleteTorrentAsync(List<string> hashList, bool deleteFile = false)
    {
        var hash = string.Join('|', hashList.ToArray());
        await DeleteTorrentAsync(hash, deleteFile);
    }

    public void ResumeTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        netUtils.Post($"{BaseUrl}/resume", parameters);
    }

    public void ResumeTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ResumeTorrent(hash);
    }

    public async Task ResumeTorrentAsync(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        await netUtils.PostAsync($"{BaseUrl}/resume", parameters);
    }

    public async Task ResumeTorrentAsync(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await ResumeTorrentAsync(hash);
    }

    public void ReannounceTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        netUtils.Post($"{BaseUrl}/reannounce", parameters);
    }

    public void ReannounceTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ReannounceTorrent(hash);
    }

    public async Task ReannounceTorrentAsync(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        await netUtils.PostAsync($"{BaseUrl}/reannounce", parameters);
    }

    public async Task ReannounceTorrentAsync(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await ReannounceTorrentAsync(hash);
    }

    public void RecheckTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        netUtils.Post($"{BaseUrl}/recheck", parameters);
    }

    public void RecheckTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        RecheckTorrent(hash);
    }

    public async Task RecheckTorrentAsync(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };
        await netUtils.PostAsync($"{BaseUrl}/recheck", parameters);
    }

    public async Task RecheckTorrentAsync(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await RecheckTorrentAsync(hash);
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

    public async Task PauseTorrentAsync(string hash)
    {
        await netUtils.GetAsync($"{BaseUrl}/pause?hashes={hash}");
    }

    public async Task PauseTorrentAsync(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await PauseTorrentAsync(hash);
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
    public async Task<string> AddTorrentAsync(AddTorrentRequest request)
    {
        var parameters = request.ToDictionary();

        if (request.FilePaths is { Count: > 0 })
        {
            var result = await netUtils.PostWithFilesAsync($"{BaseUrl}/add", parameters, request.FilePaths);
            return result.Item2;
        }
        else if (request.Urls is { Count: > 0 })
        {
            var result = await netUtils.PostAsync($"{BaseUrl}/add", parameters);
            return result.Item2;
        }
        else
        {
            return "No torrent file or URL provided.";
        }
    }

    // 兼容旧版本，直接传入参数
    public async Task<string> AddTorrentAsync(
        List<string>? filePaths              = null,
        List<string>? urls                   = null,
        string?       savePath               = "/download",
        string?       category               = null,
        string?       tags                   = null,
        bool?         skipChecking           = null,
        bool?         paused                 = null,
        bool?         rootFolder             = null,
        string?       rename                 = null,
        int?          uploadLimit            = null,
        int?          downloadLimit          = null,
        float?        ratioLimit             = null,
        int?          seedingTimeLimit       = null,
        bool?         autoTmm                = null,
        bool?         sequentialDownload     = null,
        bool?         firstLastPiecePriority = null)
    {
        var request = new AddTorrentRequest
        {
            FilePaths              = filePaths,
            Urls                   = urls,
            SavePath               = savePath,
            Category               = category,
            Tags                   = tags,
            SkipChecking           = skipChecking,
            Paused                 = paused,
            RootFolder             = rootFolder,
            Rename                 = rename,
            UploadLimit            = uploadLimit,
            DownloadLimit          = downloadLimit,
            RatioLimit             = ratioLimit,
            SeedingTimeLimit       = seedingTimeLimit,
            AutoTmm                = autoTmm,
            SequentialDownload     = sequentialDownload,
            FirstLastPiecePriority = firstLastPiecePriority
        };

        return await AddTorrentAsync(request);
    }
}
