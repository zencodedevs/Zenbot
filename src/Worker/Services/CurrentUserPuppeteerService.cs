namespace Worker.Services
{
    using Zenbot.Domain.Shared.Common;
    using Zenbot.Domain.Shared.Interfaces;

    public class CurrentUserPuppeteerService : ICurrentUserService
    {
        public string UserId => Constants.ServiceWorkerUser.Id;

        public string UserName => Constants.ServiceWorkerUser.Name;

        public string FacilitatorId => Constants.ServiceWorkerUser.FacilitatorId;

        public string UserMerchants => Constants.ServiceWorkerUser.Merchants;
    }
}
