# [AnotherQbittorrent](https://github.com/banned2054/AnotherQbittorrent)

参考python的[qbittorrent-api](https://github.com/rmartin16/qbittorrent-api)和官方wiki，开发的第三方qbittorrent sdk，相比[qbittorrent-net-client](https://github.com/fedarovich/qbittorrent-net-client)，修改了很多api的访问，从get改成了post（否则很多数据不能及时更新）。

## 安装

通过 NuGet 包管理器安装：

```bash
dotnet add package AnotherQbittorrent
```

## 基本用法

```csharp
using AnotherQbittorrent;

// 创建客户端实例
var client = new QbittorrentClient("http://localhost:8080");

// 登录（如果启用了身份验证）
await client.LoginAsync("username", "password");

// 获取种子列表
var torrents = await client.GetTorrentListAsync();

// 添加种子
await client.AddTorrentAsync("magnet:?xt=urn:btih:...");

// 暂停种子
await client.PauseTorrentsAsync(new[] { "torrent_hash" });

// 恢复种子
await client.ResumeTorrentsAsync(new[] { "torrent_hash" });
```

## 已实现功能

#### Application

- [x] [Get application version](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-application-version)
- [x] [Get API version](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-api-version)
- [x] [Get build info](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-build-info)

#### Torrent
- [x] [Get torrent list](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-torrent-list)
- [x] [Delete torrents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#delete-torrents)
- [x] [Resume torrents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#resume-torrents)
- [x] [Reannounce torrents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#reannounce-torrents)
- [x] [Recheck torrents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#recheck-torrents)
- [x] [Pause torrents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#pause-torrents)
- [x] [Get torrent trackers](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-torrent-trackers)
- [x] [Add new torrent](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#add-new-torrent)
- [x] [Get torrent contents](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#get-torrent-contents)
- [x] [Rename file](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#rename-file)
- [x] [Rename folder](https://github.com/qbittorrent/qBittorrent/wiki/WebUI-API-(qBittorrent-4.1)#rename-folder)

## 版本信息

当前版本：0.1.5

## 许可证

本项目采用 Apache-2.0 许可证。

## 贡献

欢迎提交 Issue 和 Pull Request！