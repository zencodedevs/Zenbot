using BotCore.Entities;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotCore.Services
{
    public class GsuiteServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGoogleAuthProvider _auth;

        public GsuiteServices(IHttpClientFactory httpfClient, IGoogleAuthProvider auth)
        {
            _httpClientFactory = httpfClient;
            _auth = auth;
        }

        static string[] scopes = { DirectoryService.Scope.AdminDirectoryUser };

        static string ApplicationName = "Zenbot";

        public async Task<string> CreateGSuiteAccount(GSuiteAccount gSuite)
        {
            string result = "";
            try
            {

                using (HttpClient httpClient = _httpClientFactory.CreateClient())
                {

                    GoogleCredential cred = await _auth.GetCredentialAsync();

                    var service = new DriveService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = cred,

                    });

                    var responseMessage = await httpClient.PostAsJsonAsync("https://admin.googleapis.com/admin/directory/v1/users", gSuite);

                    var responseResult = responseMessage.Content.ReadAsStringAsync().Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result = $"Account created for {gSuite.Name.GivenName} Successfully!";
                    }
                    else if (responseResult != null || responseResult != "")
                    {
                        result = responseResult;
                    }
                    else
                    {
                        result = $"Account Creation Faild";
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

    public class GSuiteAccount
    {
        public Name Name { get; set; }
        public string Password { get; set; }
        public string PrimaryEmail { get; set; }
    }
    public class Name
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
    }



    //static string ApplicationName = "Zenbot";
    //public async static Task CreateGSuiteAccount(string name, string family, string email, string password, string phone)
    //{
    //    UserCredential userCredential;

    //    using (var stream =
    //        new FileStream("credential.json", FileMode.Open, FileAccess.Read))
    //    {
    //        string credPath = "suiToken.json";
    //        userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
    //            scopes, "user", CancellationToken.None,
    //            new FileDataStore(credPath, true)).Result;
    //    }

    //    var services = new DirectoryService(new BaseClientService.Initializer()
    //    {
    //        HttpClientInitializer = userCredential,
    //        ApplicationName = ApplicationName
    //    });

    //    User newUserbody = new User();
    //    UserName newUsername = new UserName();

    //    newUsername.GivenName = name;
    //    newUsername.FamilyName = family;

    //    newUserbody.PrimaryEmail = email;
    //    newUserbody.Name = newUsername;
    //    newUserbody.Password = password;


    //    services.Users.Insert(newUserbody);
    //}
}

