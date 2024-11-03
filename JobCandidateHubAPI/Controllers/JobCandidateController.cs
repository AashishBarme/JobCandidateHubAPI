using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace JobCandidateHubAPI.Controllers
{
    /// <summary>
    /// Controller for managing job candidate operations such as creating and updating candidate records.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class JobCandidateController : Controller
    {
        private readonly IJobCandidateService _service;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobCandidateController"/> class.
        /// </summary>
        /// <param name="service">The service interface for job candidate data operations.</param>
        ///  <param name="cache">The cache interface for caching.</param>
        public JobCandidateController(IJobCandidateService service, IMemoryCache cache)
        {
            _service =  service;
            _cache = cache;
        }

        /// <summary>
        /// Creates or updates a job candidate record.
        /// </summary>
        /// <param name="dto">The data transfer object containing job candidate information.</param>
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] JobCandidateDto dto)
        {
            // Validate the candidate data.
            if (dto == null)
                return BadRequest("Invalid candidate data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cacheOptions = new MemoryCacheEntryOptions()
                  .SetSlidingExpiration(TimeSpan.FromMinutes(60));

            try
            {
                long candidateId = await CheckIfCandidateAlreadyExists(dto.Email, cacheOptions);
                if (candidateId != 0)
                {
                    // If candidate exists, update the existing record.
                    await _service.Update(dto, candidateId);
                    return Ok($"User with email: {dto.Email} is updated successfully");
                }
                else
                {
                    // If candidate does not exist, create a new record.
                    candidateId = await _service.Create(dto);
                    string cacheKey = $"CandidateId_{dto.Email}";
                    _cache.Set(cacheKey, candidateId, cacheOptions);
                    return Ok($"User with email: {dto.Email} is created successfully");
                }
            }
            catch (Exception e)
            {
                // Handle unexpected errors.
                return BadRequest($"An error occurred: {e.Message}");
            }
        }

        private async Task<long> CheckIfCandidateAlreadyExists(string email, MemoryCacheEntryOptions cacheOptions)
        {
            string cacheKey = $"CandidateId_{email}";
            if (!_cache.TryGetValue(cacheKey, out long candidateId))
            {
                // If not found in cache, get from database
                candidateId = await _service.GetCandidateIdByEmail(email);
                if(candidateId > 0)
                {
                    _cache.Set(cacheKey, candidateId, cacheOptions);
                }
            }
            return candidateId;
        }
    }
}
