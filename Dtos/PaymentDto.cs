namespace SimpleSelfEmployApi.Dtos
{
    public class PaymentDto : IDto
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
