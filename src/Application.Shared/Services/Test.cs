using Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using ZenAchitecture.Domain.Shared.Entities.Geography;

namespace Application.Shared.Services
{
    public class Test : ITest
    {
        private readonly IEntityFrameworkRepository<City> _repository;
        public Test(IEntityFrameworkRepository<City> repository)
        {
            _repository = repository;
        }
        public async Task CreateNewCity()
        {
            try
            {
                var city = new City().Create("zen bot city");
                await _repository.InsertAsync(city);
                await _repository.SaveChangesAsync(true);
            }catch(Exception ex)
            {
                var d = ex;
            }
        }
    }
}
