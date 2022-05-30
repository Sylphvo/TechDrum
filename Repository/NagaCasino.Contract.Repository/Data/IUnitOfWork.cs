using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TechDrum.Contract.Repository.Data
{
    public interface IUnitOfWork
    {
        [DebuggerStepThrough]
        int SaveChanges();

        [DebuggerStepThrough]
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
