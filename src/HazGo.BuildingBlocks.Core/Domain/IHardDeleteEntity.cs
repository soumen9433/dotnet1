namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface IHardDeleteEntity
    {
        public bool IsPermanentDelete { get; set; }
    }
}
