using System.Text.Json;
using AnotherQbittorrent.Models.Enums;

namespace AnotherQbittorrent.Test;

public class TorrentUnitTest
{
    public class TestConfig
    {
        public string BaseUrl  { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    private QBittorrentClient _client;

    [SetUp]
    public void SetUp()
    {
        // 从 JSON 文件中读取配置
        var config = JsonSerializer.Deserialize<TestConfig>(File.ReadAllText("test-config.json"));
        _client = new QBittorrentClient(config.BaseUrl, config.Username, config.Password);
    }

    [Test]
    public void TestGetTorrentList()
    {
        var torrentList = _client.Torrent.GetTorrentInfos(EnumTorrentFilter.All, null, "tv");
        Assert.Pass();
    }
}