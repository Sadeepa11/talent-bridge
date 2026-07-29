using System;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateExperience
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string EmployerName { get; set; } = string.Empty;
        public string EmployerDescriptor { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Responsibilities { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
