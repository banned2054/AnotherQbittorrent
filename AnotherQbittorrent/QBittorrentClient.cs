using AnotherQbittorrent.Services;
using AnotherQbittorrent.Utils;

namespace AnotherQbittorrent;

public class QBittorrentClient
{
    public ApplicationService Application;
    public TorrentService     Torrent;

    public QBittorrentClient(string url, string userName, string password)
    {
        var netUtils = new NetUtils(url, userName, password);

        Application = new ApplicationService(netUtils);
        var apiVersion = Application.GetApiVersion();

        Torrent = new TorrentService(netUtils, apiVersion);
    }
}
