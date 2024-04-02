using SimpleSelfEmploy.Models.Mongo;
using SimpleSelfEmployApi.Dtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSelfEmployApi.Models
{
    public class Job : MongoDbDocument
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string CustomerName { get; set; }

        #region NotMapped
        [NotMapped]
        private JobDto _jobDto { get; set; }

        [NotMapped]
        public JobDto JobDto
        {
            get
            {
                if (_jobDto == null)
                {
                    _jobDto = new JobDto()
                    {
                        Id = Id.ToString(),
                        Name = Name,
                        Description = Description,
                        StartDate = StartDate,
                        CustomerName = CustomerName,
                    };
                }

                return _jobDto;
            }
        }
        #endregion
    }
}