using AnotherQbittorrent.Models.Enums;
using AnotherQbittorrent.Models.Requests;
using AnotherQbittorrent.Models.Torrent;
using AnotherQbittorrent.Utils;
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

        var stringResponse = netUtils.Fetch($"{BaseUrl}/info{requestPara}");
        return StringToTorrentInfoList(stringResponse);
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

        var stringResponse = await netUtils.AsyncFetch($"{BaseUrl}/info{requestPara}");
        return StringToTorrentInfoList(stringResponse);
    }

    public static List<TorrentInfo>? StringToTorrentInfoList(string stringResponse)
    {
        try
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TorrentInfoConverter());

            var torrentInfos = JsonSerializer.Deserialize<List<TorrentInfo>>(stringResponse, options);
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
        netUtils.Fetch($"{BaseUrl}/delete?hashes={hash}&deleteFiles={deleteFile.ToString().ToLower()}");
    }

    public void DeleteTorrent(List<string> hashList, bool deleteFile = false)
    {
        var hash = string.Join('|', hashList.ToArray());
        DeleteTorrent(hash, deleteFile);
    }

    public async Task AsyncDeleteTorrent(string hash, bool deleteFile = false)
    {
        await netUtils.AsyncFetch($"{BaseUrl}/delete?hashes={hash}&deleteFiles={deleteFile.ToString().ToLower()}");
    }

    public async Task AsyncDeleteTorrent(List<string> hashList, bool deleteFile = false)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncDeleteTorrent(hash, deleteFile);
    }

    public void ResumeTorrent(string hash)
    {
        netUtils.Fetch($"{BaseUrl}/resume?hashes={hash}");
    }

    public void ResumeTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ResumeTorrent(hash);
    }

    public async Task AsyncResumeTorrent(string hash)
    {
        await netUtils.AsyncFetch($"{BaseUrl}/resume?hashes={hash}");
    }

    public async Task AsyncResumeTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncResumeTorrent(hash);
    }

    public void ReannounceTorrent(string hash)
    {
        netUtils.Fetch($"{BaseUrl}/reannounce?hashes={hash}");
    }

    public void ReannounceTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        ReannounceTorrent(hash);
    }

    public async Task AsyncReannounceTorrent(string hash)
    {
        await netUtils.AsyncFetch($"{BaseUrl}/reannounce?hashes={hash}");
    }

    public async Task AsyncReannounceTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncReannounceTorrent(hash);
    }

    public void RecheckTorrent(string hash)
    {
        netUtils.Fetch($"{BaseUrl}/recheck?hashes={hash}");
    }

    public void RecheckTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        RecheckTorrent(hash);
    }

    public async Task AsyncRecheckTorrent(string hash)
    {
        await netUtils.AsyncFetch($"{BaseUrl}/recheck?hashes={hash}");
    }

    public async Task AsyncRecheckTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncRecheckTorrent(hash);
    }

    public void PauseTorrent(string hash)
    {
        netUtils.Fetch($"{BaseUrl}/pause?hashes={hash}");
    }

    public void PauseTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        PauseTorrent(hash);
    }

    public async Task AsyncPauseTorrent(string hash)
    {
        await netUtils.AsyncFetch($"{BaseUrl}/pause?hashes={hash}");
    }

    public async Task AsyncPauseTorrent(List<string> hashList)
    {
        var hash = string.Join('|', hashList.ToArray());
        await AsyncPauseTorrent(hash);
    }
}