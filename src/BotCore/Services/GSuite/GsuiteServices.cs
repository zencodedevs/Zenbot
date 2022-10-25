using BotCore.Entities;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

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


        //Scopes for api requests
        static string[] scopes = { DirectoryService.Scope.AdminDirectoryUser };

        public async Task<string> CreateGSuiteAccount(User gSuite, string auth)
        {

            try
            {
                var token = "";
                // Here this credential shoul be added from Google Api console app
                UserCredential credential;
                using (var stream = new FileStream(auth, FileMode.Open, FileAccess.Read))
                {
                    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    credPath = Path.Combine(credPath, ".credentials/", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                    // Requesting Authentication or loading previously stored authentication for userName
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                            scopes,
                                                                            "user",
                                                                            CancellationToken.None,
                                                                            new FileDataStore(credPath, true)).Result;

                    token = await credential.GetAccessTokenForRequestAsync();

                }

                var service = new DirectoryService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "ZenbotDesktop",
                });
              var status = service.Users.Insert(gSuite).ExecuteAsync().Status;

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }
        
    }
   
}

