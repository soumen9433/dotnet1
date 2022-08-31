using System;
using System.Text;

namespace HazGo.BuildingBlocks.Persistence.EF.SearchRepository
{
    public class SearchOptions<T>
    {
        public string Filter { get; set; }

        public string OrderBy { get; set; }

        public OrderByDirection OrderByDirection { get; set; }

        public int Skip { get; set; } = 0;


        public int Top { get; set; } = 1000;


        public string? SkipToken { get; set; }

        public string? GetContinuationToken()
        {
            if (SkipToken == null)
            {
                return null;
            }

            byte[] bytes = Convert.FromBase64String(SkipToken);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
