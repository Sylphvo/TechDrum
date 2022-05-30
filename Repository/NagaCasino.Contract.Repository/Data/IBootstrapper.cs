using System.Threading;
using System.Threading.Tasks;

namespace TechDrum.Contract.Repository.Data
{
    public interface IBootstrapper
    {
        Task InitialAsync(CancellationToken cancellationToken = default);

        Task RebuildAsync(CancellationToken cancellationToken = default);
    }
}
