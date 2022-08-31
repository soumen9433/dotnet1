namespace HazGo.BuildingBlocks.Persistence.EF
{
    using System;
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;

    public abstract class EFDataContext<TDbContext> : DbContext, IDataContext
        where TDbContext : DbContext
    {
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
