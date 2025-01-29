using System.Text.Json;

namespace AnotherQbittorrent.Test;

public class ApplicationUnitTest
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
        var config = JsonSerializer.Deserialize<TorrentUnitTest.TestConfig>(File.ReadAllText("test-config.json"));
        _client = new QBittorrentClient(config.BaseUrl, config.Username, config.Password);
    }

    [Test]
    public void TestGetVersion()
    {
        var version = _client.Application.GetApiVersion();
        Console.WriteLine(version);
        Assert.Pass();
    }

    [Test]
    public void TestGetBuildInfo()
    {
        var buildInfo = _client.Application.GetBuildInfo();
        Console.WriteLine(buildInfo);
        Assert.Pass();
    }
}