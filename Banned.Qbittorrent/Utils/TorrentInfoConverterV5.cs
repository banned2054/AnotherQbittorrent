using Banned.Qbittorrent.Models.Enums;
using Banned.Qbittorrent.Models.Torrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banned.Qbittorrent.Utils;

/// <summary>
/// Qbittorrent版本小于5.x.x的实现方法
/// </summary>
public class TorrentInfoConverterV5 : JsonConverter<TorrentInfo>
{
    public override TorrentInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 先反序列化为 Dictionary<string, object>，方便读取属性
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ref reader, options);

        // 手动映射 JSON 字段到 TorrentInfo
        return new TorrentInfo
        {
            AddedOn = FromUnixTimeSeconds(dictionary!["added_on"].GetInt64()),
            AmountLeft = dictionary["amount_left"].GetInt64(),
            AutoTmm = dictionary["auto_tmm"].GetBoolean(),
            Availability = dictionary["availability"].GetSingle(),
            Category = dictionary["category"].GetString(),
            Completed = dictionary["completed"].GetInt64(),
            CompletionOn = FromUnixTimeSeconds(dictionary["completion_on"].GetInt64()),
            ContentPath = dictionary["content_path"].GetString(),
            DlLimit = dictionary["dl_limit"].GetInt64(),
            DownloadSpeed = dictionary["dlspeed"].GetInt64(),
            Downloaded = dictionary["downloaded"].GetInt64(),
            DownloadedSession = dictionary["downloaded_session"].GetInt64(),
            Eta = TimeSpan.FromSeconds(dictionary["eta"].GetInt64()),
            FirstLastPiecePriority = dictionary["f_l_piece_prio"].GetBoolean(),
            ForceStart = dictionary["force_start"].GetBoolean(),
            Hash = dictionary["hash"].GetString(),
            IsPrivate = dictionary.TryGetValue("isPrivate", out var isPrivateElement) && isPrivateElement.GetBoolean(),
            LastActivity = FromUnixTimeSeconds(dictionary["last_activity"].GetInt64()),
            MagnetUri = dictionary["magnet_uri"].GetString(),
            MaxRatio = dictionary["max_ratio"].GetSingle(),
            MaxSeedingTime = TimeSpan.FromSeconds(dictionary["max_seeding_time"].GetInt64()),
            Name = dictionary["name"].GetString(),
            NumComplete = dictionary["num_complete"].GetInt64(),
            NumIncomplete = dictionary["num_incomplete"].GetInt64(),
            NumLeechs = dictionary["num_leechs"].GetInt64(),
            NumSeeds = dictionary["num_seeds"].GetInt64(),
            Priority = dictionary["priority"].GetInt64(),
            Progress = dictionary["progress"].GetSingle(),
            Ratio = dictionary["ratio"].GetSingle(),
            RatioLimit = dictionary["ratio_limit"].GetSingle(),
            SavePath = dictionary["save_path"].GetString(),
            SeedingTime = TimeSpan.FromSeconds(dictionary["seeding_time"].GetInt64()),
            SeedingTimeLimit = TimeSpan.FromSeconds(dictionary["seeding_time_limit"].GetInt64()),
            SeenComplete = FromUnixTimeSeconds(dictionary["seen_complete"].GetInt64()),
            SeqDl = dictionary["seq_dl"].GetBoolean(),
            Size = dictionary["size"].GetInt64(),
            State = EnumTorrentStateExtensions.FromTorrentStateStringV5(dictionary["state"].GetString()!),
            SuperSeeding = dictionary["super_seeding"].GetBoolean(),
            TagList = dictionary["tags"]
                     .GetString()!
                     .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                     .ToList(),
            TimeActive      = TimeSpan.FromSeconds(dictionary["time_active"].GetInt64()),
            TotalSize       = dictionary["total_size"].GetInt64(),
            Tracker         = dictionary["tracker"].GetString(),
            UpLimit         = dictionary["up_limit"].GetInt64(),
            Uploaded        = dictionary["uploaded"].GetInt64(),
            UploadedSession = dictionary["uploaded_session"].GetInt64(),
            UploadSpeed     = dictionary["upspeed"].GetInt64()
        };
    }

    public override void Write(Utf8JsonWriter writer, TorrentInfo value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Serialization is not implemented.");
    }

    private static DateTime FromUnixTimeSeconds(long seconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
    }
}
