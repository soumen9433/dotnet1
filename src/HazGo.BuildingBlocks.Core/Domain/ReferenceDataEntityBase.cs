namespace HazGo.BuildingBlocks.Core.Domain
{
    public abstract class ReferenceDataEntityBase<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        protected ReferenceDataEntityBase()
        {
        }

        protected ReferenceDataEntityBase(TPrimaryKey id)
           : base(id)
        {
        }
    }
}
