using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Zenbot.WebUI.Controllers;
using Zenbot.WebUI.Helpers;

namespace Zenbot.WebUI.DiscordOAuth
{
    public class DiscordOptions : OAuthOptions
    {
        public DiscordOptions()
        {
            CallbackPath = new PathString("/signin-discord");
            AccessDeniedPath = nameof(ErrorController.Code403).GetActionRoute(nameof(ErrorController));
            AuthorizationEndpoint = DiscordDefaults.AuthorizationEndpoint;
            TokenEndpoint = DiscordDefaults.TokenEndpoint;
            UserInformationEndpoint = DiscordDefaults.UserInformationEndpoint;
            Scope.Add("identify");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id", ClaimValueTypes.UInteger64);
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username", ClaimValueTypes.String);
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
            ClaimActions.MapJsonKey("urn:discord:discriminator", "discriminator", ClaimValueTypes.UInteger32);
            ClaimActions.MapJsonKey("urn:discord:avatar", "avatar", ClaimValueTypes.String);
            ClaimActions.MapJsonKey("urn:discord:verified", "verified", ClaimValueTypes.Boolean);
        }

        public string AppId { get => ClientId; set => ClientId = value; }

        public string AppSecret { get => ClientSecret; set => ClientSecret = value; }
    }
}
