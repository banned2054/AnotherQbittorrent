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
}
