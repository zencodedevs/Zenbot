using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotCore.Services.GSuite
{
    public class GsuiteServices
    {
        static string[] scopes = {DirectoryService.Scope.AdminDirectoryUser};
        static string ApplicationName = "Zenbot";
        public async Task CreateGSuiteAccount(string name, string family, string email, string password, string phone)
        {
            UserCredential userCredential;

            using (var stream = 
                new FileStream("credential.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    scopes, "user", CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var services = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = ApplicationName
            });

            User newUserbody = new User();
            UserName newUsername = new UserName();

            newUsername.GivenName = name;
            newUsername.FamilyName = family;

            newUserbody.PrimaryEmail = email;
            newUserbody.Name = newUsername;
            newUserbody.Password = password;


            services.Users.Insert(newUserbody);
        }
    }
}
