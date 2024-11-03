﻿using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Infrastructure.Entities;
using JobCandidateHubAPI.Infrastructure.Persistence;
using JobCandidateHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobCandidateHubAPI.Infrastructure.Repositories
{
    public class JobCandidateRepository : IJobCandidateRepository
    {
        public readonly AppDbContext _context;
        public JobCandidateRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<long> GetCandidateIdByEmail(string email)
        {
            return _context.JobCandidates.Where(x => x.Email == email).Select(y => y.Id).FirstAsync();
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


            _context.JobCandidates.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<long> Update(JobCandidateDto dto)
        {
            JobCandidate entity = new()
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                GithubProfileUrl = dto.GithubProfileUrl,
                LinkedinProfileUrl = dto.LinkedinProfileUrl,
                TimeInterval = dto.TimeInterval,
                Comment = dto.Comment
            };

            List<string> constantField = ["Id", "Email"];
            var entry = _context.JobCandidates.Attach(entity);
            foreach (var property in entry.OriginalValues.Properties)
            {
                if (!constantField.Contains(property.Name))
                {
                    entry.Property(property.Name).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}