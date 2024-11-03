using System.ComponentModel.DataAnnotations;

namespace JobCandidateHubAPI.Models
{
    public class JobCandidateDto
    {
        public long Id { get; set; } = 0;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? TimeInterval { get; set; }
        public string? LinkedinProfileUrl { get; set; }
        public string? GithubProfileUrl { get; set; }
        [Required]
        public string? Comment { get; set; }
    }
}
