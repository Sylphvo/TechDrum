using Invedia.DI.Attributes;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.ActionLog;
using TechDrum.Repository.Data;
using Serilog;

namespace TechDrum.Repository
{
    [ScopedDependency(ServiceType = typeof(IActionLogRepository))]
    public class ActionLogRepository : Repository<ActionLogEntity>, IActionLogRepository
    {
        private readonly ILogger _logger;
        public ActionLogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _logger = Log.Logger;
        }
    }
}
