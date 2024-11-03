using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JobCandidateController"/> class.
        /// </summary>
        /// <param name="repo">The repository interface for job candidate data operations.</param>
        public JobCandidateController(IJobCandidateService service)
        {
            _service =  service;
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

            try
            {
                // Check if the candidate already exists based on the email.
                long candidateId = await _service.GetCandidateIdByEmail(dto.Email);

                if (candidateId != 0)
                {
                    // If candidate exists, update the existing record.
                    dto.Id = candidateId;
                    await _service.Update(dto);
                    return Ok($"User with email: {dto.Email} is updated successfully");
                }
                else
                {
                    // If candidate does not exist, create a new record.
                    await _service.Create(dto);
                    return Ok($"User with email: {dto.Email} is created successfully");
                }
            }
            catch (Exception e)
            {
                // Handle unexpected errors.
                return BadRequest($"An error occurred: {e.Message}");
            }
        }
    }
}
