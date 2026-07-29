using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class CandidateQualification
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string QualificationName { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public string InstitutionDescriptor { get; set; } = string.Empty;
        public QualificationLevel Level { get; set; }
        public int YearCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
