namespace Zenbot.WebUI.Helpers
{
    public static class DiscordOAuthHelper
    {
        public static string GetLoginUrl(string loginUrlFormat, string clientId, string redirectUrl)
        {
            return loginUrlFormat
                .Replace("{clientId}", clientId)
                .Replace("{redirectUrl}", redirectUrl);
        }
    }
}
