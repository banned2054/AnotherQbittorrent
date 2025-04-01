namespace AnotherQbittorrent.Test;

public class ApplicationUnitTest
{
    private QBittorrentClient _client;

    [SetUp]
    public void SetUp()
    {
        // 从 JSON 文件中读取配置
        _client = new QBittorrentClient(StaticConfig.BaseUrl, StaticConfig.Username, StaticConfig.Password);
    }

    [Test]
    public void TestGetApiVersion()
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
