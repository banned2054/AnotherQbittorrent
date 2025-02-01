using AnotherQbittorrent.Models.Enums;

namespace AnotherQbittorrent.Models.Torrent;

public class TorrentFileInfo
{
    public int?                    Index        { get; set; }
    public string                  Name         { get; set; } = string.Empty;
    public long                    Size         { get; set; }
    public double                  Progress     { get; set; }
    public EnumTorrentFilePriority Priority     { get; set; }
    public bool                    IsSeed       { get; set; }
    public int[]                   PieceRange   { get; set; } = Array.Empty<int>();
    public double?                 Availability { get; set; }
}
