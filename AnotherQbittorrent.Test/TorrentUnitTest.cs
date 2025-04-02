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
                "D:\\Downloads\\[U2][jsum][Mahou Tsukai Precure!][01-50][1080p][JPSC].mp4.torrent"
            };
        var tags = "byr-anime";
        await _client.Torrent.AddTorrentAsync(
                                              filePaths : torrentPaths,
                                              savePath : "/downloads",
                                              stopped : true,
                                              tags : tags,
                                              rename : "[Anime]魔法使光之美少女",
                                              ratioLimit : -1
                                             );
    }

    [Test]
    public async Task GetTorrentTrackers()
    {
        var trackers = await _client.Torrent.GetTorrentInfosAsync(hash : "d092ce98ea297205dfb30433e127b9980a2dd23f");
    }
}
