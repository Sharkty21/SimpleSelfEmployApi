namespace SimpleSelfEmployApi.Dtos
{
    public class JobDto : IDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string CustomerName { get; set; }
    }
}
