namespace Zenbot.Domain.Models.DiscordAuth.GetDiscordToken
{
    public class GetDiscordTokenRequest
    {
        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string grant_type
        {
            get
            {
                return "authorization_code";
            }
        }

        public string code { get; set; }

        public string redirect_uri { get; set; }
    }
}
