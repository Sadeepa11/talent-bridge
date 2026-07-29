using System;
using System.Collections.Generic;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class Grant
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public GrantScope Scope { get; set; }
        public GrantStatus Status { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public int? OrderId { get; set; }
        public int? SupersedesGrantId { get; set; }
        public int IssuedBy { get; set; }
        public int? RevokedBy { get; set; }
        public string? RevocationReason { get; set; }
        public int ExtensionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Batch? Batch { get; set; }
        public Company? Company { get; set; }
        public CandidateProfile? CandidateProfile { get; set; }
        public Order? Order { get; set; }
        public Grant? SupersededGrant { get; set; }
        public User? Issuer { get; set; }
        public List<AccessEvent> AccessEvents { get; set; } = new();
        public Outcome? Outcome { get; set; }
    }
}
