using AnotherQbittorrent.Models.Application;
using AnotherQbittorrent.Models.Enums;
using AnotherQbittorrent.Models.Requests;
using AnotherQbittorrent.Models.Torrent;
using AnotherQbittorrent.Utils;
using System.Net;
using System.Text.Json;

namespace AnotherQbittorrent.Services;

public class TorrentService(NetUtils netUtils, ApiVersion apiVersion)
{
    private const    string     BaseUrl      = "/api/v2/torrents";
    private readonly ApiVersion _apiVersion5 = new("2.11.0");

    public List<TorrentInfo> GetTorrentInfos(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                             string?           category = null,
                                             string?           tag      = null,
                                             string?           sort     = null,
                                             bool              reverse  = false,
                                             int               limit    = 0,
                                             int               offset   = 0,
                                             string            hash     = "")
    {
        var hashList = hash == "" ? null : new List<string> { hash };
        return GetTorrentInfos(filter, category, tag, sort, reverse, limit, offset, hashList);
    }

    public async Task<List<TorrentInfo>> GetTorrentInfosAsync(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                                              string?           category = null,
                                                              string?           tag      = null,
                                                              string?           sort     = null,
                                                              bool              reverse  = false,
                                                              int               limit    = 0,
                                                              int               offset   = 0,
                                                              string            hash     = "")
    {
        var hashList = hash == "" ? null : new List<string> { hash };
        return await GetTorrentInfosAsync(filter, category, tag, sort, reverse, limit, offset, hashList);
    }

