namespace JobCandidateHubAPI.Infrastructure.Entities
{
    public class JobCandidate
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? TimeInterval { get; set; }
        public string? LinkedinProfileUrl { get; set; }
        public string? GithubProfileUrl { get; set; }
        public string? Comment { get; set; }
    }
}
