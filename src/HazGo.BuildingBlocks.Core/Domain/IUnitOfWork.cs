using System.Threading;
using System.Threading.Tasks;

namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        Task<int> SQLQuery(string sql, params object[] parameters);
    }
}
