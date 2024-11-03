using JobCandidateHubAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobCandidateHubAPI.Infrastructure.Persistence.Configurations
{
    public class JobCandidateConfiguration : IEntityTypeConfiguration<JobCandidate>
    {
        public void Configure(EntityTypeBuilder<JobCandidate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("jobcandidates");
            builder.Property(t => t.FirstName).HasMaxLength(255).IsRequired();
            builder.Property(t => t.LastName).HasMaxLength(255).IsRequired();
            builder.Property(t => t.Email).HasMaxLength(255).IsRequired();
            builder.Property(t => t.Comment).IsRequired();
            builder.Property(t => t.PhoneNumber).HasMaxLength(15);
            builder.Property(t => t.TimeInterval).HasMaxLength(15);
            builder.Property(x => x.LinkedinProfileUrl).HasMaxLength(255);
            builder.Property(x => x.GithubProfileUrl).HasMaxLength(255);
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
