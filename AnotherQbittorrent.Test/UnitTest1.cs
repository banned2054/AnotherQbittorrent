using AnotherQbittorrent.Models.Enums;
using System.Text.Json;
using AnotherQbittorrent.Services;

namespace AnotherQbittorrent.Test;

public class Tests
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
        // �� JSON �ļ��ж�ȡ����
        var config = JsonSerializer.Deserialize<TestConfig>(File.ReadAllText("test-config.json"));
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

    [Test]
    public void TestGetTorrentList()
    {
        var torrentList = _client.Torrent.GetTorrentInfos(EnumTorrentFilter.All, null, "tv");
        Assert.Pass();
    }
}