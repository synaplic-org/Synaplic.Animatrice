using Microsoft.Extensions.Localization;
using Synaplic.Inventory.Infrastructure.Contexts;
using Synaplic.Inventory.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synaplic.Inventory.Infrastructure.Services.v1
{
    public interface ISyncService
    {
        bool ExecuteJobNow();

        bool RemoveRecurentJob();

        bool StartRecurentJob();

        //Task SyncAll();
        
    }

    public class SyncService : ISyncService
    {
        private readonly UniContext _db;
        

        public SyncService(UniContext db )
        {
            _db = db;
           
        }

        //[DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        //[AutomaticRetry(Attempts = 1)]
        //public async Task SyncAll()
        //{
        //    await SyncOdataTasks();
        //    await SyncLogisticsAreas();
        //    await SyncSoapTasks();
        //}

 

        public bool StartRecurentJob()
        {
            try
            {
                //Hangfire.RecurringJob.RemoveIfExists(nameof(SyncService) + "." + nameof(SyncAll));
                // Hangfire.RecurringJob.AddOrUpdate<ISyncService>(x => x.SyncAll(), "*/15 * * * *");
                //_ =ExecuteJobNow();
                //SyncAll().GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("StartRecurentJob failed!", ex);
            }
        }

        public bool RemoveRecurentJob()
        {
            try
            {
                //Hangfire.RecurringJob.RemoveIfExists(nameof(SyncService) + "." + nameof(SyncAll));
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("RemoveRecurentJob  failed!", ex);
            }
        }

        public bool ExecuteJobNow()
        {
            try
            {
                // Hangfire.BackgroundJob.Enqueue<ISyncService>(x => x.SyncAll());
               // SyncAll().GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Executing Job Now failed!", ex);
            }
        }

        public async Task GlobalSyncronization()
        {
            await Task.Delay(1);
        }

        
    }
}