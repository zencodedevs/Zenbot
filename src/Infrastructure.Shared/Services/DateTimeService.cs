using System;
using ZenAchitecture.Domain.Shared.Interfaces;

namespace ZenAchitecture.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
