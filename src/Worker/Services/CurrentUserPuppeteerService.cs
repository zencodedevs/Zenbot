namespace Worker.Services
{
    using ZenAchitecture.Domain.Shared.Common;
    using ZenAchitecture.Domain.Shared.Interfaces;

    public class CurrentUserPuppeteerService : ICurrentUserService
    {
        public string UserId => Constants.ServiceWorkerUser.Id;

        public string UserName => Constants.ServiceWorkerUser.Name;

        public string FacilitatorId => Constants.ServiceWorkerUser.FacilitatorId;

        public string UserMerchants => Constants.ServiceWorkerUser.Merchants;
    }
}
