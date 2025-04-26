# AnotherQbittorrent

A .NET client library for qBittorrent's Web API, inspired by [qbittorrent-api](https://github.com/rmartin16/qbittorrent-api) and the official qBittorrent wiki. This library improves upon [qbittorrent-net-client](https://github.com/fedarovich/qbittorrent-net-client) by using POST requests instead of GET for better data synchronization.

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package AnotherQbittorrent
```

## Quick Start

```csharp
using AnotherQbittorrent;

// Create a client instance
var client = new QbittorrentClient("http://localhost:8080");

// Login (if authentication is enabled)
await client.LoginAsync("username", "password");

// Get torrent list
var torrents = await client.GetTorrentListAsync();

// Add a new torrent
await client.AddTorrentAsync("magnet:?xt=urn:btih:...");

// Pause torrents
await client.PauseTorrentsAsync(new[] { "torrent_hash" });

// Resume torrents
await client.ResumeTorrentsAsync(new[] { "torrent_hash" });
```

## Version

Current version: 0.1.5

## License

This project is licensed under the Apache License 2.0.

## Contributing

Contributions are welcome! Feel free to submit issues and pull requests.

## Documentation

For Chinese documentation, please refer to [Docs/README.md](Docs/README.md).