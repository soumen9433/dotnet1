using System;
using System.Collections.Generic;
using System.Text;

namespace HazGo.BuildingBlocks.Persistence.EF.SearchRepository
{
    public class SearchResult<T>
    {
        public virtual IReadOnlyList<T> Values { get; }

        public virtual long? TotalCount { get; }

        public virtual string? ContinuationToken { get; }

        public SearchResult(IReadOnlyList<T> values, long? totalCount, string? continuationToken = null)
        {
            Values = values;
            TotalCount = totalCount;
            if (continuationToken != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(continuationToken);
                ContinuationToken = Convert.ToBase64String(bytes);
            }
        }
    }
}
