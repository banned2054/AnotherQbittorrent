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
    public async Task TestDeleteTorrent()
    {
        await _client.Torrent.DeleteTorrentAsync("b963306bd91ff97492079b5510e91a111757322f");
    }

    [Test]
    public async Task TestAddTorrent()
    {
        var torrentPaths =
            new List<string>
            {
                "D:\\Downloads\\Your.Forma.S01E01.1080p.BILI.WEB-DL.AAC2.0.H.264-VARYG.torrent"
            };
        var tags = "test";
        await _client.Torrent.AddTorrentAsync(
                                              filePaths : torrentPaths,
                                              savePath : "/downloads",
                                              stopped : true,
                                              tags : tags,
                                              rename : "test-download"
                                             );
    }

    [Test]
    public async Task GetTorrentTrackers()
    {
        var trackers = await _client.Torrent.GetTorrentInfosAsync(hash : "d092ce98ea297205dfb30433e127b9980a2dd23f");
    }
}
