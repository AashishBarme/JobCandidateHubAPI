using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Infrastructure.Entities;
using JobCandidateHubAPI.Infrastructure.Repositories;
using JobCandidateHubAPI.Models;

namespace JobCandidateHubAPI.Services
{
    public class JobCandidateService : IJobCandidateService
    {
        public readonly IJobCandidateRepository _repository;
        public JobCandidateService(IJobCandidateRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> Create(JobCandidateDto dto)
        {

            JobCandidate entity = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                GithubProfileUrl = dto.GithubProfileUrl,
                LinkedinProfileUrl = dto.LinkedinProfileUrl,
                TimeInterval = dto.TimeInterval,
                Comment = dto.Comment
            };

            return await _repository.Create(entity);
        }

        public async Task<long> GetCandidateIdByEmail(string email)
        {
            return await _repository.GetCandidateIdByEmail(email);
        }

        public async Task<long> Update(JobCandidateDto dto, long candidateId)
        {

            JobCandidate entity = new()
            {
                Id = candidateId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                GithubProfileUrl = dto.GithubProfileUrl,
                LinkedinProfileUrl = dto.LinkedinProfileUrl,
                TimeInterval = dto.TimeInterval,
                Comment = dto.Comment
            };

            return await _repository.Update(entity);
        }
    }
}
