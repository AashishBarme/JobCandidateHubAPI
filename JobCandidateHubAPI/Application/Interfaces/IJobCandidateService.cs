using JobCandidateHubAPI.Models;

namespace JobCandidateHubAPI.Application.Interfaces
{
    public interface IJobCandidateService
    {
        public Task<long> Create(JobCandidateDto dto);
        public Task<long> Update(JobCandidateDto dto);
        public Task<long> GetCandidateIdByEmail(string email);
    }
}
