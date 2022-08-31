namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface IDataContext
    {
        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
