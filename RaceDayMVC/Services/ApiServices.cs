using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;

namespace RaceDayMVC.Services
{
    public class ApiService
    {
        public async Task<byte[]?> GetImageBytesAsync(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            var response = await _httpClient.GetAsync(url.TrimStart('/'));
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsByteArrayAsync();
        }

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("https://localhost:7270/");
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AddSessionCookieToRequest();
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            AddSessionCookieToRequest();
            var settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var json = JsonConvert.SerializeObject(data, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> EnrolInEventAsync(int eventId, int categoryId)
        {
            AddSessionCookieToRequest();
            var payload = new { CategoryId = categoryId };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync($"api/events/{eventId}/enrol", content);
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            AddSessionCookieToRequest();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var json = JsonConvert.SerializeObject(data, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PutAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            AddSessionCookieToRequest();
            return await _httpClient.DeleteAsync(endpoint);
        }
        public async Task<HttpResponseMessage> PostFormDataAsync(string endpoint, MultipartFormDataContent formData)
        {
            AddSessionCookieToRequest();
            return await _httpClient.PostAsync(endpoint, formData);
        }

        public async Task<HttpResponseMessage> PostFileAsync(string endpoint, MultipartFormDataContent content)
        {
            AddSessionCookieToRequest();
            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<byte[]?> GetBytesAsync(string endpoint)
        {
            AddSessionCookieToRequest();
            var cleanEndpoint = endpoint.TrimStart('/');
            var response = await _httpClient.GetAsync(cleanEndpoint);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsByteArrayAsync();
        }

        private void AddSessionCookieToRequest()
        {
            var sessionCookie = _httpContextAccessor.HttpContext?.Session.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(sessionCookie))
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Cookie", $".AspNetCore.Session={sessionCookie}");
            }
            else
            {
                var rawCookie = _httpContextAccessor.HttpContext?.Request.Cookies[".AspNetCore.Session"];
                if (!string.IsNullOrEmpty(rawCookie))
                {
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("Cookie", $".AspNetCore.Session={rawCookie}");
                }
            }
        }
    }
}"// API Service for MVC" 
