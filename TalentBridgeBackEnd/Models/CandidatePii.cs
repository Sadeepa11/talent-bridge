using System;

namespace TalentBridgeBackEnd.Models
{
    public class CandidatePii
    {
        public int Id { get; set; }
        public int CandidateProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string NicNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string? PostalCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public CandidateProfile? CandidateProfile { get; set; }
    }
}
