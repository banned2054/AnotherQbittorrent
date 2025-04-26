namespace Banned.Qbittorrent.Models.Enums;

public enum EnumTorrentState
{
    Error,
    MissingFiles,
    Uploading,

    /// <summary>
    /// 上传已暂停
    /// <c>qBittorrent</c>5.0.0后 <c>stoppedUP</c> 替代了 <c>pausedUP</c>。
    /// </summary>
    PausedUpload,

    /// <summary>
    /// 上传已停止
    /// qBittorrent5.0.0后 <c>stoppedUP</c> 替代了 <c>pausedUP</c>。
    /// </summary>
    StoppedUpload,
    QueuedUpload,
    StalledUpload,
    CheckingUpload,
    ForcedUpload,
    Allocating,
    Downloading,
    MetaDownload,

    /// <summary>
    /// 上传已暂停,
    /// <c>qBittorrent</c>5.0.0后 <c>stoppedDL</c> 替代了 <c>pausedDL</c>。
    /// </summary>
    PausedDownload,

    /// <summary>
    /// 上传已暂停,
    /// <c>qBittorrent</c>5.0.0后 <c>stoppedDL</c> 替代了 <c>pausedDL</c>。
    /// </summary>
    StoppedDownload,
    QueuedDownload,
    StalledDownload,
    CheckingDownload,
    ForcedDownload,
    CheckingResumeData,
    Moving,
    Unknown,
}

internal static class EnumTorrentStateExtensions
{
    public static string ToTorrentStateStringV4(this EnumTorrentState value)
    {
        return value switch
        {
            EnumTorrentState.Error              => "error",
            EnumTorrentState.MissingFiles       => "missingFiles",
            EnumTorrentState.Uploading          => "uploading",
            EnumTorrentState.PausedUpload       => "pausedUP",
            EnumTorrentState.StoppedUpload      => "pausedUP",
            EnumTorrentState.QueuedUpload       => "queuedUP",
            EnumTorrentState.StalledUpload      => "stalledUP",
            EnumTorrentState.CheckingUpload     => "checkingUP",
            EnumTorrentState.ForcedUpload       => "forcedUP",
            EnumTorrentState.Allocating         => "allocating",
            EnumTorrentState.Downloading        => "downloading",
            EnumTorrentState.MetaDownload       => "metaDL",
            EnumTorrentState.PausedDownload     => "pausedDL",
            EnumTorrentState.StoppedDownload    => "pausedDL",
            EnumTorrentState.QueuedDownload     => "queuedDL",
            EnumTorrentState.StalledDownload    => "stalledDL",
            EnumTorrentState.CheckingDownload   => "checkingDL",
            EnumTorrentState.ForcedDownload     => "forcedDL",
            EnumTorrentState.CheckingResumeData => "checkingResumeData",
            EnumTorrentState.Moving             => "moving",
            EnumTorrentState.Unknown            => "unknown",
            _                                   => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string ToTorrentStateStringV5(this EnumTorrentState value)
    {
        return value switch
        {
            EnumTorrentState.Error              => "error",
            EnumTorrentState.MissingFiles       => "missingFiles",
            EnumTorrentState.Uploading          => "uploading",
            EnumTorrentState.StoppedUpload      => "stoppedUP",
            EnumTorrentState.PausedUpload       => "stoppedUP",
            EnumTorrentState.QueuedUpload       => "queuedUP",
            EnumTorrentState.StalledUpload      => "stalledUP",
            EnumTorrentState.CheckingUpload     => "checkingUP",
            EnumTorrentState.ForcedUpload       => "forcedUP",
            EnumTorrentState.Allocating         => "allocating",
            EnumTorrentState.Downloading        => "downloading",
            EnumTorrentState.MetaDownload       => "metaDL",
            EnumTorrentState.PausedDownload     => "stoppedDL",
            EnumTorrentState.StoppedDownload    => "stoppedDL",
            EnumTorrentState.QueuedDownload     => "queuedDL",
            EnumTorrentState.StalledDownload    => "stalledDL",
            EnumTorrentState.CheckingDownload   => "checkingDL",
            EnumTorrentState.ForcedDownload     => "forcedDL",
            EnumTorrentState.CheckingResumeData => "checkingResumeData",
            EnumTorrentState.Moving             => "moving",
            EnumTorrentState.Unknown            => "unknown",
            _                                   => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static EnumTorrentState FromTorrentStateStringV4(string value)
    {
        return value switch
        {
            "error"              => EnumTorrentState.Error,
            "missingFiles"       => EnumTorrentState.MissingFiles,
            "uploading"          => EnumTorrentState.Uploading,
            "pausedUP"           => EnumTorrentState.PausedUpload,
            "stoppedUP"          => EnumTorrentState.PausedUpload,
            "queuedUP"           => EnumTorrentState.QueuedUpload,
            "stalledUP"          => EnumTorrentState.StalledUpload,
            "checkingUP"         => EnumTorrentState.CheckingUpload,
            "forcedUP"           => EnumTorrentState.ForcedUpload,
            "allocating"         => EnumTorrentState.Allocating,
            "downloading"        => EnumTorrentState.Downloading,
            "metaDL"             => EnumTorrentState.MetaDownload,
            "pausedDL"           => EnumTorrentState.PausedDownload,
            "stoppedDL"          => EnumTorrentState.PausedDownload,
            "queuedDL"           => EnumTorrentState.QueuedDownload,
            "stalledDL"          => EnumTorrentState.StalledDownload,
            "checkingDL"         => EnumTorrentState.CheckingDownload,
            "forcedDL"           => EnumTorrentState.ForcedDownload,
            "checkingResumeData" => EnumTorrentState.CheckingResumeData,
            "moving"             => EnumTorrentState.Moving,
            "unknown"            => EnumTorrentState.Unknown,
            _                    => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static EnumTorrentState FromTorrentStateStringV5(string value)
    {
        return value switch
        {
            "error"              => EnumTorrentState.Error,
            "missingFiles"       => EnumTorrentState.MissingFiles,
            "uploading"          => EnumTorrentState.Uploading,
            "pausedUP"           => EnumTorrentState.StoppedUpload,
            "stoppedUP"          => EnumTorrentState.StoppedUpload,
            "queuedUP"           => EnumTorrentState.QueuedUpload,
            "stalledUP"          => EnumTorrentState.StalledUpload,
            "checkingUP"         => EnumTorrentState.CheckingUpload,
            "forcedUP"           => EnumTorrentState.ForcedUpload,
            "allocating"         => EnumTorrentState.Allocating,
            "downloading"        => EnumTorrentState.Downloading,
            "metaDL"             => EnumTorrentState.MetaDownload,
            "pausedDL"           => EnumTorrentState.StoppedDownload,
            "stoppedDL"          => EnumTorrentState.StoppedDownload,
            "queuedDL"           => EnumTorrentState.QueuedDownload,
            "stalledDL"          => EnumTorrentState.StalledDownload,
            "checkingDL"         => EnumTorrentState.CheckingDownload,
            "forcedDL"           => EnumTorrentState.ForcedDownload,
            "checkingResumeData" => EnumTorrentState.CheckingResumeData,
            "moving"             => EnumTorrentState.Moving,
            "unknown"            => EnumTorrentState.Unknown,
            _                    => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
