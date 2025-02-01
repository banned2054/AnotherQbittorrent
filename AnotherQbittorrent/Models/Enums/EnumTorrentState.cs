namespace AnotherQbittorrent.Models.Enums;

public enum EnumTorrentState
{
    Error,
    MissingFiles,
    Uploading,
    PausedUpload,
    QueuedUpload,
    StalledUpload,
    CheckingUpload,
    ForcedUpload,
    Allocating,
    Downloading,
    MetaDownload,
    PausedDownload,
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
    public static string ToTorrentStateString(this EnumTorrentState value)
    {
        return value switch
        {
            EnumTorrentState.Error              => "error",
            EnumTorrentState.MissingFiles       => "missingFiles",
            EnumTorrentState.Uploading          => "uploading",
            EnumTorrentState.PausedUpload       => "pausedUP",
            EnumTorrentState.QueuedUpload       => "queuedUP",
            EnumTorrentState.StalledUpload      => "stalledUP",
            EnumTorrentState.CheckingUpload     => "checkingUP",
            EnumTorrentState.ForcedUpload       => "forcedUP",
            EnumTorrentState.Allocating         => "allocating",
            EnumTorrentState.Downloading        => "downloading",
            EnumTorrentState.MetaDownload       => "metaDL",
            EnumTorrentState.PausedDownload     => "pausedDL",
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

    public static EnumTorrentState FromTorrentStateString(string value)
    {
        return value switch
        {
            "error"              => EnumTorrentState.Error,
            "missingFiles"       => EnumTorrentState.MissingFiles,
            "uploading"          => EnumTorrentState.Uploading,
            "pausedUP"           => EnumTorrentState.PausedUpload,
            "queuedUP"           => EnumTorrentState.QueuedUpload,
            "stalledUP"          => EnumTorrentState.StalledUpload,
            "checkingUP"         => EnumTorrentState.CheckingUpload,
            "forcedUP"           => EnumTorrentState.ForcedUpload,
            "allocating"         => EnumTorrentState.Allocating,
            "downloading"        => EnumTorrentState.Downloading,
            "metaDL"             => EnumTorrentState.MetaDownload,
            "pausedDL"           => EnumTorrentState.PausedDownload,
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
