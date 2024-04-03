using AutoMapper;
using MongoDB.Bson;
using SimpleSelfEmployApi.Data;
using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Services
{
    public class JobService : IJobService
    {
        private readonly JobRepository _repository;
        private readonly IMapper _mapper;

        public JobService(JobRepository jobRepository, IMapper mapper)
        {
            _repository = jobRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDto>> Index(int? page, int limit = 10)
        {
            var query = await _repository.Index(page, limit);
            return query.ToList().Select(j => j.JobDto);
        }

        public async Task<JobDto> SaveJob(JobDto job)
        {
            Job jobModel = _mapper.Map<Job>(job);

            if (jobModel == null)
            {
                throw new Exception("Unable to map job dto.");
            }

            if (jobModel.Id == ObjectId.Empty)
            {
                await _repository.InsertOneAsync(jobModel);
                return jobModel.JobDto;
            }

            await _repository.ReplaceOneAsync(jobModel ?? new Job());
            return jobModel.JobDto;
        }

        public async Task<JobDto> GetJob(string id)
        {
            try
            {
                var job = await _repository.FindByIdAsync(id);
                return job.JobDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteJob(string id)
        {
            try
            {
                await _repository.DeleteByIdAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
