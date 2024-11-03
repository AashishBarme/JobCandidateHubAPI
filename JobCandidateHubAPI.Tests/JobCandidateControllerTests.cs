using JobCandidateHubAPI.Application.Interfaces;
using JobCandidateHubAPI.Controllers;
using JobCandidateHubAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
namespace JobCandidateHubAPI.Tests
{
    public class JobCandidateControllerTests
    {
        private readonly Mock<IJobCandidateService> _mockService;
        private readonly JobCandidateController _controller;
        private readonly Mock<IMemoryCache> _cache;

        public JobCandidateControllerTests()
        {
            _mockService = new Mock<IJobCandidateService>();
            _cache = new Mock<IMemoryCache>();
            _controller = new JobCandidateController(_mockService.Object, _cache.Object);
        }

        [Fact]
        public async Task Index_ReturnsBadRequest_WhenDtoIsNull()
        {
            var result = await _controller.Index(null);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var dto = new JobCandidateDto();
            _controller.ModelState.AddModelError("Email", "Email is required");
            var result = await _controller.Index(dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsOk_WhenCandidateIsUpdated()
        {
            var dto = new JobCandidateDto { Email = "test@example.com" };
            var candidateId = 1;
            _cache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            _mockService.Setup(repo => repo.GetCandidateIdByEmail(dto.Email)).ReturnsAsync(candidateId); // Candidate exists
            _mockService.Setup(repo => repo.Update(dto, candidateId)).ReturnsAsync(1);
            var result = await _controller.Index(dto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with email: {dto.Email} is updated successfully", okResult.Value);
            // Verify that Update was called once and Create was never called
            _mockService.Verify(s => s.Update(dto, candidateId), Times.Once);
            _mockService.Verify(s => s.Create(It.IsAny<JobCandidateDto>()), Times.Never);
        }

        [Fact]
        public async Task Index_ReturnsOk_WhenCandidateIsCreated()
        {
            var dto = new JobCandidateDto { Email = "newuser@example.com" };

            _mockService.Setup(repo => repo.GetCandidateIdByEmail(dto.Email)).ReturnsAsync(0); // Candidate does not exist
            _mockService.Setup(repo => repo.Create(dto)).ReturnsAsync(1);
            _cache.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
            var result = await _controller.Index(dto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"User with email: {dto.Email} is created successfully", okResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var dto = new JobCandidateDto { Email = "exception@example.com" };
            _mockService.Setup(repo => repo.GetCandidateIdByEmail(dto.Email)).ThrowsAsync(new Exception("Database error"));
            var result = await _controller.Index(dto);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred: Database error", badRequestResult.Value);
        }
    }
}
