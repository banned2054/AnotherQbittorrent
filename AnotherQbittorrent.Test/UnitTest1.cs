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
        // 从 JSON 文件中读取配置
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

    [Test]
    public void TestStringToTorrentInfo()
    {
        var a =
            "[{\"dlspeed\":9681262,\"eta\":87,\"f_l_piece_prio\":false,\"force_start\":false,\"hash\":\"8c212779b4abde7c6bc608063a0d008b7e40ce32\",\"category\":\"\",\"tags\": \"\",\"name\":\"debian-8.1.0-amd64-CD-1.iso\",\"num_complete\":-1,\"num_incomplete\":-1,\"num_leechs\":2,\"num_seeds\":54,\"priority\":1,\"progress\":0.16108787059783936,\"ratio\":0,\"seq_dl\":false,\"size\":657457152,\"state\":\"downloading\",\"super_seeding\":false,\"upspeed\":0,\"isPrivate\":true}]";
        var b = TorrentService.StringToTorrentInfoList(a);
    }
}