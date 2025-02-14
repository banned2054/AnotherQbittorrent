using RestSharp;
using System.Net;

namespace AnotherQbittorrent.Utils;

public class NetUtils
{
    private readonly RestClient      _client;
    private          CookieContainer _cookieContainer;
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
        _client = new RestClient(new RestClientOptions(baseUrl)
        {
            CookieContainer = _cookieContainer,
            Timeout         = TimeSpan.FromSeconds(15) // 15s 超时
        });
    }

    /// <summary>
    /// 通用 GET 请求，确保状态刷新
    /// </summary>
    public async Task<(HttpStatusCode, string)> GetAsync(string subPath)
    {
        await EnsureLoggedInAsync();

        var request = new RestRequest(subPath)
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        try
        {
            var response = await ExecuteWithRetryAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用 GET 请求（同步）
    /// </summary>
    public (HttpStatusCode, string) Get(string subPath)
    {
        EnsureLoggedIn();

        var request = new RestRequest(subPath)
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        try
        {
            var response = ExecuteWithRetry(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Get: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 处理 HTTP 响应
    /// </summary>
    private static (HttpStatusCode, string) HandleResponse(RestResponseBase response)
    {
        if (response.StatusCode == HttpStatusCode.NotModified)
        {
            Console.WriteLine("状态未修改，强制重新请求...");
            return (HttpStatusCode.NotModified, "Not Modified");
        }

        if (response.IsSuccessful)
        {
            return (HttpStatusCode.OK, response.Content ?? string.Empty);
        }

        return (response.StatusCode, response.ErrorMessage ?? "Unknown error");
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
    /// 通用的同步 POST 请求
    /// </summary>
    public (HttpStatusCode, string) Post(string subPath, Dictionary<string, string> parameters)
    {
        EnsureLoggedIn();

        var request = new RestRequest(subPath, Method.Post)
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        try
        {
            var response = ExecuteWithRetry(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Post: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用的异步 POST 请求
    /// </summary>
    public async Task<(HttpStatusCode, string)> PostAsync(string subPath, Dictionary<string, string> parameters)
    {
        await EnsureLoggedInAsync();

        var request = new RestRequest(subPath, Method.Post)
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        try
        {
            var response = await ExecuteWithRetryAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用的同步 POST 文件上传
    /// </summary>
    public (HttpStatusCode, string) PostWithFiles(string       subPath, Dictionary<string, string> parameters,
                                                  List<string> filePaths)
    {
        EnsureLoggedIn();

        var request = new RestRequest(subPath, Method.Post)
                      {
                          CookieContainer         = _cookieContainer,
                          AlwaysMultipartFormData = true
                      }
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        // 添加表单参数
        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        // 添加文件
        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath))
            {
                request.AddFile("torrents", filePath, "application/x-bittorrent");
            }
            else
            {
                Console.WriteLine($"文件未找到: {filePath}");
                return (HttpStatusCode.BadRequest, $"File not found: {filePath}");
            }
        }

        try
        {
            var response = ExecuteWithRetry(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostWithFiles: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用的异步 POST 文件上传
    /// </summary>
    public async Task<(HttpStatusCode, string)> PostWithFilesAsync(string                     subPath,
                                                                   Dictionary<string, string> parameters,
                                                                   List<string>               filePaths)
    {
        await EnsureLoggedInAsync();

        var request = new RestRequest(subPath, Method.Post)
                      {
                          CookieContainer         = _cookieContainer,
                          AlwaysMultipartFormData = true
                      }
                     .AddHeader("Cache-Control", "no-cache")
                     .AddHeader("Pragma", "no-cache")
                     .AddHeader("Connection", "close");

        // 添加表单参数
        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        // 添加文件
        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath))
            {
                request.AddFile("torrents", filePath, "application/x-bittorrent");
            }
            else
            {
                Console.WriteLine($"文件未找到: {filePath}");
                return (HttpStatusCode.BadRequest, $"File not found: {filePath}");
            }
        }

        try
        {
            var response = await ExecuteWithRetryAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostWithFilesAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }


    /// <summary>
    /// 登录（异步）
    /// </summary>
    private async Task LoginAsync()
    {
        var request = new RestRequest("api/v2/auth/login", Method.Post);
        request.AddParameter("username", _userName);
        request.AddParameter("password", _password);

        try
        {
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                ExtractSidFromResponse(response);
            }
            else
            {
                throw new Exception($"Login failed: {response.StatusCode} - {response.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in LoginAsync: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// 登录（同步）
    /// </summary>
    private void Login()
    {
        var request = new RestRequest("api/v2/auth/login", Method.Post);
        request.AddParameter("username", _userName);
        request.AddParameter("password", _password);

        try
        {
            var response = _client.Execute(request);
            if (response.IsSuccessful)
            {
                ExtractSidFromResponse(response);
            }
            else
            {
                throw new Exception($"Login failed: {response.StatusCode} - {response.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Login: {ex.Message}");
            throw;
        }
    }

    private void ExtractSidFromResponse(RestResponseBase response)
    {
        lock (_lock)
        {
            _cookieContainer = new CookieContainer();

            var setCookieHeader = response.Headers?
               .FirstOrDefault(h => h.Name.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase));

            if (setCookieHeader?.Value is not { } cookieValue) return;
            var sid = ExtractSid(cookieValue);
            _cookieContainer.Add(new Uri(_url), new Cookie("SID", sid));
        }
    }

    private static string ExtractSid(string cookie)
    {
        var parts = cookie.Split(';').FirstOrDefault(p => p.StartsWith("SID="));
        return parts?[4..] ?? throw new Exception("SID not found.");
    }

    /// <summary>
    /// 增加自动重试逻辑（同步）
    /// </summary>
    private RestResponse ExecuteWithRetry(RestRequest request, int maxRetries = 3)
    {
        var retries = 0;
        while (true)
        {
            try
            {
                var response = _client.Execute(request);
                if (response.StatusCode != HttpStatusCode.InternalServerError || retries >= maxRetries) return response;
                Thread.Sleep(500 * retries); // 指数退避
                retries++;
            }
            catch (Exception ex)
            {
                if (retries >= maxRetries)
                    throw;

                Console.WriteLine($"重试中 ({retries + 1}/{maxRetries}): {ex.Message}");
                Thread.Sleep(500 * retries);
                retries++;
            }
        }
    }

    /// <summary>
    /// 增加自动重试逻辑（异步）
    /// </summary>
    private async Task<RestResponse> ExecuteWithRetryAsync(RestRequest request, int maxRetries = 3)
    {
        var retries = 0;
        while (true)
        {
            try
            {
                var response = await _client.ExecuteAsync(request);
                if (response.StatusCode != HttpStatusCode.InternalServerError || retries >= maxRetries) return response;
                await Task.Delay(500 * retries); // 指数退避
                retries++;
            }
            catch (Exception ex)
            {
                if (retries >= maxRetries)
                    throw;

                Console.WriteLine($"重试中 ({retries + 1}/{maxRetries}): {ex.Message}");
                await Task.Delay(500 * retries);
                retries++;
            }
        }
    }
}
