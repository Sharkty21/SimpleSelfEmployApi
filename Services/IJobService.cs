﻿using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Services
{
    public interface IJobService
    {
        Task<IEnumerable<JobDto>> Index(int? pageNumber, int limit);
        Task<JobDto> SaveJob(JobDto job);
        Task<JobDto> GetJob(string id);
        Task<bool> DeleteJob(string id);
    }
}
