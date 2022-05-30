using Invedia.DI.Attributes;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.TransactionMail;
using TechDrum.Repository.Data;
using Serilog;

namespace TechDrum.Repository
{
    [ScopedDependency(ServiceType =typeof(ITransactionMailRepository))]
    public class TransactionMailRepository:Repository<TransactionMailEntity>, ITransactionMailRepository
    {
        private readonly ILogger _logger;
        public TransactionMailRepository(AppDbContext dbContext):base (dbContext)
        {
            _logger = Log.Logger;
        }
    }
}
