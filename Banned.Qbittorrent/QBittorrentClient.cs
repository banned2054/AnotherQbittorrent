using Banned.Qbittorrent.Services;
using Banned.Qbittorrent.Utils;

namespace Banned.Qbittorrent;

public class QBittorrentClient
{
    public ApplicationService Application;
    public TorrentService     Torrent;

    public QBittorrentClient(string url, string userName, string password)
    {
        var netUtils = new NetUtils(url, userName, password);

        Application = new ApplicationService(netUtils);
        var apiVersion = Application.GetApiVersionAsync().Result;

        Torrent = new TorrentService(netUtils, apiVersion);
    }
}
