# Banned.Qbittorrent

[English Documentation](../README.md)

一个基于 .NET 的 qBittorrent Web API 客户端库，参考了 [qbittorrent-api](https://github.com/rmartin16/qbittorrent-api) 和官方 qBittorrent wiki。相比 [qbittorrent-net-client](https://github.com/fedarovich/qbittorrent-net-client)，本库将许多 API 请求从 GET 改为 POST，以获得更好的数据同步效果。

## 安装

通过 NuGet 包管理器安装：

```bash
dotnet add package Banned.Qbittorrent
```

## 快速开始

```csharp
using Banned.Qbittorrent;

// 创建客户端实例
var client = new QbittorrentClient("http://localhost:8080");

// 登录（如果启用了身份验证）
await client.LoginAsync("username", "password");

// 获取种子列表
var torrents = await client.GetTorrentListAsync();

// 添加新种子
await client.AddTorrentAsync("magnet:?xt=urn:btih:...");

// 暂停种子
await client.PauseTorrentsAsync(new[] { "torrent_hash" });

// 恢复种子
await client.ResumeTorrentsAsync(new[] { "torrent_hash" });
```

## 版本

当前版本：0.0.1

## 许可证

本项目采用 Apache-2.0 许可证。

## 贡献

欢迎提交 Issue 和 Pull Request！ 