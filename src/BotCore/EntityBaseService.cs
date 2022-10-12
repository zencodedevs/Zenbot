using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;
using Zen.Domain.Interfaces;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore
{
    public abstract class EntityBaseService<T> where T : class, IEntity
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        public EntityBaseService(IServiceProvider services, IServiceScopeFactory scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
        }
        public async Task<List<T>> GetManyAsync(Expression<Func<T, bool>> predicate)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<T>>();

                using (var uow = unitOfWorkManager.Begin())
                {
                    return await _repository.GetListAsync(predicate);
                }
            }
        }

       
        // Common method for getting the data from database
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] propertySelectors)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<T>>();

                using (var uow = unitOfWorkManager.Begin())
                {
                    return await _repository.FindAsync(predicate, default(CancellationToken), propertySelectors);
                }
            }
        }


        // Common method for inserting data into database
        public async Task<T> InsertAsync(T value)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<T>>();

                using (var uow = unitOfWorkManager.Begin())
                {
                    var result = await _repository.InsertAsync(value);

                    await _repository.SaveChangesAsync(true);

                    return result;
                }
            }
        }


        // Common method for updating the data into database
        public async Task<T> UpdateAsync(T obj, Action<T> value) => await UpdateAsync(obj.Id, value);
        public async Task<T> UpdateAsync(int id, Action<T> value)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<T>>();

                using (var uow = unitOfWorkManager.Begin())
                {
                    var channel = await _repository.FindAsync(x => x.Id == id);

                    value.Invoke(channel);

                    await _repository.UpdateAsync(channel);
                    await _repository.SaveChangesAsync(true);

                    return channel;
                }
            }
        }
    }
}
