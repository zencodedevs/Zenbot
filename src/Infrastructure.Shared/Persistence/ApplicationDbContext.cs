using ZenAchitecture.Domain.Shared.Common;
using ZenAchitecture.Domain.Shared.Entities;
using ZenAchitecture.Domain.Shared.Entities.Geography;
using ZenAchitecture.Domain.Shared.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Zen.Domain.Entities.Attributes;
using Zen.Domain.Entities.Log;
using Zen.Domain.Entities.Payment;
using Zen.Domain.Events;
using Zen.Domain.Interfaces;
using Zen.EventProcessor;
using Zen.Infrastructure.Interfaces;

namespace ZenAchitecture.Infrastructure.Shared.Persistence
{

    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IAppDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;
        private readonly ICurrentUserService _currentUserService;


        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService, IDomainEventService domainEventService, IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }


        #region Zen Framework Suggested

        public DbSet<Log> Logs { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventArchive> EventArchives { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<Payment> Payments { get; set; }


        #endregion Zen Framework Suggested



        public DbSet<City> Cities { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        public override DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b => { });


            /// Entity type configurations  

        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>().Select(x => x.Entity.DomainEvents).Where(x => x != null)
                    .SelectMany(x => x).Where(domainEvent => !domainEvent.IsPublished).FirstOrDefault();

                if (domainEventEntity == null)
                    break;

                var processorAttributes = domainEventEntity.GetType().GetCustomAttributes(typeof(ProcessedByEventProcessorAttribute), true);

                var processorAttribute = (ProcessedByEventProcessorAttribute)processorAttributes.SingleOrDefault();

                if (processorAttribute != null)
                {
                    domainEventEntity.IsPublished = true;
                    continue;
                }

                domainEventEntity.IsPublished = true;

                await _domainEventService.PublishAsync(domainEventEntity);
            }
        }
    }
}
