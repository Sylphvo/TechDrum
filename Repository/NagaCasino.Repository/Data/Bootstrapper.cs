using Invedia.DI.Attributes;
using Microsoft.EntityFrameworkCore;
using TechDrum.Contract.Repository.Data;
using System.Threading;
using System.Threading.Tasks;

namespace TechDrum.Repository.Data
{
    [ScopedDependency(ServiceType = typeof(IBootstrapper))]
    public class Bootstrapper : IBootstrapper
    {
        private readonly AppDbContext _dbContext;

        public Bootstrapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task InitialAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task RebuildAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
