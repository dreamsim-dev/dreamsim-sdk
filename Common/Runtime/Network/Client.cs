using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dreamsim
{
public static class Client
{
    private static HttpClient _httpClient;

    private static HttpClient HttpClient
    {
        get
        {
            if (_httpClient != null) return _httpClient;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }

    public static async Task<HttpResponseMessage> SendGetAsync(string url)
    {
        HttpResponseMessage response;

        try
        {
            response = await HttpClient.GetAsync(url);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }

        return response;
    }

    public static async Task<Tuple<HttpResponseMessage, string>> SendPostAsync(string url, string contents)
    {
        var content = new StringContent(contents, Encoding.UTF8, "application/json");

        Tuple<HttpResponseMessage, string> result;
        try
        {
            result = await SendPostAsync(url, content);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }

        Debug.Log($"POST request sent\n\nContents:\n{contents}\n\nResponse:\n{result}\n");
        Debug.Log(result.Item1.IsSuccessStatusCode ? "POST request: Succeeded" : "POST request: Failed");

        return result;
    }

    private static async Task<Tuple<HttpResponseMessage, string>> SendPostAsync(string url, HttpContent content)
    {
        HttpResponseMessage response;
        string result;
        try
        {
            response = await HttpClient.PostAsync(url, content);
            result = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }

        return new Tuple<HttpResponseMessage, string>(response, result);
    }
}
}