using ODataWithOnionCQRS.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.OData;

namespace ODataWithOnionCQRS.MyODataApi.Helpers
{
    public static class ODataBoilerplate
    {
        public static async Task<TEntity> UpdateEntity<TEntity>(ISchoolDbContext dbCtx, Delta<TEntity> patch, object key)
            where TEntity : class
        {
            TEntity currentModel = await dbCtx.Set<TEntity>().FindAsync(key);
            if (currentModel == null)
            {
                throw new InvalidOperationException("Not Found");
            }

            patch.Patch(currentModel);

            // We know that we want to explicitly save changes, so we cast here
            await ((DbContext)dbCtx).SaveChangesAsync();

            return currentModel;
        }

        public static async Task CreateEntity<TEntity>(ISchoolDbContext dbCtx, TEntity entity)
            where TEntity : class
        {
            dbCtx.Set<TEntity>().Add(entity);

            // We know that we want to explicitly save changes, so we cast here
            await ((DbContext)dbCtx).SaveChangesAsync();
        }

        public static async Task DeleteEntity<TEntity>(ISchoolDbContext dbCtx, object key)
            where TEntity : class
        {
            TEntity currentModelToDelete = await dbCtx.Set<TEntity>().FindAsync(key);
            if (currentModelToDelete == null)
            {
                throw new InvalidOperationException("Not Found");
            }

            dbCtx.Set<TEntity>().Remove(currentModelToDelete);

            // We know that we want to explicitly save changes, so we cast here
            await ((DbContext)dbCtx).SaveChangesAsync();
        }
    }
}