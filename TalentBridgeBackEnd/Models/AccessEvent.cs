using System;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Models
{
    public class AccessEvent
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int CandidateProfileId { get; set; }
        public int? ProfileVersionId { get; set; }
        public AccessEventType EventType { get; set; }
        public int? DocumentId { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}
