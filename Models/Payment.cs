using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSelfEmployApi.Models
{
    public class Payment : MongoDbDocument
    {
        public string JobId { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        #region NotMapped
        [NotMapped]
        private PaymentDto _PaymentDto { get; set; }

        [NotMapped]
        public PaymentDto PaymentDto
        {
            get
            {
                if (_PaymentDto == null)
                {
                    _PaymentDto = new PaymentDto()
                    {
                        Id = Id.ToString(),
                        JobId = JobId,
                        Memo = Memo,
                        Amount = Amount,
                        Date = Date,
                    };
                }

                return _PaymentDto;
            }
        }
        #endregion
    }
}