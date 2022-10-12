namespace Zenbot.WebUI.DiscordOAuth
{
    public static class DiscordDefaults
    {
        public const string AuthenticationScheme = "Discord";
        public const string DisplayName = "Discord";

        private static readonly string DiscordBaseUrl = "https://discord.com/";
        public static readonly string AuthorizationEndpoint = DiscordBaseUrl + "api/oauth2/authorize";
        public static readonly string TokenEndpoint = DiscordBaseUrl + "api/oauth2/token";
        public static readonly string UserInformationEndpoint = DiscordBaseUrl + "api/users/@me";
    }
}
