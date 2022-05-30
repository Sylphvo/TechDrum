using Invedia.DI.Attributes;
using TechDrum.Contract.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace TechDrum.Repository.Data
{
    [ScopedDependency(ServiceType = typeof(IUnitOfWork))]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
