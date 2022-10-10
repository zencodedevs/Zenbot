using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Zen.Bog.Ecommerce;
using Zenbot.Domain.Interfaces;
using Zenbot.Domain.Models.DiscordAuth.GetDiscordToken;

namespace Zenbot.Infrastructure.Services
{
    public class DiscordAuthService : IDiscordAuthService
    {
        private readonly ILogger<DiscordAuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DiscordAuthService(
            ILogger<DiscordAuthService> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        private void AddFormUrlencodedContentTypeHeader(HttpClient httpClient)
        {
            //httpClient.DefaultRequestHeaders.Accept.Clear();

            //httpClient.DefaultRequestHeaders.Accept
            //    .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            //httpClient.DefaultRequestHeaders.AcceptCharset.Clear();
            //httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(Encoding.UTF8.BodyName));
        }

        public async Task<GetDiscordTokenResponse> GetDiscordTokenAsync(GetDiscordTokenRequest request)
        {
            return default;

            //if (request is null)
            //    throw new ArgumentNullException("GetDiscordTokenAsync request param(s) is not valid!");

            //var discordBaseUrl = _configuration.GetSection("DiscordBase")["BaseUrl"];
            //var discordTokenUrl = _configuration.GetSection("DiscordAuth")["TokenUrl"];
            //var uri = discordBaseUrl + discordTokenUrl;

            //HttpClient httpClient = _httpClientFactory.CreateClient();
            //AddFormUrlencodedContentTypeHeader(httpClient);

            //var response = await httpClient.PostAsJsonAsync(uri, request);
            //if (!response.IsSuccessStatusCode)
            //{
            //    _logger.LogError("GetDiscordTokenAsync failed!");
            //    throw new Exception("GetDiscordTokenAsync failed!");
            //}

            //var result = await response.Content.ReadAsJsonAsync<GetDiscordTokenResponse>();
            //return result;
        }
    }
}
