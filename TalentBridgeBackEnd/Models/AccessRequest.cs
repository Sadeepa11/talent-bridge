using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class AccessRequest
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CandidateProfileId { get; set; }
        public int? GrantId { get; set; }
        public AccessRequestStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
