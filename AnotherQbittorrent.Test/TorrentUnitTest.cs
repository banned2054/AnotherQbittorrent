using System.Text.Json;

namespace AnotherQbittorrent.Test;

public class TorrentUnitTest
{
    private QBittorrentClient _client;

    [SetUp]
    public void SetUp()
    {
        // 从 JSON 文件中读取配置
        _client = new QBittorrentClient(StaticConfig.BaseUrl, StaticConfig.Username, StaticConfig.Password);
    }

    [Test]
    public void TestDeleteTorrent()
    {
        _client.Torrent.DeleteTorrent("b963306bd91ff97492079b5510e91a111757322f");
    }

    [Test]
    public async Task TestAddTorrent()
    {
        var torrentPaths =
            new List<string>
            {
                "D:\\Downloads\\[BYRBT].[Nekomoe kissaten][BanG Dream! Ave Mujica][07][1080p][JPSC].mp4.torrent"
            };
        var tags = "tv|byr-anime";
        await _client.Torrent.AddTorrentAsync(
                                              filePaths : torrentPaths,
                                              savePath : "/downloads",
                                              paused : true,
                                              tags : tags,
                                              rename : "[Anime]BanG Dream! Ave Mujica E07",
                                              ratioLimit : -1
                                             );
    }

    [Test]
    public void GetTorrentTrackers()
    {
        var trackers = _client.Torrent.GetTrackerList("77de2b4c96e59c23422085cfd1dbad10d440abd7");
        foreach (var tracker in trackers)
        {
            Console.WriteLine(JsonSerializer.Serialize(tracker));
        }
    }
}
