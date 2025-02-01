using RestSharp;
using System.Net;

namespace AnotherQbittorrent.Utils;

public class NetUtils
{
    private readonly RestClient       _client;
    private          CookieContainer? _cookieContainer;
    private readonly string           _userName;
    private readonly string           _password;
    private readonly string           _url;

    public NetUtils(string baseUrl, string userName, string password)
    {
        _url      = baseUrl;
        _userName = userName;
        _password = password;
        _client   = new RestClient(baseUrl);
    }


    /// <summary>
    /// 通用的异步 GET 请求
    /// </summary>
    public async Task<(HttpStatusCode, string)> GetAsync(string subPath)
    {
        await EnsureLoggedInAsync();

        var request = new RestRequest(subPath)
        {
            CookieContainer = _cookieContainer
        };

        try
        {
            var response = await _client.ExecuteAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in FetchAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用的同步 GET 请求
    /// </summary>
    public (HttpStatusCode, string) Get(string subPath)
    {
        EnsureLoggedIn();

        var request = new RestRequest(subPath)
        {
            CookieContainer = _cookieContainer
        };

        try
        {
            var response = _client.Execute(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Get: {ex.Message}");
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
        {
            CookieContainer = _cookieContainer
        };
        request.AddHeader("Content-Type", "multipart/form-data");

        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        try
        {
            var response = await _client.ExecuteAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// 通用的同步 POST 请求
    /// </summary>
    public (HttpStatusCode, string) Post(string subPath, Dictionary<string, string> parameters)
    {
        EnsureLoggedIn();

        var request = new RestRequest(subPath, Method.Post)
        {
            CookieContainer = _cookieContainer
        };
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        try
        {
            var response = _client.Execute(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Post: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    public async Task<(HttpStatusCode, string)> PostWithFilesAsync(string                     subPath,
                                                                   Dictionary<string, string> parameters,
                                                                   List<string>?              filePaths)
    {
        await EnsureLoggedInAsync(); // 确保已登录

        var request = new RestRequest(subPath, Method.Post)
        {
            CookieContainer         = _cookieContainer,
            AlwaysMultipartFormData = true
        };

        // 添加表单参数
        foreach (var param in parameters)
        {
            request.AddParameter(param.Key, param.Value);
        }

        // 添加文件
        if (filePaths != null)
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
            var response = await _client.ExecuteAsync(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostWithFileAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }

    public (HttpStatusCode, string) PostWithFiles(string                     subPath,
                                                  Dictionary<string, string> parameters,
                                                  List<string>               filePaths)
    {
        EnsureLoggedIn(); // 确保已登录

        var request = new RestRequest(subPath, Method.Post)
        {
            CookieContainer         = _cookieContainer,
            AlwaysMultipartFormData = true
        };

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
            var response = _client.Execute(request);
            return HandleResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PostWithFileAsync: {ex.Message}");
            return (HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
        }
    }


    /// <summary>
    /// 自动登录（异步）
    /// </summary>
    private async Task EnsureLoggedInAsync()
    {
        if (_cookieContainer == null)
        {
            await LoginAsync();
            return;
        }

        if (_cookieContainer.GetCookies(new Uri(_url)).Count > 0) return;

        await LoginAsync();
    }

    /// <summary>
    /// 自动登录（同步）
    /// </summary>
    private void EnsureLoggedIn()
    {
        if (_cookieContainer == null)
        {
            Login();
            return;
        }

        if (_cookieContainer.GetCookies(new Uri(_url)).Count > 0) return;

        Login();
    }

    /// <summary>
    /// 处理 HTTP 响应
    /// </summary>
    private static (HttpStatusCode, string) HandleResponse(RestResponseBase response)
    {
        if (response.IsSuccessful)
        {
            return (HttpStatusCode.OK, response.Content ?? string.Empty);
        }

        return (response.StatusCode, response.ErrorMessage ?? "Unknown error");
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
        _cookieContainer = new CookieContainer();

        var setCookieHeader = response.Headers?
           .FirstOrDefault(h => h.Name.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase));

        if (setCookieHeader?.Value is not { } cookieValue) return;
        var sid = ExtractSid(cookieValue);
        _cookieContainer.Add(new Uri(_url), new Cookie("SID", sid));
    }

    private static string ExtractSid(string cookie)
    {
        var parts = cookie.Split(';').FirstOrDefault(p => p.StartsWith("SID="));

        return parts?[4..] ?? throw new Exception("SID not found.");
    }
}
