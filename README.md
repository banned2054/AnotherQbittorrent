# AnotherQbittorrent

参考python的[qbittorrent-api](https://github.com/rmartin16/qbittorrent-api)和官方wiki，开发的第三方qbittorrent sdk，相比[qbittorrent-net-client](https://github.com/fedarovich/qbittorrent-net-client)，修改了很多api的访问，从get改成了post（否则很多数据不能及时更新）。

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