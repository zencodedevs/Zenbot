namespace Zenbot.Services
{
    using ZenAchitecture.Domain.Shared.Common;
    using ZenAchitecture.Domain.Shared.Interfaces;

    public class CurrentUserPuppeteerService : ICurrentUserService
    {
        public string UserId => Constants.ZenBotUser.Id;

        public string UserName => Constants.ZenBotUser.Name;

        public string FacilitatorId => Constants.ZenBotUser.FacilitatorId;

        public string UserMerchants => Constants.ZenBotUser.Merchants;
    }
}
