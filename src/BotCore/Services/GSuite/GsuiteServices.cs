using BotCore.Entities;
using Google;
using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.v201809;
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
            var result = "";
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

              var exx =  service.Users.Insert(gSuite).ExecuteAsync().Exception;
              
            }

            catch (GoogleApiException ex)
            {
                result = ex.Message;
            }

            return result;
        }

        // Not used yet
        public async Task<bool> ValidatePassword(string password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 15;

            if (password == null) throw new ArgumentNullException();

            bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
            bool hasUpperCaseLetter = false;
            bool hasLowerCaseLetter = false;
            bool hasDecimalDigit = false;

            if (meetsLengthRequirements)
            {
                foreach (char c in password)
                {
                    if (char.IsUpper(c)) hasUpperCaseLetter = true;
                    else if (char.IsLower(c)) hasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) hasDecimalDigit = true;
                }
            }

            bool isValid = meetsLengthRequirements
                        && hasUpperCaseLetter
                        && hasLowerCaseLetter
                        && hasDecimalDigit
                        ;
            return isValid;

        }

    }
   
}

