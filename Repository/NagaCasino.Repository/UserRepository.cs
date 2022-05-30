using Invedia.DI.Attributes;
using TechDrum.Contract.Repository.Interface;
using TechDrum.Contract.Repository.Models.User;
using TechDrum.Repository.Data;
using Serilog;
namespace TechDrum.Repository
{
    [ScopedDependency(ServiceType =typeof(IUserRepository))]
    public class UserRepository:Repository<UserEntity>, IUserRepository 
    {
        private readonly ILogger _logger;
        public UserRepository(AppDbContext dbContext):base (dbContext)
        {
            _logger = Log.Logger;
        }
    }
}
