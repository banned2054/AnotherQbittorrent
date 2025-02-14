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

    [Test]
    public async Task SelfCheckAsync()
    {
        const string hash = "6cf662d0310a8dd42e4648366dc32b3a260ebf38";
        await _client.Torrent.RecheckTorrentAsync(hash);

        await Task.Delay(5000); // 先等 5 秒，避免立即查询

        var hashList = new List<string> { hash };
        var infoList = await _client.Torrent.GetTorrentInfosAsync(hashList : hashList);
        if (infoList == null || infoList.Count == 0) return;

        var info = infoList[0];

        int       retryCount = 0;
        const int maxRetries = 30; // 最多 60 秒超时退出

        while (info.State == EnumTorrentState.CheckingDownload ||
               info.State == EnumTorrentState.CheckingUpload)
        {
            Console.WriteLine($"状态未完成: {info.State}, 当前时间: {DateTime.Now}");

            if (++retryCount >= maxRetries)
            {
                Console.WriteLine("检测超时，强制退出循环。");
                break;
            }

            await Task.Delay(2000); // 改为 2 秒轮询，提升更新频率

            infoList = await _client.Torrent.GetTorrentInfosAsync(hashList : hashList);
            if (infoList == null || infoList.Count == 0) continue;

            info = infoList[0];
        }

        Console.WriteLine("状态检查完成！");
    }
}
