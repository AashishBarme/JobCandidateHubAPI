using JobCandidateHubAPI.Infrastructure.Entities;
using JobCandidateHubAPI.Models;

namespace JobCandidateHubAPI.Application.Interfaces
{
    public interface IJobCandidateRepository
    {
        public Task<long> Create(JobCandidate entity);
        public Task<long> Update(JobCandidate entity);
        public Task<long> GetCandidateIdByEmail(string email);
    }
}
