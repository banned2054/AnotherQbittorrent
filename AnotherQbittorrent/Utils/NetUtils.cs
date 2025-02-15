using System.Net;
using System.Net.Http.Headers;

namespace AnotherQbittorrent.Utils;

public class NetUtils
{
    private readonly HttpClient      _client;
    private readonly CookieContainer _cookieContainer;
    private readonly string          _userName;
    private readonly string          _password;
    private readonly string          _url;
    private readonly object          _lock = new();

    public NetUtils(string baseUrl, string userName, string password)
    {
        _url             = baseUrl;
        _userName        = userName;
        _password        = password;
        _cookieContainer = new CookieContainer();

        var handler = new HttpClientHandler
        {
            CookieContainer   = _cookieContainer,
            AllowAutoRedirect = true,
            UseCookies        = true
        };

        _client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = true
        };
    }

    /// <summary>
    /// 通用 GET 请求（同步）
    /// </summary>
    public (HttpStatusCode, string) Get(string subPath)
    {
        EnsureLoggedIn();

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}{subPath}");
        request.Headers.ConnectionClose = true;

        return ExecuteWithRetry(() => _client.Send(request));
    }

    /// <summary>
    /// 通用 GET 请求（异步）
    /// </summary>
    public async Task<(HttpStatusCode, string)> GetAsync(string subPath)
    {
        await EnsureLoggedInAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}{subPath}");
        request.Headers.ConnectionClose = true;

        return await ExecuteWithRetryAsync(() => _client.SendAsync(request));
    }

    /// <summary>
    /// 通用 POST 请求（同步）
    /// </summary>
    public (HttpStatusCode, string) Post(string subPath, Dictionary<string, string> parameters)
    {
        EnsureLoggedIn();

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}{subPath}")
        {
            Content = new FormUrlEncodedContent(parameters)
        };
        request.Headers.ConnectionClose = true;

        return ExecuteWithRetry(() => _client.Send(request));
    }

    /// <summary>
    /// 通用 POST 请求（异步）
    /// </summary>
    public async Task<(HttpStatusCode, string)> PostAsync(string subPath, Dictionary<string, string> parameters)
    {
        await EnsureLoggedInAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}{subPath}")
        {
            Content = new FormUrlEncodedContent(parameters)
        };
        request.Headers.ConnectionClose = true;

        return await ExecuteWithRetryAsync(() => _client.SendAsync(request));
    }

    /// <summary>
    /// 通用 POST 文件上传（同步）
    /// </summary>
    public (HttpStatusCode, string) PostWithFiles(string       subPath, Dictionary<string, string> parameters,
                                                  List<string> filePaths)
    {
        EnsureLoggedIn();

        using var content = new MultipartFormDataContent();
        foreach (var param in parameters)
        {
            content.Add(new StringContent(param.Value), param.Key);
        }

        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath))
            {
                content.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "torrents", Path.GetFileName(filePath));
            }
            else
            {
                Console.WriteLine($"文件未找到: {filePath}");
                return (HttpStatusCode.BadRequest, $"File not found: {filePath}");
            }
        }

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/{subPath}") { Content = content };
        request.Headers.ConnectionClose = true;

        return ExecuteWithRetry(() => _client.Send(request));
    }

    /// <summary>
    /// 通用 POST 文件上传（异步）
    /// </summary>
    public async Task<(HttpStatusCode, string)> PostWithFilesAsync(string                     subPath,
                                                                   Dictionary<string, string> parameters,
                                                                   List<string>               filePaths)
    {
        await EnsureLoggedInAsync();

        using var content = new MultipartFormDataContent();
        foreach (var param in parameters)
        {
            content.Add(new StringContent(param.Value), param.Key);
        }

        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath))
            {
                content.Add(new ByteArrayContent(await File.ReadAllBytesAsync(filePath)), "torrents",
                            Path.GetFileName(filePath));
            }
            else
            {
                Console.WriteLine($"文件未找到: {filePath}");
                return (HttpStatusCode.BadRequest, $"File not found: {filePath}");
            }
        }

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_url}/{subPath}") { Content = content };
        request.Headers.ConnectionClose = true;

        return await ExecuteWithRetryAsync(() => _client.SendAsync(request));
    }

    /// <summary>
    /// 确保已登录（异步）
    /// </summary>
    private async Task EnsureLoggedInAsync()
    {
        lock (_lock)
        {
            if (_cookieContainer.GetCookies(new Uri(_url)).Count > 0) return;
        }

        await LoginAsync();
    }

    /// <summary>
    /// 确保已登录（同步）
    /// </summary>
    private void EnsureLoggedIn()
    {
        lock (_lock)
        {
            if (_cookieContainer.GetCookies(new Uri(_url)).Count > 0) return;
        }

        Login();
    }

    /// <summary>
    /// 登录（异步）
    /// </summary>
    private async Task LoginAsync()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "username", _userName },
            { "password", _password }
        });

        var response = await _client.PostAsync($"{_url}/api/v2/auth/login", content);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Login failed: {response.StatusCode}");
        }
    }

    /// <summary>
    /// 登录（同步）
    /// </summary>
    private void Login()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "username", _userName },
            { "password", _password }
        });

        var response = _client.PostAsync($"{_url}/api/v2/auth/login", content).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Login failed: {response.StatusCode}");
        }
    }

    /// <summary>
    /// 自动重试逻辑（同步）
    /// </summary>
    private static (HttpStatusCode, string) ExecuteWithRetry(Func<HttpResponseMessage> action, int maxRetries = 3)
    {
        for (var retry = 0; retry < maxRetries; retry++)
        {
            var response = action();
            if (response.StatusCode != HttpStatusCode.InternalServerError)
                return (response.StatusCode, response.Content.ReadAsStringAsync().Result);

            Thread.Sleep(500 * (retry + 1));
        }

        return (HttpStatusCode.InternalServerError, "Max retry limit reached.");
    }

    /// <summary>
    /// 自动重试逻辑（异步）
    /// </summary>
    private static async Task<(HttpStatusCode, string)> ExecuteWithRetryAsync(Func<Task<HttpResponseMessage>> action,
                                                                              int maxRetries = 3)
    {
        for (var retry = 0; retry < maxRetries; retry++)
        {
            var response = await action();
            if (response.StatusCode != HttpStatusCode.InternalServerError)
                return (response.StatusCode, await response.Content.ReadAsStringAsync());

            await Task.Delay(500 * (retry + 1));
        }

        return (HttpStatusCode.InternalServerError, "Max retry limit reached.");
    }
}
