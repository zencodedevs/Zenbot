using BotCore.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.ScrinIO
{
    public class ScrinIOService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _service;
        private readonly BotConfiguration _config;
        public ScrinIOService(IHttpClientFactory httpfClient, IServiceProvider service)
        {
            _httpClientFactory = httpfClient;
            _service = service;
            _config = service.GetRequiredService<BotConfiguration>();
        }

        public async Task<string> InviteUser(ScrinEmail email)
        {
            string result = "";
            try
            {
                using (HttpClient httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.Add(_config.ScrinIO.HeaderName, _config.ScrinIO.Token);

                    var responseMessage = await httpClient.PostAsJsonAsync("https://screenshotmonitor.com/api/v2/InviteEmployee", email);
                    var responseResult = responseMessage.Content.ReadAsStringAsync().Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = $"Invitation sent to {email.Email} Successfully!";
                    }
                    else if (responseResult != null || responseResult != "")
                    {
                        result = responseResult;
                    }
                    else
                    {
                        result = $"Invitation Faild";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

    }

    public static class Extention
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);

            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }
    }

    public class ScrinEmail
    {
        public string Email { get; set; }
    }
}
