using System.Text.Json;
using AnotherQbittorrent.Models.Enums;

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
    public void TestGetTorrentList()
    {
        var torrentList = _client.Torrent.GetTorrentInfos(EnumTorrentFilter.All, null, "tv");
        if (torrentList != null)
            foreach (var torrent in torrentList)
            {
                Console.WriteLine(JsonSerializer.Serialize(torrent));
            }

        Assert.Pass();
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
                "D:\\Downloads\\[BYRBT].[Sakurato] Okinawa de Suki ni Natta Ko ga Hougen Sugite Tsura Sugiru [04][AVC-8bit 1080p AAC][CHS&JPN].mp4.torrent"
            };
        var tags = "tv,mikan";
        await _client.Torrent.AddTorrentAsync(
                                              filePaths : torrentPaths,
                                              savePath : "/downloads/Media/Anime",
                                              paused : true,
                                              tags : tags,
                                              rename : "[Anime]在冲绳喜欢上的女孩方言讲得太过令人困扰 E04",
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
