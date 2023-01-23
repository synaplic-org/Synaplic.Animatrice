using Microsoft.Extensions.Options;
using Synaplic.Inventory.Infrastructure.Sage.Configurations;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using Dapper;
using System.Data.SqlClient;
using Synaplic.Inventory.Infrastructure.Sage.Manager;

namespace Synaplic.Inventory.Infrastructure.Sage.Repositories
{
    public class SageRepository: IDisposable
    {
        private readonly SageConfiguration _config;


   

        private bool disposedValue;
        private IDbTransaction transaction;

        public IDbConnection db { get; private set; }
        public ISageQueryManager QueryManager { get; private set; }

        public SageRepository(IOptions<SageConfiguration> config,ISageQueryManager queryManager)
        {
            _config = config.Value;
            QueryManager = queryManager;
            QueryManager.Version = _config.Version;
            db = new SqlConnection(_config.SageConnection);


        }

        //public async Task<T> QuerySingleOrDefaultAsync<T>( string QueryName, object param = null)
        //{
        //    return await Db.QuerySingleOrDefaultAsync<T>(QueryService.GetQuery(QueryName), param);
        //}
        public async Task<T> QueryFirstOrDefaultAsync<T>(string QueryName, object param = null)
        {
            return await db.QueryFirstOrDefaultAsync<T>(QueryManager.GetQuery(QueryName), param, transaction);
        }

        public async Task<T> ExecuteScalarAsync<T>(string QueryName, object param = null)
        {
            return await db.ExecuteScalarAsync<T>(QueryManager.GetQuery(QueryName), param, transaction);
        }


        public async Task<IEnumerable<T>> QueryAsync<T>(string QueryName, object param = null)
        {
            return await db.QueryAsync<T>(QueryManager.GetQuery(QueryName), param, transaction);
        }
        public async Task<int> ExecuteAsync(string QueryName, object param = null)
        {
            return await db.ExecuteAsync(QueryManager.GetQuery(QueryName), param, transaction);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SageRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal IDbTransaction BeginTransaction()
        {
            if (transaction == null)
            {
                if (db.State == ConnectionState.Closed || db.State == ConnectionState.Broken) db.Open();
                transaction = db.BeginTransaction();
            }
            return transaction;

        }
    }
}