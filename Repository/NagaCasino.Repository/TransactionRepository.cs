using Invedia.DI.Attributes;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.Transaction;
using TechDrum.Repository.Data;
using Serilog;

namespace TechDrum.Repository
{
    [ScopedDependency(ServiceType = typeof(ITransactionRepository))]
    public class TransactionRepository:Repository<TransactionEnity>, ITransactionRepository
    {
        private readonly ILogger _logger;
        public TransactionRepository(AppDbContext dbContext) :base(dbContext)
        {
            _logger = Log.Logger;
        }       
    }
}
