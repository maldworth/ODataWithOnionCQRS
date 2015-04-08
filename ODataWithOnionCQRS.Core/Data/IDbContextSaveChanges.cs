using System.Threading;
using System.Threading.Tasks;

namespace ODataWithOnionCQRS.Core.Data
{
    /// <summary>
    /// Should only be used when the user knows they explicitly want to save the changes they made, and are not within a DbContextScope.
    /// Otherwise, users should use the DbContextScoope SaveChanges instead
    /// </summary>
    public interface IDbContextSaveChanges
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
