namespace HazGo.BuildingBlocks.Persistence.EF.SearchRepository
{
    internal class RawFilterParts
    {
        public string Property { get; set; }
        public string Operation { get; set; }
        public string Value { get; set; }
        public string FilterOperator { get; set; }
    }
}
