namespace HazGo.BuildingBlocks.Core.Domain
{
    using System.Data;

    public interface IDBConnectionFactory
    {
        IDbConnection GetOpenConnection();

        IDbConnection GetOpenConnection(string connectionString);
    }
}
