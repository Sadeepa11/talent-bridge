using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public int? JobCategoryId { get; set; }
        public string PositionSought { get; set; } = string.Empty;
        public int YearsExperience { get; set; }
        public ExperienceBand ExperienceBand { get; set; }
        public string HighestQualification { get; set; } = string.Empty;
        public string MainCity { get; set; } = string.Empty;
        public Availability Availability { get; set; }
        public decimal? ExpectedSalaryMin { get; set; }
        public decimal? ExpectedSalaryMax { get; set; }
        public CandidateStatus Status { get; set; }
        public int CompletenessPct { get; set; }
        public DateTime LastActivityAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? ProfilePhotoBase64 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User? User { get; set; }
        public JobCategory? JobCategory { get; set; }
        public List<CandidateExperience> Experiences { get; set; } = new();
        public List<CandidateQualification> Qualifications { get; set; } = new();
        public List<CandidateSkill> Skills { get; set; } = new();
        public List<CandidateDocument> Documents { get; set; } = new();
        public List<Grant> Grants { get; set; } = new();
        public List<AccessEvent> AccessEvents { get; set; } = new();
        public List<ProfileVersion> ProfileVersions { get; set; } = new();
        public List<Consent> Consents { get; set; } = new();
        public List<CandidateCategory> Categories { get; set; } = new();
    }
}
