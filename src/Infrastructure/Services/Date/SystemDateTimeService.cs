using Synaplic.Inventory.Application.Interfaces.Services;
using System;

namespace Synaplic.Inventory.Infrastructure.Services.Date
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}