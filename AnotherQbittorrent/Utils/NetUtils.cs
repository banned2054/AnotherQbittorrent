using System.Net;
using AnotherQbittorrent.Models.Enums;
using RestSharp;

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
        Login();
    }

    public async Task<(HttpStatusCode, string)> AsyncFetch(string subPath)
    {
        RestRequest request = new(subPath)
        {
            CookieContainer = _cookieContainer
        };

        try
        {
            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return (HttpStatusCode.OK, (response.Content ?? string.Empty));
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (HttpStatusCode.NotFound, string.Empty);
            }

            throw new Exception($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }

    public (HttpStatusCode, string) Fetch(string subPath)
    {
        RestRequest request = new(subPath)
        {
            CookieContainer = _cookieContainer
        };

        try
        {
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return (HttpStatusCode.OK, (response.Content ?? string.Empty));
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (HttpStatusCode.NotFound, string.Empty);
            }

            throw new Exception($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }

    private void Login()
    {
        var request = new RestRequest("api/v2/auth/login", Method.Post);
        request.AddHeader("Referer", _url);
        // 添加表单数据
        request.AddParameter("username", _userName);
        request.AddParameter("password", _password);
        _cookieContainer = new CookieContainer();

        try
        {
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                if (response.Headers == null) throw new Exception("Set-Cookie header not found.");
                var setCookieHeader = response.Headers
                                              .FirstOrDefault(h => h.Name.Equals("Set-Cookie",
                                                                       StringComparison.OrdinalIgnoreCase));

                if (setCookieHeader == null) throw new Exception("Set-Cookie header not found.");
                var cookieValue = setCookieHeader.Value;

                var sid = ExtractSid(cookieValue);

                _cookieContainer.Add(new Uri(_url), new Cookie("SID", sid));

                return;
            }

            throw new Exception($"Error: {response.StatusCode} - {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            throw;
        }
    }

    private static string ExtractSid(string cookie)
    {
        var parts = cookie.Split(';')
                          .Select(part => part.Trim())
                          .FirstOrDefault(part => part.StartsWith("SID="));

        if (parts != null)
        {
            return parts[4..]; // Skip "SID="
        }

        throw new Exception("SID not found in Set-Cookie header.");
    }
}