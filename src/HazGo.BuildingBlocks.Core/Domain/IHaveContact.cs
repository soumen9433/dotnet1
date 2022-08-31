namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface IHaveContact
    {
        public string ContactType { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
    }
}
