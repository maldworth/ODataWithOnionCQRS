using ODataWithOnionCQRS.Core.Data;
using ODataWithOnionCQRS.Core.Extensions;
using ODataWithOnionCQRS.Data;

namespace ODataWithOnionCQRS.Bootstrapper
{
    public static class DbContextScopeExtensionConfig
    {
        public static void Setup()
        {
            DbContextScopeExtensions.GetDbContextFromCollection = (collection, type) =>
            {
                if(type == typeof(ISchoolDbContext))
                    return collection.Get<SchoolDbContext>();
                return null;
            };

            DbContextScopeExtensions.GetDbContextFromLocator = (locator, type) =>
            {
                if (type == typeof(ISchoolDbContext))
                    return locator.Get<SchoolDbContext>();
                return null;

            };
        }
    }
}