    public List<TorrentInfo> GetTorrentInfos(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                             string?           category = null,
                                             string?           tag      = null,
                                             string?           sort     = null,
                                             bool              reverse  = false,
                                             int               limit    = 0,
                                             int               offset   = 0,
                                             List<string>?     hashList = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "filter", filter.ToString().ToLower() }
        };

        if (!string.IsNullOrEmpty(category)) parameters.Add("category", category);
        if (!string.IsNullOrEmpty(tag)) parameters.Add("tag", tag);
        if (!string.IsNullOrEmpty(sort)) parameters.Add("sort", sort);
        if (reverse) parameters.Add("reverse", "true");
        if (limit  > 0) parameters.Add("limit", limit.ToString());
        if (offset > 0) parameters.Add("offset", offset.ToString());
        if (hashList is { Count: > 0 })
        {
            parameters.Add("hashes", string.Join("|", hashList)); // 确保 `hashes` 以 `|` 连接
        }

        var response = netUtils.Post($"{BaseUrl}/info", parameters);
        return response.Item1 == HttpStatusCode.OK ? StringToTorrentInfoList(response.Item2) : new List<TorrentInfo>();
    }

    public List<TorrentInfo> GetTorrentInfos(GetTorrentInfoListRequest request)
    {
        var parameters = new Dictionary<string, string>
        {
            { "filter", request.Filter.ToString().ToLower() }
        };

        if (!string.IsNullOrEmpty(request.Category)) parameters.Add("category", request.Category);
        if (!string.IsNullOrEmpty(request.Tag)) parameters.Add("tag", request.Tag);
        if (!string.IsNullOrEmpty(request.Sort)) parameters.Add("sort", request.Sort);
        if (request.Reverse) parameters.Add("reverse", "true");
        if (request.Limit  > 0) parameters.Add("limit", request.Limit.ToString());
        if (request.Offset > 0) parameters.Add("offset", request.Offset.ToString());
        if (request.HashList is { Count: > 0 })
        {
            parameters.Add("hashes", string.Join("|", request.HashList));
        }

        var response = netUtils.Post($"{BaseUrl}/info", parameters);
        return response.Item1 == HttpStatusCode.OK ? StringToTorrentInfoList(response.Item2) : new List<TorrentInfo>();
    }

    public async Task<List<TorrentInfo>> GetTorrentInfosAsync(EnumTorrentFilter filter   = EnumTorrentFilter.All,
                                                              string?           category = null,
                                                              string?           tag      = null,
                                                              string?           sort     = null,
                                                              bool              reverse  = false,
                                                              int               limit    = 0,
                                                              int               offset   = 0,
                                                              List<string>?     hashList = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "filter", filter.ToString().ToLower() }
        };

        if (!string.IsNullOrEmpty(category)) parameters.Add("category", category);
        if (!string.IsNullOrEmpty(tag)) parameters.Add("tag", tag);
        if (!string.IsNullOrEmpty(sort)) parameters.Add("sort", sort);
        if (reverse) parameters.Add("reverse", "true");
        if (limit  > 0) parameters.Add("limit", limit.ToString());
        if (offset > 0) parameters.Add("offset", offset.ToString());
        if (hashList is { Count: > 0 })
        {
            parameters.Add("hashes", string.Join("|", hashList));
        }

        var response = await netUtils.PostAsync($"{BaseUrl}/info", parameters);
        return response.Item1 == HttpStatusCode.OK ? StringToTorrentInfoList(response.Item2) : new List<TorrentInfo>();
    }

    public async Task<List<TorrentInfo>> GetTorrentInfosAsync(GetTorrentInfoListRequest request)
    {
        var parameters = new Dictionary<string, string>
        {
            { "filter", request.Filter.ToString().ToLower() }
        };

        if (!string.IsNullOrEmpty(request.Category)) parameters.Add("category", request.Category);
        if (!string.IsNullOrEmpty(request.Tag)) parameters.Add("tag", request.Tag);
        if (!string.IsNullOrEmpty(request.Sort)) parameters.Add("sort", request.Sort);
        if (request.Reverse) parameters.Add("reverse", "true");
        if (request.Limit  > 0) parameters.Add("limit", request.Limit.ToString());
        if (request.Offset > 0) parameters.Add("offset", request.Offset.ToString());
        if (request.HashList is { Count: > 0 })
        {
            parameters.Add("hashes", string.Join("|", request.HashList));
        }

        var response = await netUtils.PostAsync($"{BaseUrl}/info", parameters);
        return response.Item1 == HttpStatusCode.OK ? StringToTorrentInfoList(response.Item2) : new List<TorrentInfo>();
    }


    public static List<TorrentInfo> StringToTorrentInfoList(string jsonString)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TorrentInfoConverter());

        var torrentInfos = JsonSerializer.Deserialize<List<TorrentInfo>>(jsonString, options) ??
                           new List<TorrentInfo>();
        return torrentInfos;
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

    public void DeleteTorrent(List<string> hashList, bool deleteFile = false) =>
        DeleteTorrent(string.Join('|', hashList.ToArray()), deleteFile);


    public async Task DeleteTorrentAsync(string hash, bool deleteFile = false)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
            { "deleteFiles", deleteFile.ToString().ToLower() }
        };

        await netUtils.PostAsync($"{BaseUrl}/delete", parameters);
    }

    public async Task DeleteTorrentAsync(List<string> hashList, bool deleteFile = false) =>
        await DeleteTorrentAsync(string.Join('|', hashList.ToArray()), deleteFile);


    public void ResumeTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };
        netUtils.Post(apiVersion < _apiVersion5 ? $"{BaseUrl}/resume" : $"{BaseUrl}/start", parameters);
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
        if (apiVersion < _apiVersion5)
            await netUtils.PostAsync($"{BaseUrl}/resume", parameters);
        else
            await netUtils.PostAsync($"{BaseUrl}/start", parameters);
    }

    public async Task ResumeTorrentAsync(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await ResumeTorrentAsync(hash);
    }

    public void ReannounceTorrent(string hash)
    {
        if (apiVersion < new ApiVersion("2.0.2"))
        {
            throw new Exception("This method was introduced with qBittorrent v4.1.2 (Web API v2.0.2).");
        }

        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        netUtils.Post($"{BaseUrl}/reannounce", parameters);
    }

    public void ReannounceTorrent(List<string> hashList) =>
        ReannounceTorrent(string.Join('|', hashList.ToArray()));


    public async Task ReannounceTorrentAsync(string hash)
    {
        if (apiVersion < new ApiVersion("2.0.2"))
        {
            throw new Exception("This method was introduced with qBittorrent v4.1.2 (Web API v2.0.2).");
        }

        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        await netUtils.PostAsync($"{BaseUrl}/reannounce", parameters);
    }

    public async Task ReannounceTorrentAsync(List<string> hashList) =>
        await ReannounceTorrentAsync(string.Join('|', hashList.ToArray()));

    public void RecheckTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };

        netUtils.Post($"{BaseUrl}/recheck", parameters);
    }

    public void RecheckTorrent(List<string> hashList) =>
        RecheckTorrent(string.Join('|', hashList.ToArray()));

    public async Task RecheckTorrentAsync(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };
        await netUtils.PostAsync($"{BaseUrl}/recheck", parameters);
    }

    public async Task RecheckTorrentAsync(List<string> hashList) =>
        await RecheckTorrentAsync(string.Join('|', hashList.ToArray()));

    /// <summary>
    /// 暂停Torrent，5.0.0开始使用stop而不是pause
    /// </summary>
    public void StopTorrent(string hash) => PauseTorrent(hash);

    /// <summary>
    /// 暂停Torrent，5.0.0之前使用pause，之后更新成stop
    /// </summary>
    public void PauseTorrent(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };
        netUtils.Post(apiVersion < _apiVersion5 ? $"{BaseUrl}/pause" : $"{BaseUrl}/stop", parameters);
    }

    /// <summary>
    /// 暂停Torrent，5.0.0开始使用stop而不是pause
    /// </summary>
    public void StopTorrent(List<string> hashList) => PauseTorrent(hashList);

    /// <summary>
    /// 暂停Torrent，5.0.0之前使用pause，之后更新成stop
    /// </summary>
    public void PauseTorrent(List<string> hashList) => PauseTorrent(string.Join('|', hashList.ToArray()));

    /// <summary>
    /// 暂停Torrent，5.0.0开始使用stop而不是pause
    /// </summary>
    public async Task StopTorrentAsync(string hash) => await PauseTorrentAsync(hash);

    /// <summary>
    /// 暂停Torrent，5.0.0之前使用pause，之后更新成stop
    /// </summary>
    public async Task PauseTorrentAsync(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hashes", hash },
        };
        if (apiVersion < _apiVersion5)
            await netUtils.PostAsync($"{BaseUrl}/pause", parameters);
        else
            await netUtils.PostAsync($"{BaseUrl}/stop", parameters);
    }

    /// <summary>
    /// 暂停Torrent，5.0.0开始使用stop而不是pause
    /// </summary>
    public async Task StopTorrentAsync(List<string> hashList) => await PauseTorrentAsync(hashList);

    /// <summary>
    /// 暂停Torrent，5.0.0之前使用pause，之后更新成stop
    /// </summary>
    public async Task PauseTorrentAsync(List<string> hashList) =>
        await PauseTorrentAsync(string.Join('|', hashList.ToArray()));

    public List<TrackerInfo> GetTrackerList(string hash)
    {
        var parameters = new Dictionary<string, string>
        {
            { "hash", hash },
        };
        var response = netUtils.Post($"{BaseUrl}/trackers", parameters);
        if (response.Item1 == HttpStatusCode.NotFound) return new List<TrackerInfo>();
        try
        {
            var trackers = JsonSerializer.Deserialize<List<TrackerInfo>>(response.Item2);
            if (trackers != null)
            {
                return trackers;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON 解析失败: {ex.Message}");
        }

        return new List<TrackerInfo>();
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
        bool?         stopped                = null,
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
            Stopped                = stopped,
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

    public List<TorrentFileInfo> GetTorrentContents(string hash, List<int>? indexes = null)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        const string requestUrl = $"{BaseUrl}/files";
        var parameters = new Dictionary<string, string>
        {
            { "hash", hash }
        };

        if (indexes is { Count: > 0 })
        {
            parameters["indexes"] = string.Join("|", indexes);
        }

        var response = netUtils.Post(requestUrl, parameters);

        if (response.Item1 == HttpStatusCode.NotFound)
        {
            return new List<TorrentFileInfo>();
        }

        var fileList = JsonSerializer.Deserialize<List<TorrentFileInfo>>(response.Item2, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return fileList ?? new List<TorrentFileInfo>();
    }

    public async Task<List<TorrentFileInfo>> GetTorrentContentsAsync(string hash, List<int>? indexes = null)
    {
        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        const string requestUrl = $"{BaseUrl}/files";
        var parameters = new Dictionary<string, string>
        {
            { "hash", hash }
        };

        if (indexes is { Count: > 0 })
        {
            parameters["indexes"] = string.Join("|", indexes);
        }

        var response = await netUtils.PostAsync(requestUrl, parameters);

        if (response.Item1 == HttpStatusCode.NotFound)
        {
            return new List<TorrentFileInfo>();
        }

        var fileList = JsonSerializer.Deserialize<List<TorrentFileInfo>>(response.Item2, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return fileList ?? new List<TorrentFileInfo>();
    }

    public bool RenameTorrentFile(string hash, string oldPath, string newPath)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        if (string.IsNullOrWhiteSpace(oldPath))
        {
            throw new ArgumentException("Old path cannot be null or empty", nameof(oldPath));
        }

        if (string.IsNullOrWhiteSpace(newPath))
        {
            throw new ArgumentException("New path cannot be null or empty", nameof(newPath));
        }

        var parameters = new Dictionary<string, string>
        {
            { "hash", hash },
            { "oldPath", oldPath },
            { "newPath", newPath }
        };

        var response = netUtils.Post($"{BaseUrl}/renameFile", parameters);

        switch (response.Item1)
        {
            case HttpStatusCode.OK :
                return true;
            case HttpStatusCode.BadRequest :
                Console.WriteLine("Error: Missing newPath parameter.");
                return false;
            case HttpStatusCode.Conflict :
                Console.WriteLine("Error: Invalid newPath, oldPath, or newPath is already in use.");
                return false;
            default :
                Console.WriteLine($"Unexpected error: {response.Item1}");
                return false;
        }
    }

    public async Task<bool> RenameTorrentFileAsync(string hash, string oldPath, string newPath)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        if (string.IsNullOrWhiteSpace(oldPath))
        {
            throw new ArgumentException("Old path cannot be null or empty", nameof(oldPath));
        }

        if (string.IsNullOrWhiteSpace(newPath))
        {
            throw new ArgumentException("New path cannot be null or empty", nameof(newPath));
        }

        var parameters = new Dictionary<string, string>
        {
            { "hash", hash },
            { "oldPath", oldPath },
            { "newPath", newPath }
        };

        var response = await netUtils.PostAsync($"{BaseUrl}/renameFile", parameters);

        switch (response.Item1)
        {
            case HttpStatusCode.OK :
                return true;
            case HttpStatusCode.BadRequest :
                Console.WriteLine("Error: Missing newPath parameter.");
                return false;
            case HttpStatusCode.Conflict :
                Console.WriteLine("Error: Invalid newPath, oldPath, or newPath is already in use.");
                return false;
            default :
                Console.WriteLine($"Unexpected error: {response.Item1}");
                return false;
        }
    }

    public bool RenameTorrentFolder(string hash, string oldPath, string newPath)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        if (string.IsNullOrWhiteSpace(oldPath))
        {
            throw new ArgumentException("Old path cannot be null or empty", nameof(oldPath));
        }

        if (string.IsNullOrWhiteSpace(newPath))
        {
            throw new ArgumentException("New path cannot be null or empty", nameof(newPath));
        }

        var parameters = new Dictionary<string, string>
        {
            { "hash", hash },
            { "oldPath", oldPath },
            { "newPath", newPath }
        };

        var response = netUtils.Post($"{BaseUrl}/renameFolder", parameters);

        switch (response.Item1)
        {
            case HttpStatusCode.OK :
                return true;
            case HttpStatusCode.BadRequest :
                Console.WriteLine("Error: Missing newPath parameter.");
                return false;
            case HttpStatusCode.Conflict :
                Console.WriteLine("Error: Invalid newPath, oldPath, or newPath is already in use.");
                return false;
            default :
                Console.WriteLine($"Unexpected error: {response.Item1}");
                return false;
        }
    }

    public async Task<bool> RenameTorrentFolderAsync(string hash, string oldPath, string newPath)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentException("Torrent hash cannot be null or empty", nameof(hash));
        }

        if (string.IsNullOrWhiteSpace(oldPath))
        {
            throw new ArgumentException("Old path cannot be null or empty", nameof(oldPath));
        }

        if (string.IsNullOrWhiteSpace(newPath))
        {
            throw new ArgumentException("New path cannot be null or empty", nameof(newPath));
        }

        var parameters = new Dictionary<string, string>
        {
            { "hash", hash },
            { "oldPath", oldPath },
            { "newPath", newPath }
        };

        var response = await netUtils.PostAsync($"{BaseUrl}/renameFolder", parameters);

        switch (response.Item1)
        {
            case HttpStatusCode.OK :
                return true;
            case HttpStatusCode.BadRequest :
                Console.WriteLine("Error: Missing newPath parameter.");
                return false;
            case HttpStatusCode.Conflict :
                Console.WriteLine("Error: Invalid newPath, oldPath, or newPath is already in use.");
                return false;
            default :
                Console.WriteLine($"Unexpected error: {response.Item1}");
                return false;
        }
    }
}
